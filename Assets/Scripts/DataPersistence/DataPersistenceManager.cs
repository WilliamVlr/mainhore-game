using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData gameData;

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
        loadGame();
    }

    public void newGame()
    {
        this.gameData = new GameData();
    }

    public void loadGame()
    {
        // TODO - load any saved data from a file using data handler


        // if no data can be loaded, initialize a new game (for development only)
        if(this.gameData == null)
        {
            // ini harus diganti nanti, kalo gaada game datanya ya jangan di load gamenya.
            // Ini utk development purpose only
            Debug.Log("No data was found. Initializing data to defaults");
            newGame();
        }

        // TODO - push the loaded data to all other scripts that need it
    }

    public void saveGame()
    {
        // TODO - pass the data to other script so they can handle it

        // TODO - save that data to a file using the data handler

    }

    private void OnApplicationQuit()
    {
        saveGame();
    }
}
