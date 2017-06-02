using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotate : MonoBehaviour {
    public Transform oldTrans;
    protected Vector3 posDirection;
    protected Vector3 newDirection;
    protected bool useFirst = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            useFirst = !useFirst;

        }
        //渐变位置朝向 位置旋转量
        if (useFirst)
        {
            //Quaternion
            posDirection = transform.position - oldTrans.position;
            transform.forward = Vector3.Lerp(transform.forward, posDirection.normalized, 0.1f);
        }
        else {
            posDirection = transform.position - oldTrans.position;
            newDirection = Vector3.RotateTowards(transform.forward,posDirection,0.1f,0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
	}
}
