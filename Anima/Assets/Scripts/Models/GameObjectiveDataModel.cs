using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectiveDataModel : MonoBehaviour {

	public static string CurrentGameObjective { get; set; }
    public static string CurrentSubGameObjetive { get; set; }
    public static int CurrentPopulationObjective { get; set; }

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
        { GameObjectiveModel.FarmMax, "Get maximum 'Farm' lv." },
        { GameObjectiveModel.ForestMax, "Get maximum 'Forest' lv." },
        { GameObjectiveModel.MineMax, "Get maximum 'Mine' lv." },
        { GameObjectiveModel.TownMax, "Get maximum 'Town' lv." },
        { GameObjectiveModel.WoodCutterMax, "Get maximum 'Wood cutter' lv."},
    };

    public static Dictionary<string, string> GameSubObjeciveDescription = new Dictionary<string, string>()
    {
        { GameObjectiveModel.FarmMax, "Get 'Farm' lv. 2" },
        { GameObjectiveModel.ForestMax, "Get 'Forest' lv. 2" },
        { GameObjectiveModel.MineMax, "Get 'Mine' lv. 2" },
        { GameObjectiveModel.TownMax, "Get 'Town' lv. 2" },
        { GameObjectiveModel.WoodCutterMax, "Get Wood cutter' lv. 2"},
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
