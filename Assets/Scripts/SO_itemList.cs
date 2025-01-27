using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "itemList", menuName = "ScriptableObjects/itemList")]
public class SO_itemList : ScriptableObject
{
    public List<SO_item> availItems;

    public SO_item GetItemByID(string id)
    {
        return availItems.Find(item => item.ID == id);
    }
}
