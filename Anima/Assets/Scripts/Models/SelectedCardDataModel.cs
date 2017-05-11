using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCardDataModel : MonoBehaviour {
    public static string SelectedCardKeyName;
}

[Serializable]
public class SendUsagedGameData
{
    public GetBuildingEXP getBuildingExp = new GetBuildingEXP();
    public GetNaturalExp getNaturalExp = new GetNaturalExp();
    public GetResource getResource = new GetResource();
}

[Serializable]
public class GetBuildingEXP
{
    public int woodCutterGetExp = 0;
    public int mineGetExp = 0;
    public int farmGetExp = 0;
    public int townGetExp = 0;
}

[Serializable]
public class GetNaturalExp
{
    public int forestGetExp = 0;
    public int waterGetExp = 0;
}

[Serializable]
public class GetResource
{
    public int stoneUnit = 0;
    public int woodUnit = 0;
}