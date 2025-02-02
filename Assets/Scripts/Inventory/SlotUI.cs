using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SlotUI : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
{
    public Image backgroundImageDefault;  // Reference to the background image default
    public Image backgroundImageClicked;  // Reference to the background image clicked
    public Image icon;             // Icon of the item
    public GameObject itemLabel;
    public TextMeshProUGUI itemName;          // Name of the item
    public Button sellButton;      // Sell button
    public Button secondButton;      // Second button
    protected SO_item currentItem;   // The item assigned to this slot
    public static SlotUI activeSlot; // To keep track of the currently active slot
    //public SO_item currentItem;

    private void Start()
    {
        OnSlotUntouched();
        //Setup(currentItem);

        // Assign the sell and unpack button behavior
        sellButton.onClick.AddListener(onSellItem);
        secondButton.onClick.AddListener(secondAction);
    }

    // Set up the slot with an item
    public void Setup(SO_item item)
    {
        currentItem = item;
        icon.sprite = item.sprite;
        RectTransform rect = icon.GetComponent<RectTransform>();
        //Nanti ini bisa disesuaikan stlh Minigamenya kelar semua ya
        if (item is SO_Furniture furniture)
        {
            icon.SetNativeSize();
            rect.localScale = new Vector3(furniture.scale_inSlot, furniture.scale_inSlot, furniture.scale_inSlot);
        }
        else if (item is SO_Skin skin)
        {
            rect.localScale = new Vector3(skin.scale_inSlot, skin.scale_inSlot, skin.scale_inSlot);
        }

        itemName.text = item.itemName;
    }

    // Called when the slot is touched or clicked (pointer down event)
    public void OnPointerDown(PointerEventData eventData)
    {
        if (activeSlot != null && activeSlot != this)
        {
            activeSlot.OnSlotUntouched(); // Hide the previously active slot
        }

        activeSlot = this; // Mark this slot as active
        OnSlotTouched();
    }

    // Called when the pointer exits the slot (for hover effects)
    public void OnPointerExit(PointerEventData eventData)
    {
        //OnSlotUntouched();  // Un-highlight the slot when the pointer exits
    }

    // Called when slot is touched
    public virtual void OnSlotTouched()
    {
        Debug.Log("Touched: " + currentItem.itemName);

        // Highlight the slot background
        backgroundImageClicked.gameObject.SetActive(true);

        // Show item name and sell button
        itemLabel.gameObject.SetActive(true);
        sellButton.gameObject.SetActive(true);
    }

    // Called to un-highlight the slot
    public void OnSlotUntouched()
    {
        backgroundImageClicked.gameObject.SetActive(false); // Hide clicked layout by default
        itemLabel.gameObject.SetActive(false); // Hide item name by default
        sellButton.gameObject.SetActive(false); // Hide sell button by default
        secondButton.gameObject.SetActive(false); // Hide sell button by default
    }

    // Sell the item
    private void onSellItem()
    {
        ConfirmationManager confMng = FindAnyObjectByType<ConfirmationManager>();
        ConfirmationBehavior confirmationPanel = confMng.confirmationPanel;

        if (confirmationPanel != null)
        {
            confirmationPanel.showConfirmSellingPanel(
                currentItem.price,
                currentItem.sprite,
                () => confirmSell(),
                () => Debug.Log("Cancel selling")
            );
        } else
        {
            Debug.Log("Confirmation panel not found!");
        }

    }

    private void confirmSell()
    {
        Debug.Log("Selling: " + currentItem.itemName);
        FindObjectOfType<InventoryManager>().RemoveItem(currentItem);
    }

    public abstract void secondAction();
}
