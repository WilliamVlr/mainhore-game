using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="itemList.asset", menuName ="ScriptableObject/itemList")]
public class SO_itemList : ScriptableObject
{
    public List<SO_Item> items;
}
