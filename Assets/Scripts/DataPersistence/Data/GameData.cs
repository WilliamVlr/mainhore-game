using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string username;
    public int coinAmount;
    public string activeSkin;

    public JsonableListWrapper<string> inventoryItemsID;
    public int inventoryMaxCap;

    public JsonableDictionaryWrapper<string, Vector3> placedFurnitures;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData() //TODO - add parameter string _usn from user input and chosen initial skin
    {
        // TODO - set username to username the user typed
        this.username = "Ganteng Nomor 1 Duar";

        //TODO - set user active skin to chosen skin
        this.activeSkin = "Fox";

        // Set initial default value for each player attributes
        this.coinAmount = 0;
        this.inventoryItemsID = new JsonableListWrapper<string>();
        this.inventoryMaxCap = 10;
        this.placedFurnitures = new JsonableDictionaryWrapper<string, Vector3>();
    }
}
