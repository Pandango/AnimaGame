using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {
   
    public Text ObjectiveResultTxt;
    public Text MissionResultTxt;

    public Text DescriptionTxt;
    public Image ResultSymbol;

    public List<Sprite> ResultSymbolSpriteModel;

    [Header("BG")]
    public GameObject DesolateBG;
    public GameObject DefaultBG;

    public AudioSource bgmSoundMissionConplete;
    public AudioSource bgmMissionFailed;
    public AudioSource bgmMissionComplete;

    void Start()
    {
        bool isMissionComplete = GameOverModel.isMissionComplete;

        bgmSoundMissionConplete.Play();
        if (isMissionComplete)
        {
            SetUIAsMissionComplete();
        }
        else
        {
            SetUIAsMissionFailed();
        }
        
    }

    void SetUIAsMissionComplete()
    {
        DefaultBG.SetActive(true);
        DesolateBG.SetActive(false);

        bgmMissionComplete.Play();

        DescriptionTxt.text = GameOverModel.description;
        ResultSymbol.sprite = ResultSymbolSpriteModel[0];
        ObjectiveResultTxt.text = "Objective Complete.";
        MissionResultTxt.text = "Mission Complete";
        
    }

    void SetUIAsMissionFailed()
    {
        DefaultBG.SetActive(false);
        DesolateBG.SetActive(true);

        bgmMissionFailed.Play();

        DescriptionTxt.text = GameOverModel.description;
        ResultSymbol.sprite = ResultSymbolSpriteModel[1];
        ObjectiveResultTxt.text = "Objective Failed.";
        MissionResultTxt.text = "Mission Failed";
        
    }
}
