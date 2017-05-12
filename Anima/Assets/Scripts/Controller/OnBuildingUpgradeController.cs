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
        }

        UpdateBuildingSprite(exp);
    }

    void UpdateBuildingSprite(int exp)
    {
        int buildingLv = GameFormular.CalculateEXPToLv(exp);
        int buildingExp = GameFormular.RemainEXPGateAfterCalculateLv(exp);

        SetBuildingSpriteLv(buildingLv);
        SetExpBarSprite(buildingExp);
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
        spriteObj.sprite = ExpBarSpite[ExpLv];
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

