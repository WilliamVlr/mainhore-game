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
        AvatarManager avatarMng = FindAnyObjectByType<AvatarManager>();
        if (avatarMng != null)
        {
            SO_Skin oldSkin = avatarMng.changeSkin((SO_Skin)currentItem);
            InventoryManager.Instance.RemoveItem(currentItem);
            InventoryManager.Instance.AddItem(oldSkin);
        }
        else
        {
            Debug.LogWarning("Avatar Manager not found! Maybe it is inactive or not in this scene!");
        }
    }
}
