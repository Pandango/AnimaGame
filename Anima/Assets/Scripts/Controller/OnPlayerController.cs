using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnPlayerController : MonoBehaviour {
    public GameSocketHandler gameSocketHandler;
    public CardCollectionManager cardCollectionManager;

    private bool IsSocketConnected = false;
    public CardDataModel cardModel;

    private float _timeSecondCounter = 0;


    [Header("Player UI Info")]
    public Text PlayerNameTxt;
    public Text PlayerScoreTxt;
    public Text CurrentRoleTxt;
    public Text TimerMiliTxt;
    public Text TimerSecondsTxt;

    void Start()
    {  
        IntialiazSocket();
        gameSocketHandler.GetUpdateJoinGame(OnLoadPlayer);
        SetCardStarter();
    }

    void Update()
    {
        SetRunTimer();
    }

    void OnLoadPlayer()
    {
        StartCoroutine("LoadPlayer");
    }

    IEnumerator LoadPlayer()
    {
        yield return new WaitForSeconds(1f);
        if (IsAllPlayerJoiningGame())
        {
            print("All Player Ready");
            gameSocketHandler.SendRequestSortedPlayerRole();
        }
        else
        {
            StartCoroutine("LoadPlayer");
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

    void IntialiazSocket()
    {
        while (!IsSocketConnected)
        {
            if (gameSocketHandler.socket.socket.IsConnected && !IsSocketConnected)
            {
                IsSocketConnected = true;
                gameSocketHandler.SendUpdateJoinGame(UpdatePlayerInfoUI);

                gameSocketHandler.GetSortedPlayerRole();
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

        if (_timeSecondCounter <= 8000 && _timeSecondCounter >= 0)
        {          
            TimerMiliTxt.text = Utilities.FormatTimer((int)(maximunSecondPerTurn - _timeSecondCounter), "mili");
            TimerSecondsTxt.text = Utilities.FormatTimer((int)(maximunSecondPerTurn - _timeSecondCounter), "seconds");
        }
        else
        {
            TimerMiliTxt.text = "000";
            TimerSecondsTxt.text = "00";
        } 
    }

    //IEnumerator StartCounter()
    //{
    //    yield return new WaitForSeconds(1f);
    //    if (_timeSecondCounter <= 8 && _timeSecondCounter >= 0)
    //    {
    //        _timeSecondCounter++;

    //        StartCoroutine("StartCounter");
    //    }
    //}
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
        _timeSecondCounter = 0;
    }
}
