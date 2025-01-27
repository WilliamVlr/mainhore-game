using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab; // Prefab for inventory slots
    public Transform slotContainer; // Parent object for slots

    // Refresh the UI slots
    public void RefreshInventory(List<SO_item> inventory)
    {
        // Clear existing slots
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        // Create new slots
        foreach (SO_item item in inventory)
        {
            GameObject slot = Instantiate(slotPrefab, slotContainer);
            slot.GetComponent<SlotUI>().Setup(item);
        }
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
}
