using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool intializeDataIfNull = false; //true only in development via inspector

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHandler;

    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            //Debug.LogError("Found more than one Data Persistence in the scene");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= onSceneLoaded;
    }

    public void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded called");
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        loadGame();
    }

    public void newGame(string usn="Player", string skin="Fox")
    {
        this.gameData = new GameData(usn, skin);
    }

    public void loadGame()
    {
        this.gameData = dataHandler.Load();

        if(this.gameData == null && intializeDataIfNull)
        {
            newGame();
        }

        if(this.gameData == null)
        {
            Debug.Log("No data was found. A New game needs to be started before data can be loaded.");
            return;
        }

        // push the loaded data to all other scripts that need it
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void saveGame()
    {
        Debug.Log("Save game Data Persistence Manager called");
        if(this.gameData == null)
        {
            Debug.LogWarning("No data was found. A new game must be started before data can be saved.");
            return;
        }

        // pass the data to other script so they can handle it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        // save that data to a file using the data handler
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        saveGame();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            saveGame();
        }
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        //foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        //{
        //    Debug.Log(dataPersistenceObj.ToString());
        //}

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return this.gameData != null;
    }
}
