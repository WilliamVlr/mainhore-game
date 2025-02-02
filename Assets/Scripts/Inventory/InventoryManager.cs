using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance; // Singleton instance
    public List<SO_item> playerInventory = new List<SO_item>(); // Bag inventory
    public SO_itemList itemDatabase; // Reference to the item database
    public UnityEvent<SO_Furniture> OnUnpackFurniture = new UnityEvent<SO_Furniture>();  // Event to trigger furniture unpacking

    public int maxCapacity = 10; // Maximum inventory capacity
    private int upgradeCount = 0; // Number of times the player has upgraded the inventory size
    private const int initialUpgradeCost = 300; // Initial cost for the first upgrade
    private const int slotsPerUpgrade = 5; // Number of slots added per upgrade

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy this object if there's already an existing instance
        }
        else
        {
            Instance = this; // Set this as the singleton instance
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }
    private void Start()
    {
        // Check if InventoryUI exists in the scene
        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.RefreshInventory(playerInventory, maxCapacity); // Only call if InventoryUI exists
        }
        else
        {
            Debug.LogWarning("InventoryUI not found in the scene. Make sure to add it if needed.");
        }
    }

    // Add an item to the inventory
    public bool AddItem(SO_item item)
    {
        if (playerInventory.Count >= maxCapacity)
        {
            Debug.Log("Inventory is full! Expand your inventory by purchasing more space.");
            return false; // Return false if inventory is full
        }

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
            Debug.Log("Removed " + item.itemName + " (from Inventory Manager)");
            playerInventory.Remove(item);
            Debug.Log("Player inventory capacity: " + playerInventory.Capacity);
            Debug.Log("Count: " + playerInventory.Count);
            UpdateUI();
            InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
            if (item is SO_Furniture)
            {
                inventoryUI.ShowFurniture();
            } else
            {
                inventoryUI.ShowSkins();
            }
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
        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.RefreshInventory(playerInventory, maxCapacity); // Only update if InventoryUI exists
        }
    }

    public List<SO_item> GetFilteredInventory(System.Type itemType)
    {
        List<SO_item> filteredItems = new List<SO_item>();

        // Loop through the player's inventory and check the type of each item
        foreach (SO_item item in playerInventory)
        {
            if (item.GetType() == itemType)
            {
                filteredItems.Add(item); // Add item if it matches the desired type
            }
        }

        return filteredItems;
    }

    // Purchase more inventory space with coins
    public bool PurchaseInventorySlot()
    {
        // Calculate the cost of the next upgrade
        int nextUpgradeCost = CalculateUpgradeCost();
        //bool enoughCoin = PlayerManager.Instance.SubtractCoins(nextUpgradeCost);
        bool enoughCoin = true;

        // Check if the player has enough coins
        if (enoughCoin)
        {
            maxCapacity += slotsPerUpgrade; // Increase the inventory capacity by 5 slots
            upgradeCount++; // Increment the number of upgrades purchased
            Debug.Log("Purchased " + slotsPerUpgrade + " inventory slots for " + nextUpgradeCost + " coins!");
            return true;
        }
        else
        {
            Debug.Log("Not enough coins to purchase more slots!");
            return false;
        }
    }

    // Calculate the cost for the next inventory upgrade
    private int CalculateUpgradeCost()
    {
        if (upgradeCount == 0)
        {
            return initialUpgradeCost; // First upgrade cost is 300 coins
        }
        else
        {
            // Multiply the cost by 2.5 for each subsequent upgrade
            return Mathf.CeilToInt(initialUpgradeCost * Mathf.Pow(2.5f, upgradeCount));
        }
    }
}
