using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeMenu : MonoBehaviour
{
    [Header("Home Screen Menu")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;

    private void Start()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
        {
            continueButton.interactable = false;
        }
    }

    public void onNewGameClicked()
    {
        disableMenuButtons();

        DataPersistenceManager.Instance.newGame();

        DataPersistenceManager.Instance.saveGame();
        
        SceneManager.LoadSceneAsync("House");
    }

    public void onContinueClicked()
    {
        disableMenuButtons();

        DataPersistenceManager.Instance.saveGame();

        SceneManager.LoadSceneAsync("House");
    }

    private void disableMenuButtons()
    {
        newGameButton.interactable = false;
        continueButton.interactable = false;
    }
}
