using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnCardController : MonoBehaviour {

    [Header("Card Info")]
    public string cardKeyname;
    public int stoneUsage = 0;
    public int woodUsage = 0;

    public Text stoneUsageTxt;
    public Text woodUsageTxt;

    void Start()
    {
        UpdateCardData();
    }

    public void UpdateCardData()
    {
        if (cardKeyname == "FARM")
        {
            stoneUsage = CardDataModel.FarmCardModel.stoneUsage;
            woodUsage = CardDataModel.FarmCardModel.woodUsage;
            stoneUsageTxt.text = CardDataModel.FarmCardModel.stoneUsage.ToString();
            woodUsageTxt.text = CardDataModel.FarmCardModel.woodUsage.ToString();
        }
        else if (cardKeyname == "MINE")
        {
            stoneUsage = CardDataModel.MineCardModel.stoneUsage;
            woodUsage = CardDataModel.MineCardModel.woodUsage;

            woodUsageTxt.text = CardDataModel.MineCardModel.woodUsage.ToString();
        }
        else if(cardKeyname == "WOODCUTTER")
        {
            stoneUsage = CardDataModel.WoodCutterCardModel.stoneUsage;
            woodUsage = CardDataModel.WoodCutterCardModel.woodUsage;

            stoneUsageTxt.text = CardDataModel.WoodCutterCardModel.stoneUsage.ToString();
        }
        else if(cardKeyname == "TOWN")
        {
            stoneUsage = CardDataModel.TownCardModel.stoneUsage;
            woodUsage = CardDataModel.TownCardModel.woodUsage;

            woodUsageTxt.text = CardDataModel.TownCardModel.woodUsage.ToString();
            stoneUsageTxt.text = CardDataModel.TownCardModel.stoneUsage.ToString();
        }
        else if(cardKeyname == "FOREST")
        {
            stoneUsage = CardDataModel.ForestCardModel.stoneUsage;
            woodUsage = CardDataModel.ForestCardModel.woodUsage;

        }
        else if (cardKeyname == "RAIN")
        {
            stoneUsage = CardDataModel.WaterCardModel.stoneUsage;
            woodUsage = CardDataModel.WaterCardModel.woodUsage;

        }
    }
}
