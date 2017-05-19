using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnUsedCardController : MonoBehaviour {
    private GameObject _onPlayerControllerObj;
    private OnPlayerController _onPlayerController;

    public GameCalculatorService calculatorService;

    [Header("Sound")]
    public AudioSource BtnClickSound;
    public AudioSource UpdatedResourceSound;

    void Start()
    {
        _onPlayerControllerObj = GameObject.Find("GameManager");
        _onPlayerController = _onPlayerControllerObj.GetComponent<OnPlayerController>();

    }

    public void OnUsedCard()
    {
        
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");

        for (int index = 0; index < cards.Length; index++)
        {
            if (cards[index].GetComponent<OnSelectCardController>().IsSelected)
            {
                bool isUsable = IsResourceEnough(cards[index]) && IsNaturalResourceEnough() && IsBuildingNotLevelMax();
                if (isUsable)
                {
                    BtnClickSound.Play();
                    UpdatedResourceSound.Play();
                    CalResourceAfterUsed();

                    //reset selectedcard;
                    cards[index].GetComponent<OnSelectCardController>().SetDefaultOtherCards();
                    _onPlayerController.DeleteCard(cards[index]);

                }
                else
                {
                    //tell player resource not enough or playsound
                }
            }
        }
    }

    bool IsBuildingNotLevelMax()
    {
        bool isBuildingNotLevelMax = true;
        string usedCard = SelectedCardDataModel.SelectedCardKeyName;
        int buildingLv = 0;

        if (usedCard == "FARM")
        {
            buildingLv = GameFormular.CalculateEXPToLv(GameResourceDataModel.BuildingResouces.farmExp);
        }
        else if (usedCard == "MINE")
        {
            buildingLv = GameFormular.CalculateEXPToLv(GameResourceDataModel.BuildingResouces.mineExp);
        }
        else if (usedCard == "WOODCUTTER")
        {
            buildingLv = GameFormular.CalculateEXPToLv(GameResourceDataModel.BuildingResouces.woodCutterExp);
        }
        else if (usedCard == "TOWN")
        {
            buildingLv = GameFormular.CalculateEXPToLv(GameResourceDataModel.BuildingResouces.townExp);
        }
        else if (usedCard == "TREE")
        {
            buildingLv = GameFormular.CalculateEXPToLv(GameResourceDataModel.NaturalResources.forestExp);
        }

        if(buildingLv >= Utilities.MaximumAllowLevel)
        {
            isBuildingNotLevelMax = false;
        }


        return isBuildingNotLevelMax;
    }

    bool IsResourceEnough(GameObject usedCard)
    {
        bool isResourceEnough = true;
        int woodUsage = usedCard.GetComponent<OnCardController>().woodUsage;
        int stoneUsage = usedCard.GetComponent<OnCardController>().stoneUsage;

        int curretnStoneUnit = GameResourceDataModel.SharingResources.stone;
        int currentWoodUnit = GameResourceDataModel.SharingResources.wood;

        if(woodUsage > currentWoodUnit)
        {
            isResourceEnough = false;
        }

        if(stoneUsage > curretnStoneUnit)
        {
            isResourceEnough = false;
        }

        return isResourceEnough;
    }

    bool IsNaturalResourceEnough()
    {
        bool isNaturalResourceEnough = true;
        string usedCard = SelectedCardDataModel.SelectedCardKeyName;

        if (usedCard == "FARM")
        {
            if (GameResourceDataModel.NaturalResources.waterExp < 1)
                isNaturalResourceEnough = false;
        }
        else if (usedCard == "MINE")
        {
            if (GameResourceDataModel.NaturalResources.waterExp < 1)
                isNaturalResourceEnough = false;
        }
        else if (usedCard == "WOODCUTTER")
        {
            if (GameResourceDataModel.NaturalResources.forestExp < 1)
                isNaturalResourceEnough = false;
        }
        else if (usedCard == "TOWN")
        {
            if (GameResourceDataModel.NaturalResources.forestExp < 1)
                isNaturalResourceEnough = false;
        }
        else if (usedCard == "TREE")
        {
            if (GameResourceDataModel.NaturalResources.waterExp < 1)
                isNaturalResourceEnough = false;
        }
        return isNaturalResourceEnough;
    }

    public void CalResourceAfterUsed()
    {
        string usedCard = SelectedCardDataModel.SelectedCardKeyName;
        string jsonObjectString = null;

        if (usedCard == "FARM")
        {
            jsonObjectString = JsonUtility.ToJson(GenerateSendFarmCardData(), true);
        }
        else if(usedCard == "MINE")
        {
            jsonObjectString = JsonUtility.ToJson(GenerateSendMineCardData(), true);
        }
        else if( usedCard == "WOODCUTTER")
        {
            jsonObjectString = JsonUtility.ToJson(GenerateSendWoodCutterCardData(), true);
        }
        else if (usedCard == "TOWN")
        {
            jsonObjectString = JsonUtility.ToJson(GenerateSendTownCardData(), true);
        }
        else if (usedCard == "TREE")
        {
            jsonObjectString = JsonUtility.ToJson(GenerateSendTreeCardData(), true);
        }
        else if (usedCard == "RAIN")
        {
            jsonObjectString = JsonUtility.ToJson(GenerateSendRainCardData(), true);
        }

        calculatorService.sendReqUsedCard(jsonObjectString);
        

        Debug.Log(jsonObjectString);
        
    }

    SendUsagedGameData GenerateSendFarmCardData()
    {
        SendUsagedGameData sendUsageGameData = new SendUsagedGameData();
        sendUsageGameData.getBuildingExp.farmGetExp = 1;
        sendUsageGameData.getNaturalExp.waterGetExp = -1;
        sendUsageGameData.getResource.stoneUnit = CardDataModel.FarmCardModel.stoneUsage * -1;
        sendUsageGameData.getResource.woodUnit = CardDataModel.FarmCardModel.woodUsage * -1;
        return sendUsageGameData;
    }

    SendUsagedGameData GenerateSendMineCardData()
    {
        SendUsagedGameData sendUsageGameData = new SendUsagedGameData();
        sendUsageGameData.getBuildingExp.mineGetExp = 1;
        sendUsageGameData.getNaturalExp.waterGetExp = -1;
        sendUsageGameData.getResource.woodUnit = CardDataModel.MineCardModel.woodUsage * -1;
        return sendUsageGameData;
    }

    SendUsagedGameData GenerateSendWoodCutterCardData()
    {
        SendUsagedGameData sendUsageGameData = new SendUsagedGameData();
        sendUsageGameData.getBuildingExp.woodCutterGetExp = 1;
        sendUsageGameData.getNaturalExp.forestGetExp = -1;
        sendUsageGameData.getResource.stoneUnit = CardDataModel.WoodCutterCardModel.stoneUsage * -1;
        return sendUsageGameData;
    }

    SendUsagedGameData GenerateSendTownCardData()
    {
        SendUsagedGameData sendUsageGameData = new SendUsagedGameData();
        sendUsageGameData.getBuildingExp.townGetExp = 1;
        sendUsageGameData.getNaturalExp.forestGetExp = -1;
        sendUsageGameData.getResource.woodUnit = CardDataModel.TownCardModel.woodUsage * -1;
        sendUsageGameData.getResource.stoneUnit = CardDataModel.TownCardModel.stoneUsage * -1;
        return sendUsageGameData;
    }

    SendUsagedGameData GenerateSendRainCardData()
    {
        SendUsagedGameData sendUsageGameData = new SendUsagedGameData();
        sendUsageGameData.getNaturalExp.waterGetExp = 2;
        return sendUsageGameData;
    }

    SendUsagedGameData GenerateSendTreeCardData()
    {
        SendUsagedGameData sendUsageGameData = new SendUsagedGameData();
        sendUsageGameData.getNaturalExp.waterGetExp = -1;
        sendUsageGameData.getNaturalExp.forestGetExp = 1;
        return sendUsageGameData;
    }
}


