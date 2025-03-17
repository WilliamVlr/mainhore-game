using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlotUI_Furniture : SlotUI
{
    public override void OnSlotTouched()
    {
        base.OnSlotTouched();
        HouseManager house = FindObjectOfType<HouseManager>();
        if (house != null && house.IsInDecorationMode)
        {
            secondButton.gameObject.SetActive(true);
            //sellButton.gameObject.SetActive(false);
        }
    }
    public override void secondAction()
    {
        unpackItem();
    }

    //Place item in house
    private void unpackItem()
    {
        FindObjectOfType<InventoryManager>().OnUnpackFurniture.Invoke(currentItem as SO_Furniture);
        InventoryManager.Instance.RemoveItem(currentItem);
    }
}
