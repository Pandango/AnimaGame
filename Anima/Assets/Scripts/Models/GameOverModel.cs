using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverModel : MonoBehaviour {
    public static bool isMissionComplete;
    public static string description;
}

public static class SituationDescription
{
    public const string
        Drought = "DROUGHT",
        Flood = "FLOOD";
}
