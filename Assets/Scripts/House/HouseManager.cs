using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseManager : MonoBehaviour
{
    public GameObject decorationModeButton;       // Decoration Mode button
    public GameObject characterUI;                // Character-related UI (profile, coins, etc.)
    public GameObject otherButtons;
    public InventoryManager inventoryManager;     // Reference to InventoryManager
    public InventoryUI inventoryUI;               // Reference to InventoryUI
    public GameObject confirmationPanel;          // Confirmation panel for saving or resetting
    public GameObject exitPanel;
    private Animator inventoryAnimator;            // Animator for inventory UI

    public Transform furnitureContainer; // Parent object for all placed furniture
    public List<FurnitureBehavior> placedFurniture = new List<FurnitureBehavior>();  // List of placed furniture

    private bool isInDecorationMode;
    public bool IsInDecorationMode { get => isInDecorationMode; set => isInDecorationMode = value; }

    public void Start()
    {
        // Subscribe to the InventoryManager's event
        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null)
        {
            inventoryManager.OnUnpackFurniture.AddListener(PlaceFurniture);
            inventoryAnimator = inventoryUI.GetComponent<Animator>();
        }

        isInDecorationMode = false;
        decorationModeButton.GetComponent<Button>().onClick.AddListener(OnEnterDecorationMode);
        exitPanel.GetComponentInChildren<Button>().onClick.AddListener(OnExitDecorationMode);

        decorationModeButton.SetActive(true);
        characterUI.SetActive(true);
        otherButtons.SetActive(true);
        confirmationPanel.SetActive(false);  
        exitPanel.SetActive(false);
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

    public void OnEnterDecorationMode()
    {
        isInDecorationMode = true;

        // Hide character and non-house UI
        characterUI.SetActive(false);
        decorationModeButton.SetActive(false);
        otherButtons.SetActive(false);
        exitPanel.SetActive(true);

        //Set up inventory
        inventoryUI.ShowFurniture();
        inventoryUI.skinButton.gameObject.SetActive(false);

        // Trigger the Inventory open animation
        inventoryAnimator.SetTrigger("Open");

        // Store initial furniture state (positions) before any changes
        //StoreOriginalFurnitureData();
    }

    public void OnExitDecorationMode()
    {
        // Show the confirmation panel (overlay)
        confirmationPanel.SetActive(true);
    }

    public void ConfirmExitDecorationMode(bool saveChanges)
    {
        if (saveChanges)
        {
            // Save the positions of all placed furniture
            //SaveFurniturePositions();
        }
        else
        {
            // Reset to original furniture positions
            //ResetFurniturePositions();
        }

        // Hide the confirmation panel after decision
        confirmationPanel.SetActive(false);

        // Reset to house state (exit decoration mode)
        OnExitDecorationModeCleanup();
    }

    void OnExitDecorationModeCleanup()
    {
        isInDecorationMode = false;

        // Show character and non-house UI again
        characterUI.SetActive(true);
        otherButtons.SetActive(true);
        exitPanel.SetActive(false);

        // Trigger the Inventory close animation
        inventoryAnimator.SetTrigger("Close");
        inventoryUI.skinButton.gameObject.SetActive(true);

        // Show the Decoration Mode button again
        decorationModeButton.SetActive(true);
    }
}
