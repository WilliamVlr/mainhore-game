using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlotUI_Skin : SlotUI
{
    public override void OnSlotTouched()
    {
        base.OnSlotTouched();
        secondButton.gameObject.SetActive(true);
    }
    public override void secondAction()
    {
        useSkin();
    }

    private void useSkin()
    {
        Debug.Log("Memakai: " + currentItem.itemName);
        //FindObjectOfType<InventoryManager>().RemoveItem(currentItem);
    }
}
