using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnBackToMenuController : MonoBehaviour {
    public GameOverSocketHandler gameOverSocketHandler;

    public void OnBackToMainMenu()
    {
        gameOverSocketHandler.SendReqDisconnect();

        if (GameObject.Find("SocketIO") != null)
        {
            Destroy(GameObject.Find("SocketIO"));
        }

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
