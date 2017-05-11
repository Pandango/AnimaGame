using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataModel : MonoBehaviour {
    public static PlayerInGameData PlayerInGameData { get; set; }
    public static PlayerInGameData[] ClientsInGameData { get; set; }

    public static int CardsInHandUnit { get; set; }



    public static int ClientUnit
    {
        get { return ClientsInGameData.Length; }
    }

}

[Serializable]
public class PlayerInGameData
{
    public string username;
    public int role;
    public int score;
}