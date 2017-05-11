﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataModel : MonoBehaviour {

    public List<GameObject> CardObj;

    public GameObject GetCard(int cardNo)
    {
        return CardObj[cardNo];
    }

    public static int CardObjectMaxUnit {
        get
        {
            return 6;
        }
    }

    private static UsageResource _farmCard = new UsageResource();
    public static UsageResource FarmCardModel
    {
        get
        {
            int farmLevel = GameFormular.CalculateEXPToLv(GameResourceDataModel.BuildingResouces.farmExp);
            _farmCard.woodUsage = GameFormular.ForestCardWoodUsage(farmLevel);
            _farmCard.stoneUsage = GameFormular.ForestCardStoneUsage(farmLevel);

            return _farmCard;
        }
    }

    private static UsageResource _woodCutterCard = new UsageResource();
    public static UsageResource WoodCutterCardModel
    {
        get
        {
            int woodCutterLevel = GameFormular.CalculateEXPToLv(GameResourceDataModel.BuildingResouces.woodCutterExp);
            _woodCutterCard.woodUsage = 0;
            _woodCutterCard.stoneUsage = GameFormular.WoodCutterCardStoneUsage(woodCutterLevel);

            return _woodCutterCard;
        }
    }

    private static UsageResource _mineCard = new UsageResource();
    public static UsageResource MineCardModel
    {
        get
        {
            int mineLevel = GameFormular.CalculateEXPToLv(GameResourceDataModel.BuildingResouces.mineExp);
            _mineCard.woodUsage = GameFormular.MineCardWoodUsage(mineLevel);
            _mineCard.stoneUsage = 0;

            return _mineCard;
        }
    }

    private static UsageResource _townCard = new UsageResource();
    public static UsageResource TownCardModel
    {
        get
        {
            int townLevel = GameFormular.CalculateEXPToLv(GameResourceDataModel.BuildingResouces.townExp);
            _townCard.woodUsage = GameFormular.TownCardWoodUsage(townLevel);
            _townCard.stoneUsage = GameFormular.TownCardStoneUsage(townLevel);

            return _townCard;
        }
    }
}

[Serializable]
public class UsageResource
{
    public int woodUsage;
    public int stoneUsage;
}