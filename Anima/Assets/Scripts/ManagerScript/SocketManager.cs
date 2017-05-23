using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SocketManager : MonoBehaviour {
    private SocketIOComponent _socket;
    public GameObject DisconnectPanel;

    public GameObject loginPanel;
    public GameObject lobbyPanel;


    void Awake()
    {
        DisconnectPanel.SetActive(false);
        _socket = this.gameObject.GetComponent<SocketIOComponent>();
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        _socket.On("disconnect", OnServerDown);
    }

    void OnServerDown(SocketIOEvent evt)
    { 
        DisconnectPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.name != "MainMenu")
        {
            SceneManager.LoadScene("MainMenu");

            if (GameObject.Find("SocketIO") != null)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            loginPanel.SetActive(true);
            lobbyPanel.SetActive(false);
        }
        DisconnectPanel.SetActive(false);
    }
}
