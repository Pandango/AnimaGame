using SocketIO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class OnLoginController : MonoBehaviour {
    public LoginSocketHandler loginSocketHandler;
    public InputField playerNameField;
    public Button startBtn;

    public AudioSource LoginableSound;

    void Start()
    {
        loginSocketHandler = loginSocketHandler.GetComponent<LoginSocketHandler>();
        startBtn.onClick.AddListener(OnLogin);
    }

    public void OnLogin()
    {
        bool isNameApprove= ValidatePlayerName(playerNameField.text);

        if(isNameApprove)
        {
            LoginableSound.Play();

            Dictionary<string, string> playerData = new Dictionary<string, string>();
            playerData["username"] = playerNameField.text;
            playerData["state"] = UserState.Waiting;

            loginSocketHandler.SendLoginCollection(new JSONObject(playerData));
            loginSocketHandler.OnUserConnected();
        }
    }

    public bool ValidatePlayerName(string name)
    {
        if(Regex.IsMatch(name, @"^[\s\w]+$"))
        {
            return true;
        }
        return false;
    }
}
