using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HouseManager : MonoBehaviour, IDataPersistence
{
    [Header("Item Database Reference")]
    public SO_itemList itemDatabase; // Reference to the item database

    [Header("Layout Reference")]
    public GameObject decorationModeButton;       // Decoration Mode button
    public GameObject characterUI;                // Character-related UI (profile, coins, etc.)
    public GameObject otherButtons;
    public GameObject confirmationPanel;          // Confirmation panel for saving or resetting
    public GameObject exitPanel;
    private Animator inventoryAnimator;            // Animator for inventory UI

    [Header("Managers and Logic Object References")]
    public InventoryManager inventoryManager;     // Reference to InventoryManager
    public InventoryUI inventoryUI;               // Reference to InventoryUI
    public CameraMovement_DecorMode CameraMovement_decor;

    public Transform furnitureContainer; // Parent object for all placed furniture
    
    public Dictionary<string, Vector3> placedFurnitures = new Dictionary<string, Vector3>();
    public List<FurnitureBehavior> listFurnitureBehaviors = new List<FurnitureBehavior>();

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
            InventoryManager.Instance.OnUnpackFurniture.AddListener(PlaceFurniture);
        }

        isInDecorationMode = false;
        isFurnitureBeingDragged = false;
        decorationModeButton.GetComponent<Button>().onClick.AddListener(OnEnterDecorationMode);
        exitPanel.GetComponentInChildren<Button>().onClick.AddListener(OnExitDecorationMode);

        decorationModeButton.SetActive(true);
        characterUI.SetActive(true);
        otherButtons.SetActive(true);
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
            placedFurnitures.TryAdd(item.ID, newFurniture.transform.position);  // Add to the list of placed furniture
            listFurnitureBehaviors.Add(behavior);
            item.dropBehavior.HandleDrop(newFurniture);
        }
        else
        {
            Debug.LogWarning("Furniture prefab is not assigned in Item_SO!");
        }
    }

    public void RemoveFurniture(FurnitureBehavior furniture)
    {
        if (placedFurnitures.ContainsKey(furniture.furnitureData.ID))
        {
            List<FurnitureBehavior> furnitureChildren = furniture.getFurnitureBehaviorChildren();
            foreach (FurnitureBehavior child in furnitureChildren)
            {
                child.ResetSortingOrder();
            }
            placedFurnitures.Remove(furniture.furnitureData.ID);
            listFurnitureBehaviors.Remove(furniture);
            Destroy(furniture.gameObject);  // Destroy the furniture object
        }
    }

    public void RemoveAllFurnitures()
    {
        foreach (FurnitureBehavior furniture in listFurnitureBehaviors)
        {
            Destroy(furniture.gameObject);
        }
        listFurnitureBehaviors.Clear();
        placedFurnitures.Clear();
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
    }

    public void SaveFurniturePosition()
    {
        OnExitDecorationModeCleanup();
        DataPersistenceManager.Instance.saveGame();
    }

    public void ResetFurniturePosition()
    {
        OnExitDecorationModeCleanup();
        DataPersistenceManager.Instance.loadGame();
    }
    void OnExitDecorationModeCleanup()
    {
        isInDecorationMode = false;

        // reset all placed furnitures body type into Static
        foreach (FurnitureBehavior furniture in listFurnitureBehaviors)
        {
            furniture.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }

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

    public void LoadData(GameData data)
    {
        // Clear currect placed furnitures in house manager
        RemoveAllFurnitures();

        // Load dictionary data using for loop
        foreach (KeyValuePair<string, Vector3> pair in data.placedFurnitures)
        {
            string id = pair.Key;
            SO_item item = itemDatabase.GetItemByID(id);
            if (item != null && item is SO_Furniture)
            {
                SO_Furniture itemFurniture = (SO_Furniture)item;
                GameObject newFurniture = Instantiate(itemFurniture.furniturePrefab, pair.Value, Quaternion.identity, furnitureContainer);
                newFurniture.name = itemFurniture.itemName;

                // Initialize the furniture with the appropriate behavior
                FurnitureBehavior behavior = newFurniture.GetComponent<FurnitureBehavior>();
                if (behavior != null)
                {
                    behavior.Initialize(itemFurniture);
                }
                placedFurnitures.TryAdd(itemFurniture.ID, newFurniture.transform.position);  // Add to the list of placed furniture
                listFurnitureBehaviors.Add(behavior);
                itemFurniture.dropBehavior.HandleDrop(newFurniture);
            }

        }
    }

    public void SaveData(ref GameData data)
    {
        data.placedFurnitures.Clear();
        if (!isInDecorationMode)
        {
            updateFurniturePosition();
        }
        foreach (KeyValuePair<string, Vector3> pair in placedFurnitures)
        {
            data.placedFurnitures.TryAdd(pair.Key, pair.Value);
        }
    }

    private void updateFurniturePosition()
    {
        string[] keys = new string[placedFurnitures.Keys.Count];
        placedFurnitures.Keys.CopyTo(keys, 0);
        for (int j = 0; j <  keys.Length; j++)
        {
            string keyID = keys[j];
            for(int i = 0; i < listFurnitureBehaviors.Count; i++)
            {
                FurnitureBehavior fur = listFurnitureBehaviors[i];
                if (keyID.Equals(fur.furnitureData.ID))
                {
                    placedFurnitures[keyID] = fur.transform.position;
                }
            }
        }
    }
}
