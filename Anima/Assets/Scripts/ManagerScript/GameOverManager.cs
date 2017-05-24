using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {
   
    public Text ObjectiveResultTxt;
    public Text MissionResultTxt;

    public List<Sprite> ResultSymbolSpriteModel;

    [Header("BG")]
    public GameObject DesolateBG;
    public GameObject DefaultBG;

    [Header("Sound")]
    public AudioSource bgmSoundMissionConplete;
    public AudioSource bgmMissionFailed;
    public AudioSource bgmMissionComplete;

    [Header("Objective Checker")]
    public Image MainObjectiveChecker;
    public Image SubObjectiveChecker;
    public Image PopObjectiveChecker;

    [Header("Objective Description")]
    public Text MainObjectiveDescrriptionTxt;
    public Text SubObjectiveDescriptionTxt;
    public Text PopulationDescriptionTxt;

    void Start()
    {
        bool isMissionComplete = GameOverModel.IsMissionComplete;

        bgmSoundMissionConplete.Play();
        UpdateMissionResult();

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
      
        MissionResultTxt.text = "Mission Complete";  
    }

    void UpdateMissionResult()
    {
        ObjectiveResultTxt.text = GameOverModel.MissionDescription;

        MainObjectiveDescrriptionTxt.text = GameOverModel.MainMissionDescription;
        MainObjectiveChecker.sprite = GetMissionResultSprie(GameOverModel.IsMainMissionComplete);

        SubObjectiveDescriptionTxt.text = GameOverModel.SubMissionDescription;
        SubObjectiveChecker.sprite = GetMissionResultSprie(GameOverModel.IsSubMissionComplete);

        PopulationDescriptionTxt.text = GameOverModel.PopulationMissionDescription;
        PopObjectiveChecker.sprite = GetMissionResultSprie(GameOverModel.IsPopulationComplete);
    }

    Sprite GetMissionResultSprie(bool isComplete)
    {
        if (isComplete)
        {
            return ResultSymbolSpriteModel[0];
        }
        else
        {
            return ResultSymbolSpriteModel[1];
        }
    }

    void SetUIAsMissionFailed()
    {
        DefaultBG.SetActive(false);
        DesolateBG.SetActive(true);

        bgmMissionFailed.Play();

        MissionResultTxt.text = "Mission Failed"; 
    }
}
