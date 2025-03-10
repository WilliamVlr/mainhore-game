using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class FurnitureRandomizer : MonoBehaviour
{
    [Header("Small Box References")]
    public GameObject smallBox;

    [Header("Floor Box References")]
    public GameObject floorBox;

    [Header("Ceiling Box References")]
    public GameObject ceilingBox;

    [Header("Wall Box References")]
    public GameObject wallBox;
    
    [Header("Item Database")]
    public SO_itemList itemDatabase;

    private void Start()
    {
        RandomizeObjects();
        SoundManager.Instance.PlayMusicInList("Furnitur");
    }

    private void RandomizeObjects()
    {
         // Create a list to store all furniture items
        List<SO_Furniture> allFurniture = new List<SO_Furniture>();

        // Create separate lists for each type of DropBehavior
        List<SO_Furniture> weightedFloorFurniture = new List<SO_Furniture>();
        List<SO_Furniture> weightedSmallFurniture = new List<SO_Furniture>();
        List<SO_Furniture> wallSnapFurniture = new List<SO_Furniture>();
        List<SO_Furniture> ceilingSnapFurniture = new List<SO_Furniture>();

        // Loop through all items in the itemDatabase and filter out the furniture items
        foreach (SO_item item in itemDatabase.availItems)
        {
            if (item is SO_Furniture furniture)
            {
                // Add the furniture to the allFurniture list
                allFurniture.Add(furniture);

                // Check the dropBehavior type and categorize the furniture accordingly
                if (furniture.dropBehavior is WeightedDropBehavior_SO)
                {
                    if(furniture.desc == "Kecil"){
                        weightedSmallFurniture.Add(furniture);
                    }
                    else{
                        weightedFloorFurniture.Add(furniture);
                    }
                    
                }
                else if (furniture.dropBehavior is WallSnapBehavior_SO)
                {
                    wallSnapFurniture.Add(furniture);
                }
                else if (furniture.dropBehavior is CeilingSnapBehavior_SO)
                {
                    ceilingSnapFurniture.Add(furniture);
                }
            }
        }

        // Log the number of items found in each category
        //Debug.Log($"Total Furniture: {allFurniture.Count}");
        //Debug.Log($"Weighted Drop Small Furniture: {weightedSmallFurniture.Count}");
        //Debug.Log($"Weighted Drop Floor Furniture: {weightedFloorFurniture.Count}");
        //Debug.Log($"Wall Snap Furniture: {wallSnapFurniture.Count}");
        //Debug.Log($"Ceiling Snap Furniture: {ceilingSnapFurniture.Count}");

        // Randomly select an item for each category and set the corresponding image
        if (weightedSmallFurniture.Count > 0)
        {
            SO_Furniture selectedFurniture = weightedSmallFurniture[Random.Range(0, weightedSmallFurniture.Count)];
            smallBox.GetComponent<ItemTrigger>().SetItem(selectedFurniture);  // Set the furniture in small box
            // smallBoxImage.sprite = selectedFurniture.furniturePrefab.GetComponent<SpriteRenderer>().sprite;  // Set the image
        }
        else
        {
            Debug.LogError("No WeightedDropBehavior furniture found!");
        }

        
         if (weightedFloorFurniture.Count > 0)
        {
            SO_Furniture selectedFurniture = weightedFloorFurniture[Random.Range(0, weightedFloorFurniture.Count)];
            floorBox.GetComponent<ItemTrigger>().SetItem(selectedFurniture);  // Set the furniture in small box
            // smallBoxImage.sprite = selectedFurniture.furniturePrefab.GetComponent<SpriteRenderer>().sprite;  // Set the image
        }
        else
        {
            Debug.LogError("No WeightedDropBehavior furniture found!");
        }


        if (wallSnapFurniture.Count > 0)
        {
            SO_Furniture selectedFurniture = wallSnapFurniture[Random.Range(0, wallSnapFurniture.Count)];
            wallBox.GetComponent<ItemTrigger>().SetItem(selectedFurniture);  // Set the furniture in wall box
            // wallBoxImage.sprite = selectedFurniture.furniturePrefab.GetComponent<SpriteRenderer>().sprite;  // Set the image
        }
        else
        {
            Debug.LogError("No WallSnapBehavior furniture found!");
        }

        if (ceilingSnapFurniture.Count > 0)
        {
            SO_Furniture selectedFurniture = ceilingSnapFurniture[Random.Range(0, ceilingSnapFurniture.Count)];
            ceilingBox.GetComponent<ItemTrigger>().SetItem(selectedFurniture);  // Set the furniture in ceiling box
            // ceilingBoxImage.sprite = selectedFurniture.furniturePrefab.GetComponent<SpriteRenderer>().sprite;  // Set the image
        }
        else
        {
            Debug.LogError("No CeilingSnapBehavior furniture found!");
        }
    }
}

