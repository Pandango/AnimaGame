using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFormular : MonoBehaviour {
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

    public static int ForestCardWoodUsage(int level)
    {
        return 100 + (100 * level);
    }

    public static int ForestCardStoneUsage(int level)
    {
        return 50 + (50 * level);
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
        return 50 + (50 * level);
    }

    public static int TownCardStoneUsage(int level)
    {
        return 100 + (100 * level);
    }

    public static int CalPopFoodBalanced(int populationUnit, int foodUnit)
    {
        int balancedPercent = (foodUnit / (populationUnit * 3)) * 100;
        return balancedPercent;

    }
}
