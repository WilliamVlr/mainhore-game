using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<SO_item> playerInventory = new List<SO_item>(); // Bag inventory
    public SO_itemList itemDatabase; // Reference to the item database

    // Add an item to the inventory
    public bool AddItem(SO_item item)
    {
        if (!playerInventory.Contains(item))
        {
            playerInventory.Add(item);
            UpdateUI();
            return true;
        }
        Debug.Log("Item already in inventory!");
        return false;
    }

    // Remove an item from the inventory
    public void RemoveItem(SO_item item)
    {
        if (playerInventory.Contains(item))
        {
            playerInventory.Remove(item);
            UpdateUI();
        }
    }

    // Save inventory to a list of IDs
    public List<string> SaveInventory()
    {
        List<string> savedIDs = new List<string>();
        foreach (SO_item item in playerInventory)
        {
            savedIDs.Add(item.ID);
        }
        return savedIDs;
    }

    // Load inventory from a list of IDs
    public void LoadInventory(List<string> savedIDs)
    {
        playerInventory.Clear();
        foreach (string id in savedIDs)
        {
            SO_item item = itemDatabase.GetItemByID(id);
            if (item != null)
            {
                playerInventory.Add(item);
            }
        }
        UpdateUI();
    }

    // Update the inventory UI
    private void UpdateUI()
    {
        // Call the UI manager to refresh slots
        FindObjectOfType<InventoryUI>().RefreshInventory(playerInventory);
    }
}
