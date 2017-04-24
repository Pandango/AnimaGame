using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySocketHandler : MonoBehaviour {
    public SocketIOComponent socket;

    public delegate void OnUpdateClientLoginInfo();
    public OnUpdateClientLoginInfo callbackOnUpdateClientLoginInfo;

    public void SendUpdatePlayerState(string playerName, string state)
    {
        OnUpdatePlayerLobbyInfoToModel(state);

        Dictionary<string, string> sendingData = new Dictionary<string, string>();
        sendingData["role"] = PlayerDataModel.ClientRole(playerName).ToString();
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
        PlayerDataModel.ClientsInfo = clientsInfo;

        callbackOnUpdateClientLoginInfo();
    }

    void OnUpdatePlayerLobbyInfoToModel(string state)
    {
        PlayerDataModel.PlayerProfile.state = state;
    }


}
