using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour {

    public static int MaximumMiliSecondsPerTurn = 8000;
    public static int MaximumAllowLevel = 4;

    public static int StartCardUnit
    {
        get
        {
            return 5;
        }
    }

    public static string FormatDisplayLv(int level)
    {
        return "LV. " + level.ToString();
    }

    public static string FormatResourceUnit(int unit)
    {
        return "X " + unit.ToString();
    }

    public static int RandomCard()
    {
        int random = UnityEngine.Random.Range(0, CardDataModel.CardObjectMaxUnit);
        return random;
    }

    public static int RandomEventAfterEndTurn()
    {
        int randonEvent = UnityEngine.Random.Range(0, GameObjectiveDataModel.GameObjectiveTypeUnit);
        return randonEvent;
    }

    public static string FormatTimer(int timeLeft, string type)
    {
        System.TimeSpan time = System.TimeSpan.FromMilliseconds(timeLeft);
        string formatMili = string.Format("{0:000}",time.Milliseconds);
        string formatSecond = string.Format("{0:00}", time.Seconds);
        if (type == "mili")
        {
            return formatMili;
        }
        else
        {
            return formatSecond;
        }    
    }

    public static string GenerateGameObjectiveDescription(string gameObjectiveKey)
    {
        string description = GameObjectiveDataModel.GameObjeciveDescription[gameObjectiveKey];        
        return description;
    }

    public static SendingGameResource GenerateSendingGameResourceDataObj()
    {
        SendingGameResource sendingGameRes = new SendingGameResource();
        sendingGameRes.populationFoodBalanced = GameResourceDataModel.PopulationFood;
        sendingGameRes.sharingResource = GameResourceDataModel.SharingResources;
        sendingGameRes.buildingResource = GameResourceDataModel.BuildingResouces;
        sendingGameRes.naturalResource = GameResourceDataModel.NaturalResources;
        return sendingGameRes;
    }
}

[Serializable]
public class SendingGameResource
{
    public PopulationFoodBalanced populationFoodBalanced = new PopulationFoodBalanced();
    public SharingResource sharingResource = new SharingResource();
    public BuildingResource buildingResource = new BuildingResource();
    public NaturalResource naturalResource = new NaturalResource();
}