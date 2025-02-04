using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmationBehavior : MonoBehaviour
{
    //Get confirmation panel UI
    //GameObject panel;
    public Button yesButton;
    public Button noButton;
    public Button closeButton;
    public GameObject judulDekorGroup;
    public GameObject judulJualGroup;
    public GameObject pertanyaanDekor;
    public GameObject imgContainerObjJual;
    public Image objJual;
    public TextMeshProUGUI hargaTxt;

    // This will hold the actions to be triggered on Yes/No button click
    private UnityAction yesAction;
    private UnityAction noAction;

    // Initialize panel
    void Start()
    {
        //panel = gameObject;

        gameObject.SetActive(false);
        closeButton.onClick.AddListener(closeConfirmationPanel);
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);

        resetPanel();
    }

    public void showConfirmSellingPanel(int harga, Sprite itemImg, UnityAction onYes, UnityAction onNo)
    {
        // Activate selling panel first
        judulJualGroup.SetActive(true);
        imgContainerObjJual.SetActive(true);

        hargaTxt.text = harga.ToString();  // Set the price text
        objJual.sprite = itemImg;
        objJual.SetNativeSize();
        float height = objJual.GetComponent<RectTransform>().rect.height;

        if (height > 500f)
        {
            objJual.rectTransform.localScale = new Vector3(0.18f, 0.18f, 0);
        }
        else
        {
            objJual.rectTransform.localScale = new Vector3(0.3f, 0.3f, 0);
        }

        // Assign the actions to the buttons
        yesAction = onYes;
        noAction = onNo;

        gameObject.SetActive(true);

        // Force layout update on all children inside the 'judulJualGroup'
        foreach (Transform child in judulJualGroup.transform)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(child.GetComponent<RectTransform>());
        }

    }


    public void showConfirmDecorationPanel(UnityAction onYes, UnityAction onNo)
    {
        // Assign the actions to the buttons
        yesAction = onYes;
        noAction = onNo;

        judulDekorGroup.SetActive(true);
        pertanyaanDekor.SetActive(true);

        gameObject.SetActive(true);
    }


    public void closeConfirmationPanel()
    {
        resetPanel();
        gameObject.SetActive(false);
    }

    private void resetPanel()
    {
        objJual.sprite = null;
        judulDekorGroup.SetActive(false);
        pertanyaanDekor.SetActive(false);
        judulJualGroup.SetActive(false);
        imgContainerObjJual.SetActive(false);
    }

    private void OnYesClicked()
    {
        yesAction?.Invoke();  // Only invoke if yesAction is not null
        closeConfirmationPanel();
    }

    private void OnNoClicked()
    {
        noAction?.Invoke();  // Only invoke if noAction is not null
        closeConfirmationPanel();
    }

}
