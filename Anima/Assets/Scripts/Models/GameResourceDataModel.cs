using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class GameResourceDataModel {

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
    public int woodCutterExp;
    public int mineExp;
    public int farmExp;
    public int townExp;
}

[Serializable]
public class NaturalResource
{
    public int waterExp;
    public int forestExp;
}