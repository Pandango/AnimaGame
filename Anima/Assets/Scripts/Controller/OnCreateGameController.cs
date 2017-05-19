using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnCreateGameController : MonoBehaviour {
    public GameSocketHandler gameSocketHandler;

    [Header("GameObjective setter")]
    public Text GameObjectiveTxt;

    [Header("Sharing Resource UI")]
    public Text WoodUnitTxt;
    public Text StoneUnitTxt;
    public Text TotalPopulationTxt;

    [Header("BuildingGameObject")]
    public List<GameObject> buildingGameObj;
    public List<GameObject> naturalGameObj;

    [Header("Natural Building Lv")]
    public Text ForestLvTxt;

    [Header("Notify")]
    public GameObject WaterNotify;

    [Header("PopFoodBalancer")]
    public GameObject PositivePopFoodBar;
    public GameObject NegativePopFoodBar;

    private Image _positivePopFoodBarImage;
    private Image _negativFoodBarImage;

    private OnBuildingUpgradeController _woodCutterController;
    private OnBuildingUpgradeController _mineController;
    private OnBuildingUpgradeController _farmController;
    private OnBuildingUpgradeController _townController;

    private OnBuildingUpgradeController _forestController;
    private OnWaterUpgradeController _waterController;

    [Header("Sound")]
    public AudioSource bgmSound;

    void Start () {
        bgmSound.Play();
        //MockUpResource();
        _woodCutterController = buildingGameObj[0].GetComponent<OnBuildingUpgradeController>();
        _mineController = buildingGameObj[1].GetComponent<OnBuildingUpgradeController>();
        _farmController = buildingGameObj[2].GetComponent<OnBuildingUpgradeController>();
        _townController = buildingGameObj[3].GetComponent<OnBuildingUpgradeController>();

        _forestController = naturalGameObj[0].GetComponent<OnBuildingUpgradeController>();
        _waterController = naturalGameObj[1].GetComponent<OnWaterUpgradeController>();

        _positivePopFoodBarImage = PositivePopFoodBar.GetComponent<Image>();
        _negativFoodBarImage = NegativePopFoodBar.GetComponent<Image>();

        UpdateGameResource();
        OnUpdateGameObjective();

        IntializeSocket();
    }

    void IntializeSocket()
    {       
        gameSocketHandler.GetUpdatedGameResource(UpdateGameResource);
        gameSocketHandler.GetGameOver(OnLoadGameOverScene);
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

    public void OnUpdateGameObjective()
    {
        string currentGameobjectiveKey = GameObjectiveDataModel.CurrentGameObjective;
        string gameObjectiveDescription = Utilities.GenerateGameObjectiveDescription(currentGameobjectiveKey);

        GameObjectiveTxt.text = gameObjectiveDescription;
    }

    public void UpdateGameResource()
    {
        UpdateSharingResource();
        UpdateBuildingLv();
        UpdateNaturalResource();
        UpdatePopFoodBar();

        CheckGameOver();
    }

    void UpdateSharingResource()
    {
        int stoneUnit = GameResourceDataModel.SharingResources.stone;
        int woodUnit = GameResourceDataModel.SharingResources.wood;
        int population = GameResourceDataModel.PopulationFood.population;

        WoodUnitTxt.text = Utilities.FormatResourceUnit(woodUnit);
        StoneUnitTxt.text = Utilities.FormatResourceUnit(stoneUnit);
        TotalPopulationTxt.text = population.ToString();

        CheckGameObjective();
    }

    void UpdateBuildingLv()
    {
        _woodCutterController.UpgradeBuilding();
        _mineController.UpgradeBuilding();
        _farmController.UpgradeBuilding();
        _townController.UpgradeBuilding();

        CheckGameObjective();
    }

    void UpdateNaturalResource()
    {
        int forestEXP = GameResourceDataModel.NaturalResources.forestExp;
        int waterExp = GameResourceDataModel.NaturalResources.waterExp;

        _forestController.UpgradeBuilding();
        _waterController.UpgradeWater();

        OnWaterLevelOver();
        CheckGameObjective();
    }

    void UpdatePopFoodBar()
    {
        int poppulationUnit = GameResourceDataModel.PopulationFood.population;
        int foodUnit = GameResourceDataModel.PopulationFood.food;

        float balancedPercent = GameFormular.CalPopFoodBalanced(poppulationUnit, foodUnit);

        float balanceRatio = GameFormular.ConvertPercentToRatio(balancedPercent);

        if(balancedPercent >= 100)
        {
            float remainBlanceRatio = balanceRatio - 1f;

            if (balancedPercent >= 200)
            {
                balanceRatio = 1f;
            }
            _positivePopFoodBarImage.fillAmount = balanceRatio;
            _negativFoodBarImage.fillAmount = 0f;
        }
        else
        {
            float remainBlanceRatio = 1f - balanceRatio;
            _negativFoodBarImage.fillAmount = remainBlanceRatio;
            _positivePopFoodBarImage.fillAmount = 0f;
        }
        
        Debug.Log("BalancedPercent : " + balancedPercent);

    }

    public void CheckGameObjective()
    {
        string gameObjective = GameObjectiveDataModel.CurrentGameObjective;
        int objectiveLv = 0;

        if (gameObjective == GameObjectiveModel.FarmMax)
        {
            objectiveLv = GameFormular.CalculateEXPToLv(GameResourceDataModel.BuildingResouces.farmExp);
        }
        else if (gameObjective == GameObjectiveModel.ForestMax)
        {
            objectiveLv = GameFormular.CalculateEXPToLv(GameResourceDataModel.NaturalResources.forestExp);       
        }
        else if (gameObjective == GameObjectiveModel.MineMax)
        {
            objectiveLv = GameFormular.CalculateEXPToLv(GameResourceDataModel.BuildingResouces.mineExp);
        }
        else if (gameObjective == GameObjectiveModel.TownMax)
        {
            objectiveLv = GameFormular.CalculateEXPToLv(GameResourceDataModel.BuildingResouces.townExp);
        }
        else if (gameObjective == GameObjectiveModel.WoodCutterMax)
        {
            objectiveLv = GameFormular.CalculateEXPToLv(GameResourceDataModel.BuildingResouces.woodCutterExp);
        }

        IsMissionComplete(objectiveLv);
    }

    void IsMissionComplete(int currentLv)
    {
        int maxLevel = Utilities.MaximumAllowLevel;
        if(currentLv >= maxLevel)
        {
            string gameObjectiveDescription = GameObjectiveDataModel.CurrentGameObjective;
            GameOverModel.isMissionComplete = true;
            GameOverModel.description = Utilities.GenerateGameObjectiveDescription(gameObjectiveDescription);

            gameSocketHandler.SendReqGameOver();
        }
    }

    public void CheckGameOver()
    {  
        int currentPopulation = GameResourceDataModel.PopulationFood.population;

        if (currentPopulation <= 0)
        {
            GameOverModel.isMissionComplete = false;
            GameOverModel.description = SituationDescription.Desolation;

            gameSocketHandler.SendReqGameOver();
        }    
    }

    public void OnWaterLevelOver()
    {
        int waterLevel = GameFormular.CalculateEXPToLv(GameResourceDataModel.NaturalResources.waterExp);

        int allowMinimunLv = GameOverCauseByWaterLvStat.WaterLevelOverDesolated;
        int allowMaximumLv = GameOverCauseByWaterLvStat.WaterLevelOverFlood;

        int notifyMinumLv = GameOverCauseByWaterLvStat.WaterLevelDesolated;
        int notifyMaximunLv = GameOverCauseByWaterLvStat.WaterLevelFlood;

        if (waterLevel <= allowMinimunLv)
        {
            GameOverModel.isMissionComplete = false;
            GameOverModel.description = SituationDescription.Desolation;

            gameSocketHandler.SendReqGameOver();
        }
        else if (waterLevel > allowMinimunLv && waterLevel <= notifyMinumLv)
        {
            WaterNotify.SetActive(true);
            
        }
        else if (waterLevel >= notifyMaximunLv && waterLevel < allowMaximumLv)
        {
            WaterNotify.SetActive(true);
        }
        else if (waterLevel >= allowMaximumLv)
        {
            GameOverModel.isMissionComplete = false;
            GameOverModel.description = SituationDescription.Flood;

            gameSocketHandler.SendReqGameOver();
        }
        else
        {
            WaterNotify.SetActive(false);
        }
    }

    void OnLoadGameOverScene()
    {
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        //StartCoroutine("WaitForLoadingScene");
    }

    IEnumerator WaitForLoadingScene()
    {
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        yield return new WaitForSeconds(2f);  
    }
}
