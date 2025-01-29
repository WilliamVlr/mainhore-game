using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationBehavior : MonoBehaviour
{
    //Get confirmation panel UI
    GameObject panel;
    public Button yesButton;
    public Button noButton;
    public Button closeButton;
    public TextMeshProUGUI judulTxt;
    public TextMeshProUGUI pertanyaanTxt;

    // Initialize panel
    void Start()
    {
        panel = GetComponent<GameObject>();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showConfirmationPanel()
    {
        panel.SetActive(true);
    }

    public void closeConfirmationPanel()
    {
        panel.SetActive(false);
    }

    private bool confirm()
    {
        return true;
    }

    private bool cancel()
    {
        return false;
    }


}
