using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStatic : MonoBehaviour {
    public static Material mat;
	// Use this for initialization
	void Start () {
        mat = Resources.Load<Material>("test");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
