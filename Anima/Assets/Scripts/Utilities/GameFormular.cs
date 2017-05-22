using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFormular : MonoBehaviour {

    public static float ConvertPercentToRatio(float percent)
    {
        return percent / 100f;
    }

    public static int CalculateEXPToLv(int EXP)
    {
        int Lv = EXP / 3;
        return Lv;
    }

    public static int RemainEXPGateAfterCalculateLv(int EXP)
    {
        int RemainEXP = EXP % 3;
        return RemainEXP;
    }

    public static int FarmCardWoodUsage(int level)
    {
        return 100 + (100 * level);
    }

    public static int FarmtCardStoneUsage(int level)
    {
        return 100 + (100 * level);
    }

    public static int WoodCutterCardStoneUsage(int level)
    {
        return 200 + (200 * level);
    }

    public static int MineCardWoodUsage(int level)
    {
        return 200 + (200 * level);
    }

    public static int TownCardWoodUsage(int level)
    {
        return 100 + (100 * level);
    }

    public static int TownCardStoneUsage(int level)
    {
        return 100 + (100 * level);
    }

    public static float CalPopFoodBalanced(int populationUnit, int foodUnit)
    {
        float balancedPercent = (foodUnit / (populationUnit * 3f)) * 100f;
        return balancedPercent;

    }
}

public static class GameOverCauseByWaterLvStat
{
    public static int WaterLevelDesolated = 3;
    public static int WaterLevelOverDesolated = 1;

    public static int WaterLevelFlood = 7;
    public static int WaterLevelOverFlood = 9;
}
