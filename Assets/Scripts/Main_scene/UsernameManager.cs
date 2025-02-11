using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsernameManager : MonoBehaviour, IDataPersistence //attached to username UI text field
{
    private Text UsernameText;

    private void Awake()
    {
        UsernameText = GetComponent<Text>();
    }

    public void LoadData(GameData data)
    {
        UsernameText.text = data.username;
    }

    public void SaveData(ref GameData data)
    {
        data.username = UsernameText.text;
    }
    public void setUsername(string _username)
    {
        UsernameText.text = _username;
    }

    public string getUsername()
    {
        if(UsernameText == null)
        {
            return "Default Username";
        }
        return UsernameText.text;
    }
}
