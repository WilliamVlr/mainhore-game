using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item")]
public class SO_item : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public int price;
    public string desc;
}

