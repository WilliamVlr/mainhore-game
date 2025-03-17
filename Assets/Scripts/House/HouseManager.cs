using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HouseManager : MonoBehaviour, IDataPersistence
{
    [Header("Item Database Reference")]
    public SO_itemList itemDatabase; // Reference to the item database

    [Header("Layout Reference")]
    public GameObject decorationModeButton;       // Decoration Mode button
    public Camera MainCamera;
    public CanvasBehavior staticCanvas;
    public CanvasBehavior coinCanvas;
    public CanvasBehavior joystickCanvas;
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
        inventoryAnimator = inventoryUI.GetComponent<Animator>();
    }

    public void Start()
    {
        
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnUnpackFurniture.AddListener(PlaceFurniture);
        }

        isInDecorationMode = false;
        isFurnitureBeingDragged = false;
        decorationModeButton.GetComponent<Button>().onClick.AddListener(OnEnterDecorationMode);
        exitPanel.GetComponentInChildren<Button>().onClick.AddListener(OnExitDecorationMode);

        decorationModeButton.SetActive(true);
        coinCanvas.showCanvas();
        staticCanvas.showCanvas();
        joystickCanvas.showCanvas();
        exitPanel.SetActive(false);
        SoundManager.Instance.PlayMusicInList("House");
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
            float topSpawnMargin = 0.25f; // 10% margin below the top edge
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
            placedFurnitures.TryAdd(item.ID, newFurniture.transform.localPosition);  // Add to the list of placed furniture
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
        joystickCanvas.hideCanvas();

        // Hide character and non-house UI
        coinCanvas.hideCanvas();
        staticCanvas.hideCanvas();
        decorationModeButton.SetActive(false);
        exitPanel.SetActive(true);
        CameraZoomOut();

        //Set up inventory
        inventoryUI.ShowFurniture();
        inventoryUI.skinButton.gameObject.SetActive(false);

        // Trigger the Inventory open animation
        inventoryAnimator.SetBool("isOpen", true);

        // Store initial furniture state (positions) before any changes
        //StoreOriginalFurnitureData();
        UnfreezeFurnitures();

        CameraMovement_decor.onEnterDecorationMode();
    }

    public void CameraZoomIn()
    {
        StartCoroutine(ZoomInCoroutine());
    }

    public void CameraZoomOut()
    {
        StartCoroutine(ZoomOutCoroutine());
    }

    private IEnumerator ZoomInCoroutine()
    {
        Animator CameraAnimator = MainCamera.GetComponent<Animator>();
        CameraAnimator.enabled = true;
        CameraAnimator.SetBool("ZoomOut", false);

        // Wait until the animation finishes
        yield return new WaitForSeconds(1f);

        MainCamera.orthographicSize = 5f;

        //foreach (FurnitureBehavior furniture in listFurnitureBehaviors)
        //{
        //    if (furniture.furnitureData.dropBehavior is CeilingSnapBehavior_SO)
        //    {
        //        furniture.transform.position = new Vector3(furniture.transform.position.x, MainCamera.orthographicSize - 3, furniture.transform.position.z);
        //    }
        //}
    }

    private IEnumerator ZoomOutCoroutine()
    {
        Animator CameraAnimator = MainCamera.GetComponent<Animator>();
        CameraAnimator.enabled = true;

        CameraAnimator.SetBool("ZoomOut", true);

        // Wait until the animation finishes
        yield return new WaitForSeconds(1f);

        MainCamera.orthographicSize = 5.7f;

        CameraAnimator.enabled = false;

        //foreach (FurnitureBehavior furniture in listFurnitureBehaviors)
        //{
        //    if (furniture.furnitureData.dropBehavior is CeilingSnapBehavior_SO)
        //    {
        //        furniture.transform.position = new Vector3(furniture.transform.position.x, MainCamera.orthographicSize - 3, furniture.transform.position.z);
        //    }
        //}
    }

    public void UnfreezeFurnitures()
    {
        foreach (FurnitureBehavior furniture in listFurnitureBehaviors)
        {
            furniture.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void FreezeFurnitures()
    {
        foreach (FurnitureBehavior furniture in listFurnitureBehaviors)
        {
            furniture.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
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
        joystickCanvas.showCanvas();
        CameraZoomIn();

        // reset all placed furnitures body type into Static
        foreach (FurnitureBehavior furniture in listFurnitureBehaviors)
        {
            furniture.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }

        // Show character and non-house UI again
        coinCanvas.showCanvas();
        staticCanvas.showCanvas();
        exitPanel.SetActive(false);

        // Trigger the Inventory close animation
        inventoryAnimator.SetBool("isOpen", false);
        inventoryUI.skinButton.gameObject.SetActive(true);

        // Show the Decoration Mode button again
        decorationModeButton.SetActive(true);

        CameraMovement_decor.onExitDecorationMode();
    }

    public void AdjustInitialLayer()
    {
        foreach(FurnitureBehavior behavior in listFurnitureBehaviors)
        {
            behavior.AdjustInitialOverlap();
        }
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
                GameObject newFurniture = Instantiate(itemFurniture.furniturePrefab, Vector3.zero, Quaternion.identity, furnitureContainer);
                newFurniture.transform.localPosition = pair.Value;

                newFurniture.name = itemFurniture.itemName;

                // Initialize the furniture with the appropriate behavior
                FurnitureBehavior behavior = newFurniture.GetComponent<FurnitureBehavior>();
                if (behavior != null)
                {
                    behavior.Initialize(itemFurniture);
                }
                placedFurnitures.TryAdd(itemFurniture.ID, newFurniture.transform.localPosition);  // Add to the list of placed furniture
                Debug.Log("Loaded furniture: " + itemFurniture.name + " position: " + newFurniture.transform.localPosition.x + ", " + newFurniture.transform.localPosition.y);
                listFurnitureBehaviors.Add(behavior);
                //itemFurniture.dropBehavior.HandleDrop(newFurniture);
                behavior.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            }
        }

        AdjustInitialLayer();
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
            //Debug.Log("Furniture name: " + pair.Key + " pos: " + pair.Value.x + ", " + pair.Value.y);
        }
        data.mainBackgroundPos = new Vector3(11, 0, 0);
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
                if(fur != null)
                {
                    if (keyID.Equals(fur.furnitureData.ID))
                    {
                        Transform currentParent = fur.transform.parent;
                        fur.transform.SetParent(furnitureContainer, true);
                        placedFurnitures[keyID] = fur.transform.localPosition;
                        fur.transform.SetParent(currentParent, true);
                    }
                }
            }
        }
    }
}
