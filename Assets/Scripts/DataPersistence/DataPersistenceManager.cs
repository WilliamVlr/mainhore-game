using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        loadGame();
    }

    public void newGame()
    {
        this.gameData = new GameData();
    }

    public void loadGame()
    {
        // load any saved data from a file using data handler
        this.gameData = dataHandler.Load();

        // if no data can be loaded, initialize a new game (for development only)
        if(this.gameData == null)
        {
            // ini harus diganti nanti, kalo gaada game datanya ya jangan di load gamenya.
            // Ini utk development purpose only
            Debug.Log("No data was found. Initializing data to defaults");
            newGame();
        }

        // push the loaded data to all other scripts that need it
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void saveGame()
    {
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

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
