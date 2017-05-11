using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManagar : MonoBehaviour {
    public static int MinimumClient = 1;

    public GameObject LobbyManagerObj;
    public GameObject LoginManagerObj;

	// Use this for initialization
	void Start () {
        LobbyManagerObj.SetActive(false);
        LoginManagerObj.SetActive(true);
	}

}
