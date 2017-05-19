using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManagar : MonoBehaviour {
    public static int MinimumClient = 1;

    public GameObject LobbyManagerObj;
    public GameObject LoginManagerObj;

    public AudioSource bgmSound;

	// Use this for initialization
	void Start () {
        bgmSound.Play();

        LobbyManagerObj.SetActive(false);
        LoginManagerObj.SetActive(true);
	}

}
