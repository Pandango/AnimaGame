using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnCreateGameController : MonoBehaviour {
    public GameSocketHandler gameSocketHandler;

    [Header("Sharing Resource UI")]
    public Text WoodUnitTxt;
    public Text StoneUnitTxt;

    [Header("BuildingGameObject")]
    public List<GameObject> buildingGameObj;
    public List<GameObject> naturalGameObj;

    [Header("Natural Building Lv")]
    public Text ForestLvTxt;

    private OnBuildingUpgradeController _woodCutterController;
    private OnBuildingUpgradeController _mineController;
    private OnBuildingUpgradeController _farmController;
    private OnBuildingUpgradeController _townController;

    private OnBuildingUpgradeController _forestController;

    void Start () {
        //MockUpResource();
        _woodCutterController = buildingGameObj[0].GetComponent<OnBuildingUpgradeController>();
        _mineController = buildingGameObj[1].GetComponent<OnBuildingUpgradeController>();
        _farmController = buildingGameObj[2].GetComponent<OnBuildingUpgradeController>();
        _townController = buildingGameObj[3].GetComponent<OnBuildingUpgradeController>();

        _forestController = naturalGameObj[0].GetComponent<OnBuildingUpgradeController>();

        UpdateGameResource();

        IntializeSocket();
    }

    void IntializeSocket()
    {
        gameSocketHandler.GetUpdatedGameResource(UpdateGameResource);
    }

    void MockUpResource()
    {
        SharingResource sharingRes = new SharingResource();
        sharingRes.stone = 1234;
        sharingRes.wood = 4321;
        GameResourceDataModel.SharingResources = sharingRes;

        BuildingResource buildingRes = new BuildingResource();
        buildingRes.woodCutterExp = 0;
        buildingRes.mineExp = 0;
        buildingRes.farmExp = 0;
        buildingRes.townExp = 0;
        GameResourceDataModel.BuildingResouces = buildingRes;

        NaturalResource naturalRes = new NaturalResource();
        naturalRes.forestExp = 0;
        naturalRes.waterExp = 0;
        GameResourceDataModel.NaturalResources = naturalRes;
    }

    public void UpdateGameResource()
    {
        UpdateSharingResource();
        UpdateBuildingLv();
        UpdateNaturalResource();
    }

    void UpdateSharingResource()
    {
        int stoneUnit = GameResourceDataModel.SharingResources.stone;
        int woodUnit = GameResourceDataModel.SharingResources.wood;

        WoodUnitTxt.text = Utilities.FormatResourceUnit(woodUnit);
        StoneUnitTxt.text = Utilities.FormatResourceUnit(stoneUnit);
    }

    void UpdateBuildingLv()
    {
        int woodCutterEXP = GameResourceDataModel.BuildingResouces.woodCutterExp;
        int mineEXP = GameResourceDataModel.BuildingResouces.mineExp;
        int farmEXP = GameResourceDataModel.BuildingResouces.farmExp;
        int townEXP = GameResourceDataModel.BuildingResouces.townExp;

        int woodCutterLv = GameFormular.CalculateEXPToLv(woodCutterEXP);
        int mineLv = GameFormular.CalculateEXPToLv(mineEXP);
        int farmLv = GameFormular.CalculateEXPToLv(farmEXP);
        int townLv = GameFormular.CalculateEXPToLv(townEXP);

        _woodCutterController.UpgradeBuilding();
        _mineController.UpgradeBuilding();
        _farmController.UpgradeBuilding();
        _townController.UpgradeBuilding();
    }

    void UpdateNaturalResource()
    {
        int forestEXP = GameResourceDataModel.NaturalResources.forestExp;
        int waterExp = GameResourceDataModel.NaturalResources.waterExp;

        _forestController.UpgradeBuilding();
    }

}
