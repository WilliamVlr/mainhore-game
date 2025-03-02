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
    public JsonableDictionaryWrapper<string, int> minigamesProgress;
    public Vector3 mainBackgroundPos;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData(string usn, string skin)
    {
        // TODO - set username to username the user typed
        this.username = usn;

        //TODO - set user active skin to chosen skin
        this.activeSkin = skin;

        // Set initial default value for each player attributes
        this.coinAmount = 0;
        this.inventoryItemsID = new JsonableListWrapper<string>();
        this.inventoryMaxCap = 10;
        this.placedFurnitures = new JsonableDictionaryWrapper<string, Vector3>();
        this.minigamesProgress = new JsonableDictionaryWrapper<string, int>()
        {
            {"cuci tangan", 0},
            {"cafe", 0},
        };
        this.mainBackgroundPos = new Vector3(11, 0, 0);
    }
}
