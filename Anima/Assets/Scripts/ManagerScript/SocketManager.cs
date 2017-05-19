using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketManager : MonoBehaviour {

	void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        //if (GameObject.Find("SocketIO") != null)
        //{
        //    Destroy(this.gameObject);
        //}
    }
}
