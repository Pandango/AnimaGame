using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour {

    public static int MaximumSecondsPerTurn = 30;
    public static int MaximumAllowLevel = 4;
    public static int RequireSubObjectiveLevel = 2;
    public static int GamePopulationObjective = 2000;

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

    public static int GetRandomUnitOfCard()
    {
        int random = UnityEngine.Random.Range(1, 3);
        return random;
    }

    public static int RandomCard()
    {
        int random = UnityEngine.Random.Range(0, CardDataModel.CardObjectMaxUnit);
        return random;
    }

    public static int RandomGameObjective()
    {
        int randonEvent = UnityEngine.Random.Range(0, GameObjectiveDataModel.GameObjectiveTypeUnit);
        return randonEvent;
    }

    public static int RandomGameSubObjective(int usedGameObjectiveId)
    {
        int randomEvent = 0;
        bool isUsaged = true;

        while (isUsaged)
        {
            randomEvent = UnityEngine.Random.Range(0, GameObjectiveDataModel.GameObjectiveTypeUnit);
            isUsaged = (randomEvent == usedGameObjectiveId);
        }
        return randomEvent;
    }

    public static int PopulationObjective()
    {
        int requirePopulation = GamePopulationObjective;
        return requirePopulation;
    }

    public static string FormatTimer(int timeLeft, string type)
    {
        System.TimeSpan time = System.TimeSpan.FromSeconds(timeLeft);

        string formatSecond = string.Format("{0:00}", time.Seconds);
        return formatSecond;   
    }

    public static string GenerateGameObjectiveDescription(string gameObjectiveKey)
    {
        string description = GameObjectiveDataModel.GameObjeciveDescription[gameObjectiveKey];        
        return description;
    }

    public static string GenerateSubObjectiveDescription(string gameObjectiveKey)
    {
        string description = GameObjectiveDataModel.GameSubObjeciveDescription[gameObjectiveKey];
        return description;
    }

    public static string GeneratePopulationObjectiveDescription(int populationUnit)
    {
        string description = "Get " + populationUnit.ToString() + " population";
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