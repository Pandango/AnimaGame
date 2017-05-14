using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnWaterUpgradeController : MonoBehaviour {
    public List<Vector2> WaterLevelTransform;

    [Header("BG setter")]
    public GameObject GameBGDefaultSprite;
    public GameObject GameBGDesolateSprite;

    void Start()
    {
        UpgradeWater();
    }

    public void UpgradeWater()
    {
        int exp = GameResourceDataModel.NaturalResources.waterExp;
        int waterLv = GameFormular.CalculateEXPToLv(exp);
        
        this.gameObject.transform.localPosition = WaterLevelTransform[waterLv - 1];

        UpdateBGSprite(exp);
    }

    void UpdateBGSprite(int exp)
    {
        int waterLv = GameFormular.CalculateEXPToLv(exp);
        if (waterLv <= GameOverCauseByWaterLvStat.WaterLevelDesolated)
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

}
