using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameCalculatorService : MonoBehaviour {
    private GameObject gameSocketHandlerObj;
    private GameSocketHandler gameSocketHandler;

    public string hostUrl = "http://10.122.5.93:8080";

    void Start()
    {
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

            JSONObject updatedGameResourceJson = new JSONObject(www.text);

            JSONObject pfJson = updatedGameResourceJson["populationFoodBalanced"];
            GameResourceDataModel.PopulationFood = JsonUtility.FromJson<PopulationFoodBalanced>(pfJson.ToString());

            JSONObject resourceJson = updatedGameResourceJson[ "sharingResource"];
            GameResourceDataModel.SharingResources = JsonUtility.FromJson<SharingResource>(resourceJson.ToString());

            JSONObject buildingResourceJson = updatedGameResourceJson["buildingResource"];
            GameResourceDataModel.BuildingResouces = JsonUtility.FromJson<BuildingResource>(buildingResourceJson.ToString());

            JSONObject naturalResourceJson = updatedGameResourceJson["naturalResource"];
            GameResourceDataModel.NaturalResources = JsonUtility.FromJson<NaturalResource>(naturalResourceJson.ToString());

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

            JSONObject updatedGameResourceJson = new JSONObject(www.text);

            JSONObject pfJson = updatedGameResourceJson["populationFoodBalanced"];
            GameResourceDataModel.PopulationFood = JsonUtility.FromJson<PopulationFoodBalanced>(pfJson.ToString());

            JSONObject resourceJson = updatedGameResourceJson["sharingResource"];
            GameResourceDataModel.SharingResources = JsonUtility.FromJson<SharingResource>(resourceJson.ToString());

            JSONObject buildingResourceJson = updatedGameResourceJson["buildingResource"];
            GameResourceDataModel.BuildingResouces = JsonUtility.FromJson<BuildingResource>(buildingResourceJson.ToString());

            JSONObject naturalResourceJson = updatedGameResourceJson["naturalResource"];
            GameResourceDataModel.NaturalResources = JsonUtility.FromJson<NaturalResource>(naturalResourceJson.ToString());

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


    IEnumerator WaitForRequestEndRound(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            //update game resource to model
            Debug.Log("WWW EndRound" + www.data);

            JSONObject updatedGameResourceInRoundJson = new JSONObject(www.text);

            JSONObject endRoundEvent = updatedGameResourceInRoundJson["gameEvent"];
            PlayerDataModel.RoundEvent = endRoundEvent.str;

            Debug.Log(endRoundEvent.str);

            JSONObject pfJson = updatedGameResourceInRoundJson["populationFoodBalanced"];
            GameResourceDataModel.PopulationFood = JsonUtility.FromJson<PopulationFoodBalanced>(pfJson.ToString());

            JSONObject resourceJson = updatedGameResourceInRoundJson["sharingResource"];
            GameResourceDataModel.SharingResources = JsonUtility.FromJson<SharingResource>(resourceJson.ToString());

            JSONObject buildingResourceJson = updatedGameResourceInRoundJson["buildingResource"];
            GameResourceDataModel.BuildingResouces = JsonUtility.FromJson<BuildingResource>(buildingResourceJson.ToString());

            JSONObject naturalResourceJson = updatedGameResourceInRoundJson["naturalResource"];
            GameResourceDataModel.NaturalResources = JsonUtility.FromJson<NaturalResource>(naturalResourceJson.ToString());

            gameSocketHandler.SendReqRandomEventAfterEndRound();
        }
        else
        {
            Debug.Log("WWW Error:" + www.error.ToString());
        }

    }

    public void SendReqEventRandomAfterEndRound(string JsonObject)
    {
        string url = hostUrl + "/endround";

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        byte[] body = Encoding.UTF8.GetBytes(JsonObject);

        WWW www = new WWW(url, body, headers);

        StartCoroutine(WaitForRequestEndRound(www));
    }
}
