using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlotUI_Furniture : SlotUI
{
    public override void OnSlotTouched()
    {
        base.OnSlotTouched();
        if(SceneManager.GetActiveScene().name == "House")
        {
            secondButton.gameObject.SetActive(true);
        }
    }
    public override void secondAction()
    {
        unpackItem();
    }

    //Place item in house
    private void unpackItem()
    {
        Debug.Log("Placing: " + currentItem.itemName);
        //FindObjectOfType<InventoryManager>().RemoveItem(currentItem);
        // add logic to instantiate game object and add the item to house inventory
    }
}
