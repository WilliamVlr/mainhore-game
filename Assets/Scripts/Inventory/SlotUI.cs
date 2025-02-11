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
        //Debug.Log("Touched slot " + currentItem.name);
        //Debug.Log("Active slot old is " + activeSlot.name);
        if (activeSlot != null && activeSlot != this)
        {
            activeSlot.OnSlotUntouched(); // Hide the previously active slot
        }

        activeSlot = this; // Mark this slot as active
        //Debug.Log("Active slot new is " + activeSlot.name);
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
        // Highlight the slot background
        backgroundImageClicked.gameObject.SetActive(true);

        // Show item name and sell button
        itemLabel.SetActive(true);
        sellButton.gameObject.SetActive(true);
    }

    // Called to un-highlight the slot
    public void OnSlotUntouched()
    {
        backgroundImageClicked.gameObject.SetActive(false); // Hide clicked layout by default
        itemLabel.SetActive(false); // Hide item name by default
        sellButton.gameObject.SetActive(false); // Hide sell button by default
        secondButton.gameObject.SetActive(false); // Hide sell button by default
    }

    // Sell the item
    private void onSellItem()
    {
        ConfirmationBehavior confirmationPanel = FindAnyObjectByType<ConfirmationBehavior>();

        if (confirmationPanel != null)
        {
            confirmationPanel.showConfirmSellingPanel(
                currentItem,
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
        InventoryManager.Instance.RemoveItem(currentItem);
    }

    public abstract void secondAction();
}
