using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewItem", menuName = "ScriptableObject/Item")]
public class SO_Item : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public int price;
    public ItemLocation place; // Enum untuk jenis item
}
public enum ItemLocation
{
    Cafe,
    Grocery,
    Boutique,
    Global,
}