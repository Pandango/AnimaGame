using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour {

    public static int MaximumMiliSecondsPerTurn = 8000;

    public static int StartCardUnit
    {
        get
        {
            return 5;
        }
    }

    public static string FormatDisplayLv(int level)
    {
        return "LV. " + level.ToString();
    }

    public static string FormatResourceUnit(int unit)
    {
        return "X " + unit.ToString();
    }

    public static int RandomCard()
    {
        int random = Random.Range(0, CardDataModel.CardObjectMaxUnit);
        return random;
    }

    public static string FormatTimer(int timeLeft, string type)
    {
        System.TimeSpan time = System.TimeSpan.FromMilliseconds(timeLeft);
        string formatMili = string.Format("{0:000}",time.Milliseconds);
        string formatSecond = string.Format("{0:00}", time.Seconds);
        if (type == "mili")
        {
            return formatMili;
        }
        else
        {
            return formatSecond;
        }
        
    }
}
