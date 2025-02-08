using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string username;
    public int coinAmount;
    //public string activeSkin;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData()
    {
        this.username = "Ganteng Nomor 1 Duar";
        this.coinAmount = 0;
        //this.activeSkin = "Fox";
    }
}
