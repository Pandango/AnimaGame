﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataModel : MonoBehaviour {
    public static PlayerInGameData PlayerInGameData { get; set; }
    public static PlayerInGameData[] ClientsInGameData { get; set; }

    public static int CardsInHandUnit { get; set; }

    public static GameCurrentTurnData gameCurrentTurnData { get; set; }

    public static int ClientUnit
    {
        get { return ClientsInGameData.Length; }
    }

    public static string RoundEvent { get; set; }

    public static bool IsFirstTurn = true;
    public static bool IsFirstPlayerInNewRound = false;
}

[Serializable]
public class PlayerInGameData
{
    public string username;
    public int role;
    public int score;
}

[Serializable]
public class GameCurrentTurnData
{
    public int turnNo;
    public string playerNameInCurrentTurn;
}