using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public Transform furnitureContainer; // Parent object for all placed furniture
    public List<FurnitureBehavior> placedFurniture = new List<FurnitureBehavior>();  // List of placed furniture

    public void Start()
    {
        // Subscribe to the InventoryManager's event
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null)
        {
            inventoryManager.OnUnpackFurniture.AddListener(PlaceFurniture);
        }
    }

    public void PlaceFurniture(SO_Furniture item)
    {
        if (item.furniturePrefab != null)
        {
            // Instantiate the furniture prefab in the house
            GameObject newFurniture = Instantiate(item.furniturePrefab, furnitureContainer);
            newFurniture.name = item.itemName;

            // Initialize the furniture with the appropriate behavior
            FurnitureBehavior behavior = newFurniture.GetComponent<FurnitureBehavior>();
            if (behavior != null)
            {
                behavior.Initialize(item);
            }
            placedFurniture.Add(behavior);  // Add to the list of placed furniture
            item.dropBehavior.HandleDrop(newFurniture);
        }
        else
        {
            Debug.LogWarning("Furniture prefab is not assigned in Item_SO!");
        }
    }

    public void RemoveFurniture(FurnitureBehavior furniture)
    {
        if (placedFurniture.Contains(furniture))
        {
            placedFurniture.Remove(furniture);
            Destroy(furniture.gameObject);  // Destroy the furniture object
        }
    }
}
