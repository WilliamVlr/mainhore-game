using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    public Image backgroundImageDefault;  // Reference to the background image default
    public Image backgroundImageClicked;  // Reference to the background image clicked
    public Image icon;             // Icon of the item
    public TextMeshProUGUI itemName;          // Name of the item
    public Button sellButton;      // Sell button
    public Button unpackButton;      // Sell button
    private SO_item currentItem;   // The item assigned to this slot

    private void Start()
    {
        OnSlotUntouched();

        // Assign the sell and unpack button behavior
        sellButton.onClick.AddListener(SellItem);
        unpackButton.onClick.AddListener(unpackItem);
    }

    // Set up the slot with an item
    public void Setup(SO_item item)
    {
        currentItem = item;
        icon.sprite = item.sprite;
        itemName.text = item.itemName;
    }

    // Called when slot is touched
    public void OnSlotTouched()
    {
        Debug.Log("Touched: " + currentItem.itemName);

        // Highlight the slot background
        backgroundImageClicked.gameObject.SetActive(true);

        // Show item name and sell button
        itemName.gameObject.SetActive(true);
        sellButton.gameObject.SetActive(true);
        unpackButton.gameObject.SetActive(true);
    }

    // Called to un-highlight the slot
    public void OnSlotUntouched()
    {
        backgroundImageClicked.gameObject.SetActive(false); // Hide clicked layout by default
        itemName.gameObject.SetActive(false); // Hide item name by default
        sellButton.gameObject.SetActive(false); // Hide sell button by default
        unpackButton.gameObject.SetActive(false); // Hide sell button by default
    }

    // Sell the item
    private void SellItem()
    {
        Debug.Log("Selling: " + currentItem.itemName);
        FindObjectOfType<InventoryManager>().RemoveItem(currentItem);
        // Optionally, add logic to refund in-game currency
    }

    //Place item in house
    private void unpackItem()
    {
        Debug.Log("Placing: " + currentItem.itemName);
        FindObjectOfType<InventoryManager>().RemoveItem(currentItem);
        // add logic to instantiate game object and add the item to house inventory
    }
}
