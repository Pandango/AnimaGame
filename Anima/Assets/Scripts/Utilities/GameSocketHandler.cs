﻿using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSocketHandler : MonoBehaviour {
    public SocketIOComponent socket;

    public delegate void OnUpdatePlayerInfo();
    private OnUpdatePlayerInfo callbackOnUpdatePlayerInfo;

    public delegate void OnLoadPlayer();
    private OnLoadPlayer callbackOnLoadPlayer;

    public delegate void OnUpdateGameResource();
    private OnUpdateGameResource callbackUpdateGameResourcer;

    public delegate void OnUpdateGameTurn();
    private OnUpdateGameTurn callbackOnUpdateGameTurn;

    public delegate void OnGameOver();
    private OnGameOver callbackOnGameOver;

    public delegate void OnUpdateNewRound();
    private OnUpdateNewRound callbackOnUpdateNewRound;

    void Awake()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
    }

    public void SendUpdateJoinGame(OnUpdatePlayerInfo callbackOnUpdatePlayerInfo)
    {
        string playerName = LoginDataModel.PlayerProfile.username;
        int role = LoginDataModel.PlayerRole;

        Dictionary<string, string> sendingNewPlayerData = new Dictionary<string, string>();
        sendingNewPlayerData["username"] = playerName;
        sendingNewPlayerData["role"] = role.ToString();

        socket.Emit("join_game", new JSONObject(sendingNewPlayerData));

        UpdateInGamePlayerData(playerName, role, 0);
        callbackOnUpdatePlayerInfo();
    }

    public void SendRequestCurrentClients()
    {
        socket.Emit("current_ingame_clients");
    }

    public void GetUpdateJoinGame(OnLoadPlayer callbackOnLoadPlayerFunc)
    {
        socket.On("current_ingame_clients", UpdateJoiningClients);
        callbackOnLoadPlayer = callbackOnLoadPlayerFunc;
    }

    void UpdateJoiningClients(SocketIOEvent evt)
    {
        string ingameClientsInfoField = evt.data.GetField("ingame_clients").ToString();
        string serviceData = JsonHelper.FormatJsonArrayItems(ingameClientsInfoField);

        PlayerInGameData[] clientsInGameData = JsonHelper.FromJson<PlayerInGameData>(serviceData);
        PlayerDataModel.ClientsInGameData = clientsInGameData;
        print("currentPlayInGame:" + serviceData);
        callbackOnLoadPlayer();
    }

    void UpdateInGamePlayerData(string username, int playerRow, int score)
    {
        PlayerInGameData playerData = new PlayerInGameData();
        playerData.username = username;
        playerData.role = playerRow;
        playerData.score = score;

        PlayerDataModel.PlayerInGameData = playerData;
    }

    public void SendRequestSortedPlayerRole()
    {
        socket.Emit("sort_player_turn");
    }

    public void GetSortedPlayerRole()
    {
        socket.On("sorted_players", UpdateClientsData);
    }

    void UpdateClientsData(SocketIOEvent evt)
    {
        string ingameClientsInfoField = evt.data.GetField("ingame_clients").ToString();
        string serviceData = JsonHelper.FormatJsonArrayItems(ingameClientsInfoField);

        PlayerInGameData[] clientsInGameData = JsonHelper.FromJson<PlayerInGameData>(serviceData);
        PlayerDataModel.ClientsInGameData = clientsInGameData;
        print("CurrentPlayers Role:" + serviceData);
    }

    public void SendReqGameTurnData(int turnNo, string nextPlayer)
    {
        Dictionary<string, string> sendingGameTurnData = new Dictionary<string, string>();
        sendingGameTurnData["turnNo"] = turnNo.ToString();
        sendingGameTurnData["playerNameInCurrentTurn"] = nextPlayer;

        socket.Emit("update_game_turn", new JSONObject(sendingGameTurnData));
    }

    public void GetGameTurnData(OnUpdateGameTurn callbackOnUpdateGameTurnFunc)
    {
        socket.On("update_game_turn", UpdateGameTurnData);
        //update new resource afgter end turn
        callbackOnUpdateGameTurn = callbackOnUpdateGameTurnFunc;
    }

    void UpdateGameTurnData(SocketIOEvent evt)
    {
        Debug.Log("gameTurn" + evt.data.ToString());
        string gameTurnField = evt.data.GetField("gameTurnData").ToString();
        GameCurrentTurnData gameTurnData = JsonUtility.FromJson<GameCurrentTurnData>(gameTurnField);
        PlayerDataModel.gameCurrentTurnData = gameTurnData;

        callbackOnUpdateGameTurn();
    }

    public void SendReqUpdatedGameResource()
    {
        string sendingGameResource = JsonUtility.ToJson(Utilities.GenerateSendingGameResourceDataObj());
        socket.Emit("update_game_resource", new JSONObject (sendingGameResource));
    }

    public void GetUpdatedGameResource(OnUpdateGameResource callbackUpdateGameResourcerFunc)
    {
        socket.On("update_game_resource", UpdateGameResource);
        callbackUpdateGameResourcer = callbackUpdateGameResourcerFunc;
    }

    void UpdateGameResource(SocketIOEvent evt)
    {
        Debug.Log("Update Game Res" + evt.data.ToString());

        UpdateGameResource(evt.data);
        callbackUpdateGameResourcer();
    }

    #region GameOverSocket
    public void SendReqGameOver()
    {
        Dictionary<string, string> sendingNewPlayerData = new Dictionary<string, string>();
        sendingNewPlayerData["isMissionComplete"] = GameOverModel.IsMainMissionComplete.ToString();
        sendingNewPlayerData["description"] = GameOverModel.MainMissionDescription;

        socket.Emit("game_over", new JSONObject(sendingNewPlayerData));
    }
    

    public void GetGameOver(OnGameOver callbackOnGameOverFunc)
    {
        socket.On("game_over", UpdateGameOver);
        callbackOnGameOver = callbackOnGameOverFunc;
    }

    void UpdateGameOver(SocketIOEvent evt)
    {
        Debug.Log("test" + evt.data);

        GameOverModel.IsMainMissionComplete = bool.Parse(evt.data.GetField("isMissionComplete").str);
        GameOverModel.MainMissionDescription = evt.data.GetField("description").ToString();
        callbackOnGameOver();

    }
    #endregion
    public void SendReqUpdateResoundbeforeNewRound()
    {
        string sendingGameResource = JsonUtility.ToJson(GenerateSendingGameResourceAfterEndRoundObj());
        socket.Emit("update_newround_resource", new JSONObject (sendingGameResource));
    }

    public void GetUpdateResoundAfterEndRound(OnUpdateNewRound callbackOnUpdateDisplayEventInNewRoundFunc)
    {
        socket.On("update_newround_resource", UpdateResourceAfterRoundEvent);

        callbackOnUpdateNewRound = callbackOnUpdateDisplayEventInNewRoundFunc;
    }

    void UpdateResourceAfterRoundEvent(SocketIOEvent evt)
    {
        string updatedResource = evt.data.ToString();

        JSONObject endRoundEvent = evt.data.GetField("gameEvent");
        PlayerDataModel.RoundEvent = endRoundEvent.str;

        Debug.Log("handler" + updatedResource);

        UpdateGameResource(evt.data);

        callbackOnUpdateNewRound();
        callbackUpdateGameResourcer();
    }
    #region Helper
    SendingGameResourceAfterEndRound GenerateSendingGameResourceAfterEndRoundObj()
    {
        SendingGameResourceAfterEndRound sendingGameRes = new SendingGameResourceAfterEndRound();
        sendingGameRes.gameEvent = PlayerDataModel.RoundEvent;
        sendingGameRes.populationFoodBalanced = GameResourceDataModel.PopulationFood;
        sendingGameRes.sharingResource = GameResourceDataModel.SharingResources;
        sendingGameRes.buildingResource = GameResourceDataModel.BuildingResouces;
        sendingGameRes.naturalResource = GameResourceDataModel.NaturalResources;
        return sendingGameRes;
    }

    void UpdateGameResource(JSONObject gameResourceJsonString)
    {
        JSONObject pfJson = gameResourceJsonString.GetField("populationFoodBalanced");
        GameResourceDataModel.PopulationFood = JsonUtility.FromJson<PopulationFoodBalanced>(pfJson.ToString());

        JSONObject resourceJson = gameResourceJsonString.GetField("sharingResource");
        GameResourceDataModel.SharingResources = JsonUtility.FromJson<SharingResource>(resourceJson.ToString());

        JSONObject buildingResourceJson = gameResourceJsonString.GetField("buildingResource");
        GameResourceDataModel.BuildingResouces = JsonUtility.FromJson<BuildingResource>(buildingResourceJson.ToString());

        JSONObject naturalResourceJson = gameResourceJsonString.GetField("naturalResource");
        GameResourceDataModel.NaturalResources = JsonUtility.FromJson<NaturalResource>(naturalResourceJson.ToString());
    }
    #endregion
}

[Serializable]
public class SendingGameResourceAfterEndRound
{
    public string gameEvent;
    public PopulationFoodBalanced populationFoodBalanced = new PopulationFoodBalanced();
    public SharingResource sharingResource = new SharingResource();
    public BuildingResource buildingResource = new BuildingResource();
    public NaturalResource naturalResource = new NaturalResource();
}
