﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnPlayerController : MonoBehaviour {
    public GameSocketHandler gameSocketHandler;
    public CardCollectionManager cardCollectionManager;
    public GameCalculatorService calculatorService;
    public OnCreateGameController onCreateGameController;
    public EventSoundDataModel eventSoundModel;

    private bool IsSocketConnected = false;
    public CardDataModel cardModel;

    private bool _isEventDisplayEnd = false;
    private float _timeSecondCounter = 0;

    [Header("Player UI Info")]
    public Text PlayerNameTxt;
    public Text PlayerScoreTxt;
    public Text CurrentRoleTxt;
    public Text TimerMiliTxt;
    public Text TimerSecondsTxt;

    [Header("Player Action Panel")]
    public Button UsedCardBtn;
    public Button EndTurnBtn;

    [Header("Dialog")]
    public GameObject TurnNotifyDialog;
    public GameObject EventAtStartRoundDialog;
    public Text DialogHeader;
    public Text DialogDescription;

    [Header("Sound")]
    public AudioSource endBtnClickSound;
    
    void Start()
    {
        onCreateGameController = this.gameObject.GetComponent<OnCreateGameController>();

        IntialiazSocket();
        gameSocketHandler.GetUpdateJoinGame(OnLoadPlayer);
        SetCardStarter();
    }

    void OnLoadPlayer()
    { 
        if (IsAllPlayerJoiningGame() && IsLastPlayer())
        {
            //เอาแค่คนเดียวยิงไปบอก ว่า ให้ sort 
            print("All Player Ready");
            gameSocketHandler.SendRequestSortedPlayerRole();
        }
    }

    void SetCardStarter()
    {
        for (int unit = 0; unit < Utilities.StartCardUnit; unit++)
        {
            OnDrawCard();
        }
    }

    public void OnDrawCard()
    {
        int random = Utilities.RandomCard();
        CreateCard(random);   
    }

    void CreateCard(int selectedCardNo)
    {
        GameObject cardCollectionDeck = GameObject.FindGameObjectWithTag("CardDeck");

        GameObject card = cardModel.GetCard(selectedCardNo);
        GameObject instantCard = Instantiate(card) as GameObject;
        instantCard.transform.SetParent(cardCollectionDeck.transform, false);
        instantCard.transform.parent = cardCollectionDeck.transform;

        PlayerDataModel.CardsInHandUnit += 1;
        cardCollectionManager.SetupWidthOfRow(PlayerDataModel.CardsInHandUnit);
    }

    public void DeleteCard(GameObject cardSelected)
    {
        Destroy(cardSelected);
        PlayerDataModel.CardsInHandUnit -= 1;
        cardCollectionManager.SetupWidthOfRow(PlayerDataModel.CardsInHandUnit);
    }

    public void UpdateGameTurn()
    {
        //if start player when start new round will wait untill the dialog disable
        
        string currentPlayerNameTurn = PlayerDataModel.gameCurrentTurnData.playerNameInCurrentTurn;
        int turnNo = PlayerDataModel.gameCurrentTurnData.turnNo;

        if(PlayerDataModel.PlayerInGameData.username == currentPlayerNameTurn)
        {
            if (turnNo == 1)
            {
                PlayerDataModel.IsFirstPlayerInNewRound = true;
            }
            else
            {
                PlayerDataModel.IsFirstPlayerInNewRound = false;
            }

            SetPlayerTurn(true, currentPlayerNameTurn);    
        }
        else
        {
            SetPlayerTurn(false, currentPlayerNameTurn);
        }      
    }

    public void UpdateEndTurnGameResource()
    {
        if (!PlayerDataModel.IsFirstTurn)
        {
            string jsonObj = JsonUtility.ToJson(Utilities.GenerateSendingGameResourceDataObj(), true);
            calculatorService.SendReqCalEndTurnResource(jsonObj);
        }      
    }

    void SetPlayerTurn(bool isTurned , string currentPlayerNameTurn)
    {
        if (isTurned)
        {
            CurrentRoleTxt.text = "-";

            if (PlayerDataModel.IsFirstPlayerInNewRound && !PlayerDataModel.IsFirstTurn)
            {
                StartNewRound();
                StartCoroutine("WaitDialog");
            }
            else
            {
                StartCoroutine("WaitBeforeStartTurn");
            }    
        }
        else
        {
            CurrentRoleTxt.text = currentPlayerNameTurn;

            UsedCardBtn.gameObject.SetActive(false);
            EndTurnBtn.gameObject.SetActive(false);
        }
    }

    IEnumerator WaitDialog()
    {
        while (!_isEventDisplayEnd)
        {
            yield return new WaitForSeconds(0.1f);
        }
        
        StartCoroutine("WaitBeforeStartTurn");
        _isEventDisplayEnd = false;
    }

    IEnumerator WaitBeforeStartTurn()
    {
        while (!onCreateGameController.isGameObjectiveDialogVisible)
        {
            yield return new WaitForSeconds(0.5f);
        }

        RunTimer(false);

        TurnNotifyDialog.SetActive(true);
        yield return new WaitForSeconds(2f);

        TurnNotifyDialog.SetActive(false);

        UsedCardBtn.gameObject.SetActive(true);
        EndTurnBtn.gameObject.SetActive(true);
        GetRandomUnitOfDrawingCard();

        RunTimer(true);

        // tell cleint that this is your turn
    }

    void GetRandomUnitOfDrawingCard()
    {
        int unit = Utilities.GetRandomUnitOfCard();

        if (!PlayerDataModel.IsFirstTurn)
        {
            for (int index = 0; index < unit; index++)
            {
                OnDrawCard();
            }
        }       
    }

    void IntialiazSocket()
    {
        while (!IsSocketConnected)
        {
            if (gameSocketHandler.socket.socket.IsConnected && !IsSocketConnected)
            {
                IsSocketConnected = true;
                gameSocketHandler.SendUpdateJoinGame(UpdatePlayerInfoUI);

                gameSocketHandler.GetSortedPlayerRole();
                gameSocketHandler.GetUpdateResoundAfterEndRound(DisplayEventPopupDialog);
                gameSocketHandler.GetGameTurnData(UpdateGameTurn);            
            }
        }
    }

    void UpdatePlayerInfoUI()
    {
        string playerName = PlayerDataModel.PlayerInGameData.username;
        int score = PlayerDataModel.PlayerInGameData.score;

        PlayerNameTxt.text = playerName;
        PlayerScoreTxt.text = score.ToString();
    }

    void RunTimer(bool isRun) {
        if (isRun)
        {
            SetStartTimerTxt();
            StartCoroutine("Timer");
        }
        else
        {
            EndTimer();
        }      
    }

    void SetTimerTxt()
    {
        int maximunSecondPerTurn = Utilities.MaximumSecondsPerTurn;
        TimerSecondsTxt.text = Utilities.FormatTimer((int)(maximunSecondPerTurn - _timeSecondCounter), "seconds");
    }

    IEnumerator Timer()
    {
        int maximunSecondPerTurn = Utilities.MaximumSecondsPerTurn;
        bool isSecondsLimit = (_timeSecondCounter >= maximunSecondPerTurn);

        if (isSecondsLimit)
        {
            EndTimer();
            SetToNextTurn();
        }
        else
        {
            yield return new WaitForSeconds(1f);

            _timeSecondCounter += 1;

            SetTimerTxt();
            StartCoroutine("Timer");
        }
    }

    public void SetToNextTurn()
    {
        endBtnClickSound.Play();
        PlayerDataModel.IsFirstTurn = false;

        RunTimer(false);

        int currentTurnNo = PlayerDataModel.gameCurrentTurnData.turnNo;

        if(currentTurnNo < PlayerDataModel.ClientUnit)
        {
            string nextPlayer = PlayerDataModel.ClientsInGameData[currentTurnNo].username;
            gameSocketHandler.SendReqGameTurnData(currentTurnNo, nextPlayer);
        }
        else
        {
            //cal end turn for last player
            string jsonObj = JsonUtility.ToJson(Utilities.GenerateSendingGameResourceDataObj(), true);
            calculatorService.SendReqCalEndRoundResource(jsonObj);      
        }         
    }

    void StartNewRound()
    {
        string jsonObj = JsonUtility.ToJson(Utilities.GenerateSendingGameResourceDataObj(), true);

        calculatorService.SendReqCalNewRoundResource(jsonObj);
    }

    void DisplayEventPopupDialog()
    {    
        StartCoroutine("DisplayEventDialog");
    }

    IEnumerator DisplayEventDialog()
    {
        CheckEventSoundUsage(PlayerDataModel.RoundEvent);
        EventAtStartRoundDialog.SetActive(true);
        string EventKeyName = PlayerDataModel.RoundEvent; 
        DialogHeader.text = EventDataModel.EventHeaderList[EventKeyName];
        DialogDescription.text = EventDataModel.EventDescriptionList[EventKeyName];
        yield return new WaitForSeconds(3f);

        DialogDescription.text = EventDataModel.EventDescriptionEffectList[EventKeyName];
        yield return new WaitForSeconds(3f);

        EventAtStartRoundDialog.SetActive(false);
        _isEventDisplayEnd = true;
    } 

    void CheckEventSoundUsage(string eventName)
    {
        AudioSource selectedEventSound;
        selectedEventSound = eventSoundModel.getEventSound(eventName);
        selectedEventSound.Play();
    }

    bool IsAllPlayerJoiningGame()
    {
        if (LoginDataModel.ClientsUnit == PlayerDataModel.ClientUnit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsLastPlayer()
    {
        int clientsUnit = LoginDataModel.ClientsUnit;
        string lastJoiningPlayerName = LoginDataModel.ClientsInfo[clientsUnit - 1].username;

        if(PlayerDataModel.PlayerInGameData.username == lastJoiningPlayerName)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void EndTimer()
    {
        StopCoroutine("Timer");
        ResetTimer();
    }

    void ResetTimer()
    {
        _timeSecondCounter = 0;
        TimerSecondsTxt.text = "00";
    }

    void SetStartTimerTxt()
    {
        TimerSecondsTxt.text = "30";
    }
}
