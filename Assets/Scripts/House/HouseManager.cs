using System.Collections.Generic;
using Unity.VisualScripting;
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
    public CameraMovement_DecorMode CameraMovement_decor;
    private Animator inventoryAnimator;            // Animator for inventory UI

    public Transform furnitureContainer; // Parent object for all placed furniture
    public List<FurnitureBehavior> placedFurniture = new List<FurnitureBehavior>();  // List of placed furniture

    private bool isInDecorationMode;
    public bool IsInDecorationMode { get => isInDecorationMode; set => isInDecorationMode = value; }

    // Boolean flag to indicate if any furniture is being dragged
    [DoNotSerialize]
    public bool isFurnitureBeingDragged;

    private void Awake()
    {
        // Subscribe to the InventoryManager's event
        inventoryManager = FindObjectOfType<InventoryManager>();
        inventoryAnimator = inventoryUI.GetComponent<Animator>();
    }

    public void Start()
    {
        
        if (inventoryManager != null)
        {
            inventoryManager.OnUnpackFurniture.AddListener(PlaceFurniture);
        }

        isInDecorationMode = false;
        isFurnitureBeingDragged = false;
        decorationModeButton.GetComponent<Button>().onClick.AddListener(OnEnterDecorationMode);
        exitPanel.GetComponentInChildren<Button>().onClick.AddListener(OnExitDecorationMode);

        decorationModeButton.SetActive(true);
        characterUI.SetActive(true);
        otherButtons.SetActive(true);
        //confirmationPanel.SetActive(false);  
        exitPanel.SetActive(false);
    }

    public void PlaceFurniture(SO_Furniture item)
    {
        if (item.furniturePrefab != null)
        {
            // Get the Camera (Main Camera in this case)
            Camera mainCamera = Camera.main;

            // Calculate the camera's visible bounds in world space
            Vector3 cameraBottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
            Vector3 cameraTopRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

            // Define a range for the top spawn position, e.g., 10% below the top of the camera view
            float topSpawnMargin = 0.1f; // 10% margin below the top edge
            float leftRightSpawnMargin = 0.15f;

            // Get random position within the camera bounds
            float randomX = Random.Range(cameraBottomLeft.x + (cameraTopRight.x - cameraBottomLeft.x) * leftRightSpawnMargin, cameraTopRight.x - (cameraTopRight.x - cameraBottomLeft.x) * leftRightSpawnMargin);
            float randomY = Random.Range(cameraTopRight.y - (cameraTopRight.y - cameraBottomLeft.y) * topSpawnMargin, cameraTopRight.y);

            // Create the random position
            Vector3 randomPosition = new Vector3(randomX, randomY, 0); // Assuming it's a 2D game, so z = 0

            // Instantiate the furniture at the random position
            GameObject newFurniture = Instantiate(item.furniturePrefab, randomPosition, Quaternion.identity, furnitureContainer);
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
        inventoryAnimator.SetBool("isOpen", true);

        // Store initial furniture state (positions) before any changes
        //StoreOriginalFurnitureData();

        CameraMovement_decor.onEnterDecorationMode();
    }

    public void OnExitDecorationMode()
    {
        // Show the confirmation panel (overlay)
        //confirmationPanel.SetActive(true);
        //ConfirmationManager confMng = FindAnyObjectByType<ConfirmationManager>();
        ConfirmationBehavior confirmationBehavior = confirmationPanel.GetComponent<ConfirmationBehavior>();

        if (confirmationBehavior != null)
        {
            confirmationBehavior.showConfirmDecorationPanel(
                SaveFurniturePosition,
                ResetFurniturePosition
            );
        }
        else
        {
            Debug.Log("Confirmation panel not found!");
        }

        placedFurniture.ForEach(furniture =>
        {
            furniture.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        });
    }

    public void SaveFurniturePosition()
    {
        OnExitDecorationModeCleanup();
    }

    public void ResetFurniturePosition()
    {
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
        inventoryAnimator.SetBool("isOpen", false);
        inventoryUI.skinButton.gameObject.SetActive(true);

        // Show the Decoration Mode button again
        decorationModeButton.SetActive(true);

        CameraMovement_decor.onExitDecorationMode();
    }
}
