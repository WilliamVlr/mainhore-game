using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialDataManager : MonoBehaviour
{
    private string username;
    private string skin;
    public static InitialDataManager Instance;

    public string Username { get => username; set => username = value; }
    public string Skin { get => skin; set => skin = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void createNewGame()
    {
        DataPersistenceManager.Instance.newGame(Username, Skin);

        DataPersistenceManager.Instance.saveGame();

        SceneManager.LoadScene("House");
    }
    
}