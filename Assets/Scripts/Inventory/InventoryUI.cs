using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Canvas Group")]
    public CanvasGroup canvasGroup;

    [Header("Slots Prefabs and Container")]
    public GameObject slotPrefabFurniture; // Prefab for inventory slots
    public GameObject slotPrefabSkin;
    public Transform slotContainer; // Parent object for slots

    [Header("Inventory Texts")]
    public TextMeshProUGUI invMax;
    public TextMeshProUGUI invCapacity;
    public TextMeshProUGUI isEmpty;

    // Add references to the buttons that will filter the inventory
    [Header("Buttons")]
    public Button furnitureButton;
    public Button skinButton;
    public Button addSlotButton;
    public Color inactiveColor = Color.white;
    public Color activeColor = new Color(0.878f, 0.835f, 0.800f, 1f);

    public static bool isInventoryBeingDragged = false;
    public ScrollRect scrollRect;

    private void Start()
    {
        // Set up button click listeners
        furnitureButton.onClick.RemoveAllListeners();
        skinButton.onClick.RemoveAllListeners();
        addSlotButton.onClick.RemoveAllListeners();

        furnitureButton.onClick.AddListener(ToggleFurniture);
        skinButton.onClick.AddListener(ToggleSkins);
        addSlotButton.onClick.AddListener(OnAddSlotClicked);

        SetButtonInactive(furnitureButton);
        SetButtonInactive(skinButton);

        // Initially display all items (no filter)
        ShowAllItems();
        showCanvas();
        //ShowFurniture();

        // Attach listener for scroll events
        if (scrollRect != null)
        {
            // Detect if scrolling starts
            scrollRect.onValueChanged.AddListener(OnScroll);
        }
    }

    public void showCanvas()
    {
        Animator animatorInv = GetComponent<Animator>();
        if (animatorInv != null)
        {
            animatorInv.SetBool("isOpen", false);
        }
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void hideCanvas()
    {
        Animator animatorInv = GetComponent<Animator>();
        if (animatorInv != null)
        {
            animatorInv.SetBool("isOpen", false);
        }
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    // Refresh the UI slots
    public void RefreshInventory(List<SO_item> inventory, int max)
    {
        // Clear existing slots
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        if(InventoryManager.Instance.playerInventory.Count == 0)
        {
            isEmpty.gameObject.SetActive(true);
        } 
        else
        {
            isEmpty.gameObject.SetActive(false);
        }

        // Create new slots
        foreach (SO_item item in inventory)
        {
            GameObject slot = null;
            if (item is SO_Furniture)
            {
                slot = Instantiate(slotPrefabFurniture, slotContainer);
            } else
            {
                slot = Instantiate(slotPrefabSkin, slotContainer);
            }
            slot.GetComponent<SlotUI>().Setup(item);
        }

        invMax.text = InventoryManager.Instance.maxCapacity.ToString();
        invCapacity.text = InventoryManager.Instance.playerInventory.Count.ToString();

        SetButtonInactive(skinButton);
        SetButtonInactive(furnitureButton);
    }

    private void Update()
    {
        // Handle mobile touch and desktop mouse clicks
        if (IsGlobalClickDetected())
        {
            if (SlotUI.activeSlot != null)
            {
                SlotUI.activeSlot.OnSlotUntouched(); // Close the active slot
                SlotUI.activeSlot = null;           // Clear the active slot
            }
        }
    }

    // Detects global clicks or touches outside UI
    private bool IsGlobalClickDetected()
    {
        // For mobile: Check touch input
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            return !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        }

        // For desktop: Check mouse click
        if (Input.GetMouseButtonDown(0))
        {
            return !EventSystem.current.IsPointerOverGameObject();
        }

        return false;
    }

    public void ToggleFurniture()
    {
        if(furnitureButton.GetComponent<Image>().color == Color.white)
        {
            ShowFurniture();
        } 
        else
        {
            ShowAllItems();
        }
    }

    public void ToggleSkins()
    {
        if (skinButton.GetComponent<Image>().color == Color.white)
        {
            ShowSkins();
        }
        else
        {
            ShowAllItems();
        }
    }

    public void ShowFurniture()
    {
        // Get filtered furniture items and update the UI
        List<SO_item> furnitureItems = InventoryManager.Instance.GetFilteredInventory(typeof(SO_Furniture));
        RefreshInventory(furnitureItems, InventoryManager.Instance.maxCapacity);
        SetButtonActive(furnitureButton);
        SetButtonInactive(skinButton);
    }

    public void ShowSkins()
    {
        // Get filtered skin items and update the UI
        List<SO_item> skinItems = InventoryManager.Instance.GetFilteredInventory(typeof(SO_Skin));
        RefreshInventory(skinItems, InventoryManager.Instance.maxCapacity);
        SetButtonActive(skinButton);
        SetButtonInactive(furnitureButton);
    }

    private void ShowAllItems()
    {
        // Show all items if no filter is applied
        RefreshInventory(InventoryManager.Instance.playerInventory, InventoryManager.Instance.maxCapacity);
    }

    // Helper method to set a button to its active state (e.g., change color)
    private void SetButtonActive(Button button)
    {
        Image img = button.GetComponent<Image>();
        if (img != null)
        {
            img.color = activeColor;
        }
        //ColorBlock colorBlock = button.colors;
        //colorBlock.normalColor = activeColor;
        //button.colors = colorBlock;
    }

    // Helper method to set a button to its inactive state (e.g., revert color)
    private void SetButtonInactive(Button button)
    {
        Image img = button.GetComponent<Image>();
        if (img != null)
        {
            img.color = inactiveColor;
        }
        //ColorBlock colorBlock = button.colors;
        //colorBlock.normalColor = inactiveColor;
        //button.colors = colorBlock;
    }

    public void OnAddSlotClicked()
    {
        ConfirmationBehavior confirmPanel = FindAnyObjectByType<ConfirmationBehavior>();

        if( confirmPanel != null )
        {
            confirmPanel.showConfirmBuyInv(
                InventoryManager.Instance.CalculateUpgradeCost(),
                InventoryManager.Instance.PurchaseInventorySlot,
                () => Debug.Log("Cancel upgrade inventory")
                );
        }
        else
        {
            Debug.LogWarning("Confirmation Panel not found!");
        }

    }

    // Detect if inventory UI is being dragged/swiped
    private void SetInventoryDragState(bool state)
    {
        isInventoryBeingDragged = state;
    }

    private void OnScroll(Vector2 scrollPosition)
    {
        // Set the dragging state to true when the user is interacting with the scroll view
        if (scrollRect.velocity.x != 0) 
        {
            SetInventoryDragState(true);
        }
        else
        {
            SetInventoryDragState(false);
        }
    }

    public void toggleInventory()
    {
        Animator animatorInv = GetComponent<Animator>();
        if(animatorInv != null)
        {
            if (animatorInv.GetBool("isOpen"))
            {
                animatorInv.SetBool("isOpen", false);
            } 
            else
            {
                animatorInv.SetBool("isOpen", true);
            }
        }
    }
}
