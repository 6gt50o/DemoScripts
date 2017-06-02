using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDelegate : MonoBehaviour {
    public delegate void _TestDelegate();
    _TestDelegate td;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            td += t;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            td -= t;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            td = null;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (td != null) td();
        }
	}

    void t()
    {
        Debug.Log("-------------------------------");
    }
}
