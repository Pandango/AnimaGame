using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnBackToMenuController : MonoBehaviour {

    public void OnBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);

        if(GameObject.Find("SocketIO") != null)
        {
            Destroy(GameObject.Find("SocketIO"));
        }
    }
}
