using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResourceDataModel : MonoBehaviour {
    public static PopulationFoodBalanced PopulationFood { get; set; }

    public static SharingResource SharingResources { get; set; }

    public static BuildingResource BuildingResouces { get; set; }

    public static NaturalResource NaturalResources { get; set; }
}

[Serializable]
public class PopulationFoodBalanced
{
    public int population;
    public int food;
}

[Serializable]
public class SharingResource
{
    public int wood;
    public int stone;
}

[Serializable]
public class BuildingResource
{
    public int WoodCutterExp;
    public int MineExp;
    public int FarmExp;
    public int TownExp;
}

[Serializable]
public class NaturalResource
{
    public int wateLv;
    public int forestExp;
}