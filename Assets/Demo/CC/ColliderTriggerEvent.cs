using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTriggerEvent : MonoBehaviour {

    public bool UseKeyEvent = false;
    protected void OnCollisionEnter(Collision collider)
    {
        Debug.Log("OnCollisionEnter"+ collider.collider.name);
    }

    protected void OnCollisionStay(Collision collider)
    {
        Debug.Log("OnCollisionStay" + collider.collider.name);
    }

    protected void OnCollisionExit(Collision collider)
    {
        Debug.Log("OnCollisionExit" + collider.collider.name);
    }

    protected void OnTriggerStay(Collider collider)
    {
        //Debug.Log("OnTriggerStay"+ collider.name);
    }

    protected void OnTriggerEnter(Collider collider)
    {
        Debug.Log("OnTriggerEnter" + collider.name);
    }

    protected void OnTriggerExit(Collider collider)
    {
        Debug.Log("OnTriggerExit" + collider.name);
    }

    public static ColliderTriggerEvent Get(GameObject collider) {
        ColliderTriggerEvent evt = collider.GetComponent<ColliderTriggerEvent>();
        if (evt == null)
        {
            evt = collider.AddComponent<ColliderTriggerEvent>();
        }
        return evt;
    }

    void Update()
    {
        if (UseKeyEvent && Input.GetKeyDown(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        }
    }
}
