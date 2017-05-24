using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySocketHandler : MonoBehaviour {
    public SocketIOComponent socket;

    public delegate void OnUpdateClientLoginInfo();
    public OnUpdateClientLoginInfo callbackOnUpdateClientLoginInfo;

    public delegate void OnLoadNewGameScene();
    public OnLoadNewGameScene callbackOnLoadNewGameScene;

    public void SendUpdatePlayerState(string playerName, string state)
    {
        OnUpdatePlayerLobbyInfoToModel(state);

        Dictionary<string, string> sendingData = new Dictionary<string, string>();
        sendingData["role"] = LoginDataModel.ClientRole(playerName).ToString();
        sendingData["state"] = state;

        socket.Emit("update_userstate", new JSONObject(sendingData));
    }

    public void SendRequestClientCollection()
    {
        socket.Emit("current_client");
    }

    public void GetClientCollection(OnUpdateClientLoginInfo action)
    {
        socket.On("current_client", OnUpdateClientLoginInfoToModel);
        callbackOnUpdateClientLoginInfo = action;
    }

    void OnUpdateClientLoginInfoToModel(SocketIOEvent evt)
    {
        string clientField = evt.data.GetField("clients").ToString();
        string serviceData = JsonHelper.FormatJsonArrayItems(clientField);

        PlayerInfo[] clientsInfo = JsonHelper.FromJson<PlayerInfo>(serviceData);
        LoginDataModel.ClientsInfo = clientsInfo;

        callbackOnUpdateClientLoginInfo();
    }

    void OnUpdatePlayerLobbyInfoToModel(string state)
    {
        LoginDataModel.PlayerProfile.state = state;
    }

    public void SendCreateGame(string gameObjective, string gameSubObjective, int requirePopulation)
    {
        Dictionary<string, string> gameObjectiveDic = new Dictionary<string, string>();
        gameObjectiveDic["gameObjective"] = gameObjective;
        gameObjectiveDic["gameSubObjective"] = gameSubObjective;
        gameObjectiveDic["populationObjective"] = requirePopulation.ToString();

        socket.Emit("create_game", new JSONObject(gameObjectiveDic));
    }

    public void GetGameObjective()
    {
        socket.On("set_gameObjective", UpdatedGameObjective);
    }

    void UpdatedGameObjective(SocketIOEvent evt)
    {
        string gameObjectJson = evt.data.GetField("gameObjective").str;
        string subObjectiveJson = evt.data.GetField("gameSubObjective").str;
        int populationObjectiveJson = int.Parse(evt.data.GetField("populationObjective").str);

        Debug.Log("gameObjective : " + evt.data.ToString());

        GameObjectiveDataModel.CurrentGameObjective = gameObjectJson;
        GameObjectiveDataModel.CurrentSubGameObjetive = subObjectiveJson;
        GameObjectiveDataModel.CurrentPopulationObjective = populationObjectiveJson;
    }

    public void GetCreateGame(OnLoadNewGameScene onLoadNewGameScene)
    {
        socket.On("create_game", OnCreateGame);
        callbackOnLoadNewGameScene = onLoadNewGameScene;
    }

    void OnCreateGame(SocketIOEvent evt)
    {
        JSONObject pfJson = evt.data.GetField("populationFoodBalanced");
        GameResourceDataModel.PopulationFood = JsonUtility.FromJson<PopulationFoodBalanced>(pfJson.ToString());

        JSONObject resourceJson = evt.data.GetField("sharingResource");
        GameResourceDataModel.SharingResources = JsonUtility.FromJson<SharingResource>(resourceJson.ToString());

        JSONObject buildingResourceJson = evt.data.GetField("buildingResource");
        GameResourceDataModel.BuildingResouces = JsonUtility.FromJson<BuildingResource>(buildingResourceJson.ToString());

        JSONObject naturalResourceJson = evt.data.GetField("naturalResource");
        GameResourceDataModel.NaturalResources = JsonUtility.FromJson<NaturalResource>(naturalResourceJson.ToString());

        callbackOnLoadNewGameScene();
    }
}
