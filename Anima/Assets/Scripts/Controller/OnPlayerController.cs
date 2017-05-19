using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnPlayerController : MonoBehaviour {
    public GameSocketHandler gameSocketHandler;
    public CardCollectionManager cardCollectionManager;
    public GameCalculatorService calculatorService;

    private bool IsSocketConnected = false;
    public CardDataModel cardModel;

    private bool isEnableTosetNextTurn= false;
    private bool isRunTimer = false;
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
        IntialiazSocket();
        gameSocketHandler.GetUpdateJoinGame(OnLoadPlayer);
        SetCardStarter();
    }

    void Update()
    {
        if (isRunTimer)
        {
            SetRunTimer();
        } 
    }

    void OnLoadPlayer()
    { 
        if (IsAllPlayerJoiningGame())
        {
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
            ResetTimer();
            CurrentRoleTxt.text = "-";

            if (PlayerDataModel.IsFirstPlayerInNewRound && !PlayerDataModel.IsFirstTurn)
            {
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
            isRunTimer = false;
        }
    }

    IEnumerator WaitDialog()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine("WaitBeforeStartTurn");
    }

    IEnumerator WaitBeforeStartTurn()
    {
        TurnNotifyDialog.SetActive(true);
        yield return new WaitForSeconds(2f);

        TurnNotifyDialog.SetActive(false);
        isRunTimer = true;
        UsedCardBtn.gameObject.SetActive(true);
        EndTurnBtn.gameObject.SetActive(true);
        GetRandomUnitOfDrawingCard();
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
                gameSocketHandler.GetRandomEventAfterEndRound(DisplayEventAfterEventTurn);
                gameSocketHandler.GetGameTurnData(UpdateGameTurn, UpdateEndTurnGameResource);            
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

    void SetRunTimer()
    {
        int maximunSecondPerTurn = Utilities.MaximumMiliSecondsPerTurn;
        _timeSecondCounter += Time.deltaTime * 1000;

        if (_timeSecondCounter <= Utilities.MaximumMiliSecondsPerTurn && _timeSecondCounter >= 0)
        {          
            TimerMiliTxt.text = Utilities.FormatTimer((int)(maximunSecondPerTurn - _timeSecondCounter), "mili");
            TimerSecondsTxt.text = Utilities.FormatTimer((int)(maximunSecondPerTurn - _timeSecondCounter), "seconds");
            isEnableTosetNextTurn = true;
        }
        else
        {
            SetDefaultTimerTxt();

            if (isEnableTosetNextTurn)
            {
                SetToNextTurn();
                isEnableTosetNextTurn = false;
            }
               
            //send to server to set next turn
        } 
    }

    public void SetToNextTurn()
    {
        endBtnClickSound.Play();
        PlayerDataModel.IsFirstTurn = false;

        int currentTurnNo = PlayerDataModel.gameCurrentTurnData.turnNo;

        if(currentTurnNo < PlayerDataModel.ClientUnit)
        {
            SetDefaultTimerTxt();

            string nextPlayer = PlayerDataModel.ClientsInGameData[currentTurnNo].username;
            gameSocketHandler.SendReqGameTurnData(currentTurnNo, nextPlayer);
        }
        else
        {
            StartNewRound();       
        }         
    }

    void StartNewRound()
    {
        string jsonObj = JsonUtility.ToJson(Utilities.GenerateSendingGameResourceDataObj(), true);
        calculatorService.SendReqEventRandomAfterEndRound(jsonObj);
        gameSocketHandler.SendRequestSortedPlayerRole();   
    }

    void DisplayEventPopupDialog()
    {
        StartCoroutine("DisplayEventDialog");
    }

    IEnumerator DisplayEventDialog()
    {
        EventAtStartRoundDialog.SetActive(true);
        string EventKeyName = PlayerDataModel.RoundEvent; 
        DialogHeader.text = EventDataModel.EventHeaderList[EventKeyName];
        DialogDescription.text = EventDataModel.EventDescriptionList[EventKeyName];
        yield return new WaitForSeconds(2);

        DialogDescription.text = EventDataModel.EventDescriptionEffectList[EventKeyName];
        yield return new WaitForSeconds(1);

        EventAtStartRoundDialog.SetActive(false);
    } 

    void DisplayEventAfterEventTurn()
    {
        DisplayEventPopupDialog();
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

    void ResetTimer()
    {
        SetDefaultTimerTxt();
        _timeSecondCounter = 0;
        isRunTimer = false;
    }

    void SetDefaultTimerTxt()
    {
        TimerMiliTxt.text = "000";
        TimerSecondsTxt.text = "00";
    }
}
