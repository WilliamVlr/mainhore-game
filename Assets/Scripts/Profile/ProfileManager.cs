using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour, IDataPersistence
{
    //define TextMeshPro Input Field private SerializeField "inputField"
    //define Button private SerializeField "inputButton"

    [SerializeField] private TMP_InputField inputField;  
    [SerializeField] private Button inputButton;  
    [SerializeField] private Text usernameUI;
    [SerializeField] private Image imgChar;
    [SerializeField] private SO_itemList itemDatabase;
    [SerializeField] private TextMeshProUGUI namaSkin;
    // private string profileName;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure inputField and inputButton are assigned in the Inspector
        if (inputField != null && inputButton != null)
        {
            inputButton.onClick.AddListener(OnButtonClick);  // Add a listener to the button's click event
        }
    }

    public void OnButtonClick()
    {
        if (inputField != null)
        {
            // Select and activate the input field so the user can edit their username immediately
            inputField.Select();  // Selects the input field
            inputField.ActivateInputField();  // Activates the input field and opens the keyboard if on mobile

            Debug.Log("Input field selected for editing.");
        }
    }

    public bool isEmptyText(){
        return inputField.text == "";
    }

    public void SaveUsername()
    {
        DataPersistenceManager.Instance.saveGame();
    }


    public void LoadData(GameData data)
    {
        //load username to inputField.text
          if (data != null && inputField != null)
        {
            inputField.text = data.username;  // Set the input field text to the saved username
            Debug.Log("Profile Name Loaded: " + data.username);
        }

        string skinID = data.activeSkin;
        SO_item item = itemDatabase.GetItemByID(skinID);

        if(item != null && item is SO_Skin)
        {
            imgChar.sprite = item.sprite;
            namaSkin.text = item.itemName;
        }
        else
        {
            Debug.LogWarning("Can't load skin!");
        }
    }

    public void SaveData(ref GameData data)
    {
        //save inputField.text to data.username
        if (data != null && !isEmptyText())
        {
            data.username = inputField.text;  // Save the username from inputField to data
            usernameUI.text = inputField.text;
            Debug.Log("Profile Name Saved: " + data.username);
        } else if (isEmptyText()){
            inputField.text = data.username;
        }
    }

    public void returnToHome(){
        DataPersistenceManager.Instance.saveGame();
        SceneManager.LoadSceneAsync("Home Screen");
    }


}
