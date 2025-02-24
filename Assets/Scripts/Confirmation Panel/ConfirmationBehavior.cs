using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmationBehavior : MonoBehaviour
{
    [Header("Decoration Confirmation")]
    public GameObject decorationConfirmation;

    [Header("Jual Confirmation")]
    public GameObject jualConfirmation;
    public TextMeshProUGUI jualBeliTxt;
    public TextMeshProUGUI hargaTxt;
    public Image objJual;

    [Header("Upgrade Inv Confirmation")]
    public GameObject upgradeInvConfirmation;
    public TextMeshProUGUI hargaUpgradeTxt;

    [Header("Confirmation Buttons")]
    public Button yesButton;
    public Button noButton;
    public Button closeButton;

    // This will hold the actions to be triggered on Yes/No button click
    private UnityAction yesAction;
    private UnityAction noAction;

    private CanvasGroup confirmCanvasGroup;

    private void Awake()
    {
        confirmCanvasGroup = transform.parent.transform.parent.GetComponent<CanvasGroup>();
    }

    // Initialize panel
    void Start()
    {
        hideCanvas();
        closeButton.onClick.AddListener(closeConfirmationPanel);
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);

        resetPanel();
    }

    public void showCanvas()
    {
        confirmCanvasGroup.alpha = 1;
        confirmCanvasGroup.interactable = true;
        confirmCanvasGroup.blocksRaycasts = true;
    }

    public void hideCanvas()
    {
        confirmCanvasGroup.alpha = 0;
        confirmCanvasGroup.interactable = false;
        confirmCanvasGroup.blocksRaycasts = false;
    }

    public void showConfirmSellingPanel(SO_item item, UnityAction onYes, UnityAction onNo)
    {
        // Activate selling panel first
        jualConfirmation.gameObject.SetActive(true);

        jualBeliTxt.text = "Jual untuk";
        hargaTxt.text = item.price.ToString();  // Set the price text
        objJual.sprite = item.sprite;
        objJual.SetNativeSize();
        float height = objJual.GetComponent<RectTransform>().rect.height;

        if (height > 600f)
        {
            objJual.rectTransform.localScale = new Vector3(0.25f, 0.25f, 0);
        }
        else
        {
            objJual.rectTransform.localScale = new Vector3(0.4f, 0.4f, 0);
        }

        // Assign the actions to the buttons
        yesAction = onYes;
        noAction = onNo;

        // Force layout update on all children inside the 'judulJualGroup'
        foreach (Transform child in jualConfirmation.transform)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(child.GetComponent<RectTransform>());
        }

        showCanvas();
    }

    public void showConfirmBuyingPanel(SO_item item, UnityAction onYes, UnityAction onNo)
    {
        // Activate selling panel first
        jualConfirmation.gameObject.SetActive(true);

        jualBeliTxt.text = "Beli untuk";
        hargaTxt.text = item.price.ToString();  // Set the price text
        objJual.sprite = item.sprite;
        objJual.SetNativeSize();
        float height = objJual.GetComponent<RectTransform>().rect.height;

        if (height > 600f)
        {
            objJual.rectTransform.localScale = new Vector3(0.25f, 0.25f, 0);
        }
        else
        {
            objJual.rectTransform.localScale = new Vector3(0.4f, 0.4f, 0);
        }

        // Assign the actions to the buttons
        yesAction = onYes;
        noAction = onNo;

        // Force layout update on all children inside the 'judulJualGroup'
        foreach (Transform child in jualConfirmation.transform)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(child.GetComponent<RectTransform>());
        }

        showCanvas();
    }


    public void showConfirmDecorationPanel(UnityAction onYes, UnityAction onNo)
    {
        // Assign the actions to the buttons
        yesAction = onYes;
        noAction = onNo;

        decorationConfirmation.SetActive(true);

        showCanvas();
    }

    public void showConfirmBuyInv(int price, UnityAction onYes, UnityAction onNo)
    {
        yesAction = onYes;
        noAction = onNo;

        hargaUpgradeTxt.text = price.ToString();

        upgradeInvConfirmation.SetActive(true);

        // Force layout update on all children inside the 'judulJualGroup'
        foreach (Transform child in upgradeInvConfirmation.transform)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(child.GetComponent<RectTransform>());
        }

        showCanvas();
    }


    public void closeConfirmationPanel()
    {
        hideCanvas();
        resetPanel();
    }

    private void resetPanel()
    {
        objJual.sprite = null;
        decorationConfirmation.SetActive(false);
        jualConfirmation.SetActive(false);
        upgradeInvConfirmation.SetActive(false);
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
