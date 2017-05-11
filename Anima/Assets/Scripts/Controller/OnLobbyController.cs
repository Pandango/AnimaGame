using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnLobbyController : MonoBehaviour {
    public LobbySocketHandler lobbySocketHandler;
    public Text[] ClientsNameTxt;
    public Text[] ClientsStateTxt;

    [Header("Button Setter")]
    public Button ReadyBtn;
    public Button StartGameBtn;

    void Start()
    {
        SetDefaultLobbyValue();

        StartCoroutine("UpdateLobby");
        lobbySocketHandler.GetClientCollection(OnUpdateClientLoginInfo);

        InitializeSocketHandler();
    }

    void InitializeSocketHandler()
    {
        lobbySocketHandler.GetCreateGame(OnLoadNewGame);
    }

    IEnumerator UpdateLobby()
    {
        lobbySocketHandler.SendRequestClientCollection();
        yield return new WaitForSeconds(1f);
        
    }

    public void OnUpdateClientLoginInfo()
    {
        CheckPlayerIsLobbyHeader();
        UpdateClientsInfoInInLobby();     
    }

    public void OnStartGame()
    {
        lobbySocketHandler.SendCreateGame();  
    }

    void OnLoadNewGame()
    {
        SceneManager.LoadScene("PlayingGame", LoadSceneMode.Single);
    }

    public void OnReady()
    {
        string currentState = LoginDataModel.PlayerProfile.state;
        string playerName = LoginDataModel.PlayerProfile.username;
        string state;

        if (currentState == UserState.Waiting)
        {           
            state = UserState.Ready;
            ReadyBtn.gameObject.GetComponentInChildren<Text>().text = "Unready";
        }
        else
        {
            state = UserState.Waiting;
            ReadyBtn.gameObject.GetComponentInChildren<Text>().text = "ready";
        }
        lobbySocketHandler.SendUpdatePlayerState(playerName, state);
    }

    void UpdateClientsInfoInInLobby()
    {
        SetDefaultClientsDisplayName();
        //update name
        PlayerInfo[] clientInfo = LoginDataModel.ClientsInfo;
        for (int clientsNo = 0; clientsNo < clientInfo.Length; clientsNo++)
        {
            ClientsNameTxt[clientsNo].text= clientInfo[clientsNo].username;
        }

        //update state
        for (int clientsNo = 0; clientsNo < clientInfo.Length; clientsNo++)
        {
            ClientsStateTxt[clientsNo].text = clientInfo[clientsNo].state;
        }
    }

    void CheckPlayerIsLobbyHeader()
    {     
        bool isLobbyHeader = (LoginDataModel.PlayerRole == 0);
        if (isLobbyHeader)
        {
            StartGameBtn.gameObject.SetActive(true);
            UpdateStartGameButton();
        }
        else
        {
            StartGameBtn.gameObject.SetActive(false);
        }
    }

    void UpdateStartGameButton()
    {
        if (LoginDataModel.IsAllClientsReady && LoginDataModel.IsClientEnough)
        {
            StartGameBtn.interactable = true;
        }
        else
        {
            StartGameBtn.interactable = false;
        }      
    }

    void SetDefaultLobbyValue()
    {
        for (int index = 0; index < ClientsStateTxt.Length; index++)
        {
            ClientsStateTxt[index].text = string.Empty;
        }
        ReadyBtn.gameObject.GetComponentInChildren<Text>().text = "ready";
    }

    void SetDefaultClientsDisplayName()
    {
        for (int role = 0; role < ClientsNameTxt.Length; role++)
        {
            ClientsNameTxt[role].text = UserState.Waiting.ToLower() + "...";
            ClientsStateTxt[role].text = string.Empty;
        }
    }
}
