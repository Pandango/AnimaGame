using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverModel {
    public static bool IsMainMissionComplete;
    public static string MainMissionDescription;

    public static bool IsSubMissionComplete;
    public static string SubMissionDescription;

    public static bool IsPopulationComplete;
    public static string PopulationMissionDescription;

    public static bool IsMissionComplete
    {
        get
        {
            bool isMissionComplete = IsMainMissionComplete && IsSubMissionComplete && IsPopulationComplete;
            return isMissionComplete;
        }
    }
    public static string MissionDescription;
}

public static class SituationDescription
{
    public const string
        Famine = "ผู้คนล้มตายเนื่องจากอาหารขาดแคลนอย่างมาก",
        Desolation = "ผู้คนล้มตาย เนื่องจากขาดแคลนน้ำ",
        Flood = "ไม่มีการบริหารน้ำที่ดีจึงน้ำท่วม บ้านเรือนเสียหาย ผู้คนไม่สามารถอาศัยได้",
        Complete = "ยินดีด้วย คุณรักษาโลกใบนี้ไว้ได้!";
}
