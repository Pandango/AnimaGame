using SocketIO;
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

    public delegate void OnUpdateEndGameTurnResource();
    private OnUpdateEndGameTurnResource callbackOnUpdateEndGameTurnResource;

    public void SendUpdateJoinGame(OnUpdatePlayerInfo callbackOnUpdatePlayerInfo)
    {
        //string playerName = "Panda mai dai norn";
        //int role = 0;
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
        
        //check this turn is player's turn
    }

    void UpdateClientsData(SocketIOEvent evt)
    {
        string ingameClientsInfoField = evt.data.GetField("ingame_clients").ToString();
        string serviceData = JsonHelper.FormatJsonArrayItems(ingameClientsInfoField);

        PlayerInGameData[] clientsInGameData = JsonHelper.FromJson<PlayerInGameData>(serviceData);
        PlayerDataModel.ClientsInGameData = clientsInGameData;
        print("CurrentPlayers Role:" + serviceData);

        //ending loading about new turn
    }

    public void SendReqGameTurnData(int turnNo, string nextPlayer)
    {
        Dictionary<string, string> sendingGameTurnData = new Dictionary<string, string>();
        sendingGameTurnData["turnNo"] = turnNo.ToString();
        sendingGameTurnData["playerNameInCurrentTurn"] = nextPlayer;

        socket.Emit("update_game_turn", new JSONObject(sendingGameTurnData));
    }

    public void GetGameTurnData(OnUpdateGameTurn callbackOnUpdateGameTurnFunc, OnUpdateEndGameTurnResource callbackOnUpdateEndGameTurnResourceFunc)
    {
        socket.On("update_game_turn", UpdateGameTurnData);
        //update new resource afgter end turn
        callbackOnUpdateEndGameTurnResource = callbackOnUpdateEndGameTurnResourceFunc;
        callbackOnUpdateGameTurn = callbackOnUpdateGameTurnFunc;
    }

    void UpdateGameTurnData(SocketIOEvent evt)
    {
        Debug.Log("gameTurn" + evt.data.ToString());
        string gameTurnField = evt.data.GetField("gameTurnData").ToString();
        GameCurrentTurnData gameTurnData = JsonUtility.FromJson<GameCurrentTurnData>(gameTurnField);
        PlayerDataModel.gameCurrentTurnData = gameTurnData;

        callbackOnUpdateEndGameTurnResource();
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
        Debug.Log(evt.data.ToString());

        JSONObject pfJson = evt.data.GetField("populationFoodBalanced");
        GameResourceDataModel.PopulationFood = JsonUtility.FromJson<PopulationFoodBalanced>(pfJson.ToString());

        JSONObject resourceJson = evt.data.GetField("sharingResource");
        GameResourceDataModel.SharingResources = JsonUtility.FromJson<SharingResource>(resourceJson.ToString());

        JSONObject buildingResourceJson = evt.data.GetField("buildingResource");
        GameResourceDataModel.BuildingResouces = JsonUtility.FromJson<BuildingResource>(buildingResourceJson.ToString());

        JSONObject naturalResourceJson = evt.data.GetField("naturalResource");
        GameResourceDataModel.NaturalResources = JsonUtility.FromJson<NaturalResource>(naturalResourceJson.ToString());

        callbackUpdateGameResourcer();
    }

    //SendingGameResource GenerateSendingGameResourceDataObj()
    //{
    //    SendingGameResource sendingGameRes = new SendingGameResource();
    //    sendingGameRes.populationFoodBalanced = GameResourceDataModel.PopulationFood;
    //    sendingGameRes.sharingResource = GameResourceDataModel.SharingResources;
    //    sendingGameRes.buildingResource = GameResourceDataModel.BuildingResouces;
    //    sendingGameRes.naturalResource = GameResourceDataModel.NaturalResources;
    //    return sendingGameRes;
    //}
}

//[Serializable]
//public class SendingGameResource
//{
//    public PopulationFoodBalanced populationFoodBalanced = new PopulationFoodBalanced();
//    public SharingResource sharingResource = new SharingResource();
//    public BuildingResource buildingResource = new BuildingResource();
//    public NaturalResource naturalResource = new NaturalResource();
//} 
