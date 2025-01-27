using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item/Furniture")]
public class SO_Furniture : SO_item
{
    public int sellPrice;
    public float scale_inSlot;
    public float scale_inBackground;

    public DropBehavior_SO dropBehavior;
}
