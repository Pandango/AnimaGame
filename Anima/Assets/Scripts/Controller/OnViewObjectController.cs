using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnViewObjectController : MonoBehaviour {
    public GameObject GameObjectiveDialog;
    bool isGameObjectiveDialogVisible = false;

    public void OnDisplayGameObjective()
    {
        if (isGameObjectiveDialogVisible)
        {
            GameObjectiveDialog.SetActive(false);
            isGameObjectiveDialogVisible = false;
        }
        else
        {
            GameObjectiveDialog.SetActive(true);
            isGameObjectiveDialogVisible = true;
        }
    }
}
