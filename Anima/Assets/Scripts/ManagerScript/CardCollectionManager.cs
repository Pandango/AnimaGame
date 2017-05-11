using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollectionManager : MonoBehaviour {

    public GameObject CardCollectionRowObj;

    private float DefaultRowWidthSize;
	// Use this for initialization
	void Start () {
        DefaultRowWidthSize = CardCollectionRowObj.GetComponent<RectTransform>().rect.width;
        
    }

    public void SetupWidthOfRow(int CardUnit)
    {
        RectTransform cardCollectionRect = CardCollectionRowObj.GetComponent<RectTransform>();
        float newWidth = (DefaultRowWidthSize * 0.16f) * CardUnit;
        cardCollectionRect.SetSizeWithCurrentAnchors(0, newWidth);
    }
}
