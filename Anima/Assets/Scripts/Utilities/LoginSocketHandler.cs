using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSocketHandler : MonoBehaviour {
    public SocketIOComponent socket;
    public GameObject RegisterPanel;
    public GameObject LobbyPanel;

    public void SendLoginCollection(JSONObject playerDataCollection)
    {
        socket.Emit("login", playerDataCollection);
    }

    public void OnUserConnected()
    {
        socket.On("user_connected", OnUserConnected);
        socket.On("user_unconnected", OnUserUnConnected);
    }

    private void OnUserUnConnected(SocketIOEvent evt)
    {
        Debug.Log("Get the message from server is: " + evt.data + "");
    }

    private void OnUserConnected(SocketIOEvent evt)
    {
        Debug.Log("Get the message from server is: You '" + evt.data + "' are in lobby");

        string getUserProfileFromServer = evt.data.ToString();

    
        PlayerDataModel.PlayerProfile = JsonUtility.FromJson<PlayerInfo>(getUserProfileFromServer);

        ShowHideRegisterPanel(false);
        ShowHideLobbyPanel(true);
    }

    void ShowHideRegisterPanel(bool isShow)
    {
        RegisterPanel.SetActive(isShow);
    }

    void ShowHideLobbyPanel(bool isShow)
    {
        LobbyPanel.SetActive(isShow);
    }
}
