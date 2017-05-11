using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnSelectCardController : MonoBehaviour {
    private OnCardController onCardController;
    public bool IsSelected = false;
    public Sprite DefaultCardBg;

    public void Start()
    {
        onCardController = gameObject.GetComponent<OnCardController>();
    }

    public void OnSelectCard()
    {
        if (!IsSelected)
        {
            SetTransperentOtherCards();
            this.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 0);
            this.gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

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
                currentCards[unit].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
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
            currentCards[unit].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            currentCards[unit].transform.localScale = new Vector3(0.95f, 0.95f, 0); 
        }
    }

    public void SetCardSelectState(bool isSelect)
    {
        IsSelected = isSelect;
    }
}
