using UnityEditor;
using UnityEngine;

public class SOItemEditor : AssetPostprocessor
{
    private static readonly string idFormat = "{0:D4}"; // Pattern: 0001, 0002, 0003...

    // Automatically assigns IDs to imported assets
    static void OnPostprocessAllAssets(
        string[] importedAssets)
    {
        foreach (string assetPath in importedAssets)
        {
            SO_item item = AssetDatabase.LoadAssetAtPath<SO_item>(assetPath);
            if (item != null && string.IsNullOrEmpty(item.itemId))
            {
                AssignUniqueID(item);
            }
        }
    }

    // Assign IDs to all existing assets missing an ID
    [MenuItem("Tools/Sync SO_item IDs")]
    public static void SyncAllItemIDs()
    {
        // Find all SO_item assets
        string[] guids = AssetDatabase.FindAssets($"t:{nameof(SO_item)}");
        int highestNumber = GetHighestExistingNumber(guids);

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            SO_item item = AssetDatabase.LoadAssetAtPath<SO_item>(path);

            if (item != null && string.IsNullOrEmpty(item.itemId))
            {
                // Assign a new ID with the incremented number
                highestNumber++;
                item.itemId = string.Format(idFormat, highestNumber);

                // Mark the asset as dirty to save changes
                EditorUtility.SetDirty(item);
                Debug.Log($"Assigned ID '{item.itemId}' to {item.name}");
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("All missing IDs have been synchronized.");
    }

    // Helper to calculate the highest existing ID number
    private static int GetHighestExistingNumber(string[] guids)
    {
        int highestNumber = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            SO_item item = AssetDatabase.LoadAssetAtPath<SO_item>(path);

            if (item != null && !string.IsNullOrEmpty(item.itemId))
            {
                // Extract the numeric part of the ID
                if (int.TryParse(item.itemId, out int number))
                {
                    highestNumber = Mathf.Max(highestNumber, number);
                }
            }
        }

        return highestNumber;
    }

    // Assign a unique ID to a single SO_item
    private static void AssignUniqueID(SO_item item)
    {
        string[] guids = AssetDatabase.FindAssets($"t:{nameof(SO_item)}");
        int highestNumber = GetHighestExistingNumber(guids) + 1;

        item.itemId = string.Format(idFormat, highestNumber);
        EditorUtility.SetDirty(item);
        AssetDatabase.SaveAssets();
    }
}
