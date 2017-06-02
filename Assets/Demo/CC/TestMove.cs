using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//必须要有碰撞刚体（可以嵌套，父没有子有）
//优先触发 渐变向量渐变CC触发
public class TestMove : MonoBehaviour {
    CharacterController control;
    public Transform[] points;
    //下一个点的下标，主角移动速度  
    public int nextIndex;  
    public int moveSpeed = 10;
    bool isRotate = true;
	// Use this for initialization
	void Start () {
        nextIndex = 0;
        control = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isRotate = !isRotate;
        }
       //如果主角距离点的距离大于0.2，则算出主角的朝向，移动主角人物  
        if (Vector3.Distance(ignoreY(points[nextIndex % points.Length].position), ignoreY(transform.position)) > 1.3f)  
        {  
            //主角的朝向即为下一个点坐标减去主角坐标的向量  
            Vector3 direction = (ignoreY(points[nextIndex % points.Length].position) - ignoreY(transform.position)).normalized;  
            //插值改变主角的朝向，使其有一个自然转向的过程，防止其瞬间转向  
            //control.transform.forward = Vector3.Lerp(transform.forward, direction, 0.1f);  
            control.transform.forward = Vector3.Lerp(transform.forward, direction, 0.1f);  
            //移动主角  
            control.SimpleMove((isRotate ? transform.forward : direction) * moveSpeed);  
            //control.SimpleMove((control.transform.forward) * moveSpeed);  
        }  
        else  
        {  
            //如果到达点，则使下一点作为目标点  
            nextIndex++;
        }  
	}

    //这个函数用来取消向量的Y轴影响，比如主角的高度与点之间可能有一段距离，我们要忽略这段距离  
    Vector3 ignoreY(Vector3 v3)
    {
        return new Vector3(v3.x, 0, v3.z);
    }


    public float pushPower = 2.0F;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
        {
            //Debug.Log("Rigidbody------------>None");
            return;
        }

        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
    }
}
