using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSocketHandler : MonoBehaviour {
    public SocketIOComponent socket;

    void Awake()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
    }

    public void SendReqDisconnect()
    {
        socket.Emit("disconnect");
    }
}
