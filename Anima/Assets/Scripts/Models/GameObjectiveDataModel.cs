using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectiveDataModel : MonoBehaviour {

	public static string CurrentGameObjective { get; set; }

    public static int GameObjectiveTypeUnit
    {
        get { return GameObjectiveList.Count; }     
    }

    public static List<string> GameObjectiveList = new List<string> {
        GameObjectiveModel.FarmMax,
        GameObjectiveModel.ForestMax,
        GameObjectiveModel.WoodCutterMax,
        GameObjectiveModel.MineMax,
        GameObjectiveModel.TownMax
    };

    public static Dictionary<string, string> GameObjeciveDescription = new Dictionary<string, string>()
    {
        { GameObjectiveModel.FarmMax, "Get maximum 'Farm' level." },
        { GameObjectiveModel.ForestMax, "Get maximum 'Forest' level." },
        { GameObjectiveModel.MineMax, "Get maximum 'Mine' level." },
        { GameObjectiveModel.TownMax, "Get maximum 'Town' level." },
        { GameObjectiveModel.WoodCutterMax, "Get maximum 'Wood cutter' level."},
    };
}

public static class GameObjectiveModel
{
    public const string
        FarmMax = "FARMMAX",
        ForestMax = "FORESTMAX",
        WoodCutterMax = "WOODCUTTERMAX",
        MineMax = "MINEMAX",
        TownMax = "TOWNMAX";
}
