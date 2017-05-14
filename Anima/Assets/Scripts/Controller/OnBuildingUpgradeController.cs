using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnBuildingUpgradeController : MonoBehaviour {
    public string buildingKeyname;

    public Text buildingLvTxt;

    public List<Sprite> BuildingLvSprite;
    public GameObject BuildingSpiteObj;

    public List<Sprite> ExpBarSpite;
    public GameObject ExpBarSpiteObj;

    private GameObject gameSocketHandlerObj;
    private GameSocketHandler gameSocketHandler;

    [Header("BG setter")]
    public GameObject GameBGDefaultSprite;
    public GameObject GameBGDesolateSprite;

    void Start()
    {
        UpgradeBuilding();
    }

    public void UpgradeBuilding()
    {
        int exp = 0;

        if(buildingKeyname == BuildingKeyname.Farm)
        {
            exp = GameResourceDataModel.BuildingResouces.farmExp;
        }
        else if (buildingKeyname == BuildingKeyname.WoodCutter)
        {
            exp = GameResourceDataModel.BuildingResouces.woodCutterExp;
        }
        else if (buildingKeyname == BuildingKeyname.Mine)
        {
            exp = GameResourceDataModel.BuildingResouces.mineExp;
        }
        else if (buildingKeyname == BuildingKeyname.Town)
        {
            exp = GameResourceDataModel.BuildingResouces.townExp;
        }
        else if (buildingKeyname == BuildingKeyname.Forest)
        {
            exp = GameResourceDataModel.NaturalResources.forestExp;
            UpdateBGSpriteInForestCase(exp);
        }

        UpdateBuildingSprite(exp);
    }

    void UpdateBGSpriteInForestCase(int exp)
    {
        int forestLv = GameFormular.CalculateEXPToLv(exp);
        if (forestLv < 1)
        {
            GameBGDesolateSprite.SetActive(true);
            GameBGDefaultSprite.SetActive(false);
        }
        else
        {
            GameBGDefaultSprite.SetActive(true);
            GameBGDesolateSprite.SetActive(false);
        }
    }

    void UpdateBuildingSprite(int exp)
    {
        int buildingLv = GameFormular.CalculateEXPToLv(exp);
        int buildingExp = GameFormular.RemainEXPGateAfterCalculateLv(exp);

       

        if(buildingKeyname == BuildingKeyname.Forest)
        {
            SetForestSpriteLv(buildingLv);
            SetExpBarSprite(buildingExp);
        }
        else
        {
            SetBuildingSpriteLv(buildingLv);
            SetExpBarSprite(buildingExp);
        }
    }

    void SetForestSpriteLv(int forestLv)
    {
        buildingLvTxt.text = Utilities.FormatDisplayLv(forestLv);
        SpriteRenderer spriteObj = BuildingSpiteObj.GetComponent<SpriteRenderer>();

        if(forestLv < 1)
        {
            spriteObj.sprite = BuildingLvSprite[4];
        }
        else
        {
            spriteObj.sprite = BuildingLvSprite[forestLv - 1];
        }  
    }

    void SetBuildingSpriteLv(int selectedLv)
    {
        buildingLvTxt.text = Utilities.FormatDisplayLv(selectedLv);
        SpriteRenderer spriteObj = BuildingSpiteObj.GetComponent<SpriteRenderer>();
        spriteObj.sprite = BuildingLvSprite[selectedLv - 1];

        //update card model
    }

    void SetExpBarSprite(int ExpLv)
    {
        Image spriteObj = ExpBarSpiteObj.GetComponent<Image>();
        if (ExpLv < 0)
        {
            spriteObj.sprite = ExpBarSpite[0];
        }
        else
        {
            spriteObj.sprite = ExpBarSpite[ExpLv];
        }
       
    }
}

public static class BuildingKeyname
{
    public const string
        Forest = "FOREST",
        WoodCutter = "WOODCUTTER",
        Mine = "MINE",
        Farm = "FARM",
        Town = "TOWN";
}

