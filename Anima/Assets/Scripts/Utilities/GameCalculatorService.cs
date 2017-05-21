using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameCalculatorService : MonoBehaviour {
    private GameObject gameSocketHandlerObj;
    private GameSocketHandler gameSocketHandler;

    public bool isLocalHost;
    public string hostUrl = "https://blooming-peak-86038.herokuapp.com";

    void Start()
    {
        if (isLocalHost)
        {
            hostUrl = "http://127.0.0.1:8080";
        }
        gameSocketHandlerObj = GameObject.Find("GameManager");
        gameSocketHandler = gameSocketHandlerObj.GetComponent<GameSocketHandler>();
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            //update game resource to model
            Debug.Log("WWW " + www.data);

            UpdateGameResource(www.text);
            gameSocketHandler.SendReqUpdatedGameResource();
        }
        else
        {
            Debug.Log("WWW Error:" + www.error.ToString());
        }
    }

    public void sendReqUsedCard(string JsonObject)
    {
        string url = hostUrl + "/calculate";

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        byte[] body = Encoding.UTF8.GetBytes(JsonObject);

        WWW www = new WWW(url, body, headers);

        StartCoroutine(WaitForRequest(www));

    }

    IEnumerator WaitForRequestEndTurn(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            //update game resource to model
            Debug.Log("WWW " + www.data);

            UpdateGameResource(www.text);
            gameSocketHandler.SendReqUpdatedGameResource();
        }
        else
        {
            Debug.Log("WWW Error:" + www.error.ToString());
        }

    }

    public void SendReqCalEndTurnResource(string JsonObject)
    {
        string url = hostUrl + "/endturn";

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        byte[] body = Encoding.UTF8.GetBytes(JsonObject);

        WWW www = new WWW(url, body, headers);

        StartCoroutine(WaitForRequestEndTurn(www));
    }


    IEnumerator WaitForRequestNewRound(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            //update game resource to model
            Debug.Log("WWW NewRound" + www.data);

            JSONObject updatedGameResourceInRoundJson = new JSONObject(www.text);

            JSONObject endRoundEvent = updatedGameResourceInRoundJson["gameEvent"];
            PlayerDataModel.RoundEvent = endRoundEvent.str;

            Debug.Log(endRoundEvent.str);

            UpdateGameResource(www.text);

            gameSocketHandler.SendReqUpdateResoundbeforeNewRound();
        }
        else
        {
            Debug.Log("WWW Error:" + www.error.ToString());
        }

    }

    public void SendReqCalNewRoundResource(string JsonObject)
    {
        string url = hostUrl + "/endround";

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        byte[] body = Encoding.UTF8.GetBytes(JsonObject);

        WWW www = new WWW(url, body, headers);

        StartCoroutine(WaitForRequestNewRound(www));
    }

    IEnumerator WaitForRequestEndRound(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            //update game resource to model
            Debug.Log("WWW " + www.data);

            UpdateGameResource(www.text);

            gameSocketHandler.SendReqUpdatedGameResource();
            gameSocketHandler.SendRequestSortedPlayerRole();
        }
        else
        {
            Debug.Log("WWW Error:" + www.error.ToString());
        }
    }

    public void SendReqCalEndRoundResource(string JsonObject)
    {
        string url = hostUrl + "/endturn";

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        byte[] body = Encoding.UTF8.GetBytes(JsonObject);

        WWW www = new WWW(url, body, headers);

        StartCoroutine(WaitForRequestEndRound(www));
    }

    void UpdateGameResource(string JsonString)
    {
        JSONObject updatedGameResourceJson = new JSONObject(JsonString);

        JSONObject pfJson = updatedGameResourceJson["populationFoodBalanced"];
        GameResourceDataModel.PopulationFood = JsonUtility.FromJson<PopulationFoodBalanced>(pfJson.ToString());

        JSONObject resourceJson = updatedGameResourceJson["sharingResource"];
        GameResourceDataModel.SharingResources = JsonUtility.FromJson<SharingResource>(resourceJson.ToString());

        JSONObject buildingResourceJson = updatedGameResourceJson["buildingResource"];
        GameResourceDataModel.BuildingResouces = JsonUtility.FromJson<BuildingResource>(buildingResourceJson.ToString());

        JSONObject naturalResourceJson = updatedGameResourceJson["naturalResource"];
        GameResourceDataModel.NaturalResources = JsonUtility.FromJson<NaturalResource>(naturalResourceJson.ToString());

    }
}
