using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item/Default")]
public class SO_item : ScriptableObject
{
    public string itemId;
    public string itemName;
    public Sprite sprite;
    public int price;
    public string desc;
    public Place place;
}

public enum Place
{
    Global,
    Cafe,
    Groceries,
}

