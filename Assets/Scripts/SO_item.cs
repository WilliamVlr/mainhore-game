using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item/Default")]
public class SO_item : ScriptableObject
{
    [SerializeField, ReadOnly] private string id;
    public string ID => id;
    public string itemName;
    public Sprite sprite;
    public int price;
    public string desc;
    public Place place;

    // Called automatically in the Editor when the asset is modified or created
    private void OnValidate()
    {
        #if UNITY_EDITOR
        // Ensure the ID matches the asset name
        string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this);
        if (!string.IsNullOrEmpty(assetPath))
        {
            string assetName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
            if (id != assetName)
            {
                id = assetName;
            }
        }
        #endif
    }
}

public enum Place
{
    Global,
    Cafe,
    Groceries,
}

