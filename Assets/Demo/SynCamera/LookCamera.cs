using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class LookCamera : MonoBehaviour {
    private Camera cam;
    private Vector3 originPos;
    private Vector3 eulerAngle;
    void Start()
    {
#if !UNITY_EDITOR
        Object.DestroyImmediate(this);
#endif
    }

    void OnEnable()
    {
        originPos = transform.position;
        eulerAngle = transform.eulerAngles;
    }

    void OnDisable()
    {
        transform.position = originPos;
        transform.eulerAngles = eulerAngle;
    }

    void OnRenderObject()
    {
        cam = Camera.current;
        if (cam != null && cam.name == "SceneCamera")
        {
            Vector3 pos = cam.transform.position;
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
            //transform.eulerAngles = cam.transform.eulerAngles;
        }
    }
}
