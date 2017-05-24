using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnSelectCardController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    private OnCardController onCardController;
    public bool IsSelected = false;
    public Sprite DefaultCardBg;
    public string CardDescription;
    public GameObject DialogDialogPoint;

    public Sprite[] CardSprite;

    GameObject CardDescriptionDialogObj;
    
    public Vector2 DialogPosition;

    public void Start()
    {
        CardDescriptionDialogObj = GameObject.Find("CardDiscriptionPanel");
        onCardController = gameObject.GetComponent<OnCardController>();
    }

    public void OnSelectCard()
    {
        if (!IsSelected)
        {
            SetTransperentOtherCards();
            this.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 0);
            this.gameObject.GetComponent<Image>().sprite = CardSprite[1];

            IsSelected = true;
            SelectedCardDataModel.SelectedCardKeyName = onCardController.cardKeyname;
        }
        else
        {
            SetDefaultOtherCards();

            IsSelected = false;
            SelectedCardDataModel.SelectedCardKeyName = null;
        }      
    }

    public void SetTransperentOtherCards()
    {
        GameObject[] currentCards = GameObject.FindGameObjectsWithTag("Card");
        for (int unit = 0; unit < currentCards.Length; unit++)
        {
            if(currentCards[unit] != this.gameObject)
            {
                currentCards[unit].GetComponent<Image>().sprite = currentCards[unit].GetComponent<OnSelectCardController>().CardSprite[0];
                currentCards[unit].transform.localScale = new Vector3(0.8f, 0.8f, 0);

                OnSelectCardController onSelectCardController = currentCards[unit].GetComponent<OnSelectCardController>();
                onSelectCardController.SetCardSelectState(false);
            }
        }
    }

    public void SetDefaultOtherCards()
    {
        GameObject[] currentCards = GameObject.FindGameObjectsWithTag("Card");
        for (int unit = 0; unit < currentCards.Length; unit++)
        {
            currentCards[unit].GetComponent<Image>().sprite = currentCards[unit].GetComponent<OnSelectCardController>().CardSprite[1];
            currentCards[unit].transform.localScale = new Vector3(0.95f, 0.95f, 0); 
        }
    }

    public void SetCardSelectState(bool isSelect)
    {
        IsSelected = isSelect;
    }

    IEnumerator StartHoldTimer()
    { 
        CardDescriptionDialogObj.transform.position = DialogPosition;
        yield return new WaitForSeconds(1f);
        IsCardDescriptionVisible(true);
        UpdateDescription();
        Debug.Log("Hold");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        float x = DialogDialogPoint.transform.localPosition.x + this.gameObject.transform.position.x;
        float y = DialogDialogPoint.transform.localPosition.y + this.gameObject.transform.position.y;
        DialogPosition = new Vector2(x, y);

        StartCoroutine("StartHoldTimer");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine("StartHoldTimer");
        IsCardDescriptionVisible(false);
    }

    void IsCardDescriptionVisible(bool isVisible)
    {
        int childrenUnit = CardDescriptionDialogObj.transform.GetChildCount();
        for (int unit = 0; unit < childrenUnit; unit++)
        {
            CardDescriptionDialogObj.transform.GetChild(unit).gameObject.SetActive(isVisible);
        }
    }

    void UpdateDescription()
    {
        Text descriptionTxt = CardDescriptionDialogObj.transform.GetChild(1).GetComponent<Text>();
        descriptionTxt.text = CardDescription;
    }
}
