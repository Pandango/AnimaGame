using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginDataModel : MonoBehaviour {
    public static PlayerInfo PlayerProfile { get; set; }

    public static PlayerInfo[] ClientsInfo;

    public static int ClientsUnit
    {
        get { return ClientsInfo.Length; }
    }


    private static bool isAllClientsReady;
    public static bool IsAllClientsReady
    {
        get
        {
            isAllClientsReady = true;
            for (int index = 0; index < ClientsInfo.Length; index++)
            {        
                if (ClientsInfo[index].state == UserState.Waiting)
                {
                    isAllClientsReady = false;
                    break;
                }
            }
            return isAllClientsReady;
        }
    }

    private static bool isClientEnough;
    public static bool IsClientEnough
    {
        get
        {       
            if (ClientsInfo.Length >= MainMenuManagar.MinimumClient)
            {
                return true;
            }
            else
            {
                return false;
            }         
        }
    }

    private static int playerRole;
    public static int PlayerRole
    {
        get
        {
            for (int index = 0; index < ClientsInfo.Length; index++)
            {
                if (ClientsInfo[index].username == PlayerProfile.username)
                {
                    playerRole = index;
                    break;
                }
            }
            return playerRole;
        }
    }

    public static int ClientRole(string playerName)
    {
        int clientRole = 0;
        for (int index = 0; index < ClientsInfo.Length; index++)
        {
            if (ClientsInfo[index].username == playerName)
            {
                playerRole = index;
                break;
            }
        }
        return playerRole;
    }
}

[Serializable]
public class PlayerInfo
{
    public string username;
    public string state;
}

public static class UserState
{
    public const string
        Waiting = "WAITING",
        Ready = "READY";
}
