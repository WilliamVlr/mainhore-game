using System.Collections.Generic;
using UnityEngine;

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
}
