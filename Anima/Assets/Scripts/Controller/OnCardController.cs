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
        if(cardKeyname == "FARM")
        {
            stoneUsageTxt.text = CardDataModel.FarmCardModel.stoneUsage.ToString();
            woodUsageTxt.text = CardDataModel.FarmCardModel.woodUsage.ToString();
        }else if (cardKeyname == "MINE")
        {
            woodUsageTxt.text = CardDataModel.MineCardModel.woodUsage.ToString();
        }else if(cardKeyname == "WOODCUTTER")
        {
            stoneUsageTxt.text = CardDataModel.WoodCutterCardModel.stoneUsage.ToString();
        }else if(cardKeyname == "TOWN")
        {
            woodUsageTxt.text = CardDataModel.TownCardModel.woodUsage.ToString();
            stoneUsageTxt.text = CardDataModel.TownCardModel.stoneUsage.ToString();
        }
    }
}
