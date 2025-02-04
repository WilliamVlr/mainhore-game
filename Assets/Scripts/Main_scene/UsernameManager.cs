using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsernameManager : MonoBehaviour
{
    public GameObject Username;

    public void setUsername(string _username)
    {
        Text UsernameText = Username.GetComponent<Text>();
        if (UsernameText != null)
        {
            UsernameText.text = _username; // set the text
        }
        else
        {
            Debug.LogError("Text component not found on Username GameObject!");
        }
    }

    public string getUsername()
    {
        Text UsernameText = Username.GetComponent<Text>();
        if (UsernameText != null)
        {
            //Debug.Log(UsernameText.text);
            return UsernameText.text; // Return the text value
        }
        else
        {
            Debug.LogError("Text component not found on Username GameObject!");
            return "Username"; // Return default value
        }
    }
}
