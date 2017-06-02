using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class CustomEditorTools {
    public static SceneView.OnSceneFunc sceneFuc;
    public static void OpenSceneViewGUI()
    {
        if (sceneFuc == null)
        {
            sceneFuc = new SceneView.OnSceneFunc(CallSceneFuncGUI);
            SceneView.onSceneGUIDelegate += sceneFuc;
        }
    }

    public static void CloseSceneViewGUI()
    {
        if (sceneFuc != null)
        {
            SceneView.onSceneGUIDelegate -= sceneFuc;
            sceneFuc = null;
        }
    }

    public static void CallSceneFuncGUI(SceneView sceneView)
    {
        Event curEvent = Event.current;

        if (curEvent.type == EventType.Layout)
        {
            //置尾任意用户 内部操作 最后控制 系统操作
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }

        Camera cameara = sceneView.camera;
        Ray ray = HandleUtility.GUIPointToWorldRay(curEvent.mousePosition);
        RaycastHit _hitInfo;
        if (Physics.Raycast(ray, out _hitInfo, 10000, -1))
        {
            Vector3 origin = _hitInfo.point;
            origin.y += 100;
            if (Physics.Raycast(origin, Vector3.down, out _hitInfo))
            {
                Handles.color = Color.yellow;
                Handles.DrawLine(_hitInfo.point, origin);
                float arrowSize = 1;
                Vector3 pos = _hitInfo.point;
                Quaternion quat;
                Handles.color = Color.green;
                quat = Quaternion.LookRotation(Vector3.up, Vector3.up);
                Handles.ArrowCap(0, pos, quat, arrowSize);
                Handles.color = Color.red;
                quat = Quaternion.LookRotation(Vector3.right, Vector3.up);
                Handles.ArrowCap(0, pos, quat, arrowSize);
                Handles.color = Color.blue;
                quat = Quaternion.LookRotation(Vector3.forward, Vector3.up);
                Handles.ArrowCap(0, pos, quat, arrowSize);
                //Handles.DrawLine(pos + new Vector3(0, 3, 0), pos);  
            }
        }

        if (curEvent.clickCount >= 2)
        {
            Debug.Log("curEvent.clickCount:" + curEvent.clickCount);
            curEvent.Use();
        }

        if (curEvent.clickCount == 1 && curEvent.control)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("test0"), false, delegate
            {
                Debug.Log("Click GenericMenu test0");
            });
            menu.AddItem(new GUIContent("test1"), false, delegate(object userData)
            {
                Debug.Log("Click GenericMenu test1:" + (string)userData);
            }, "item 1");
            menu.AddItem(new GUIContent("test2"), false, delegate(object userData)
            {
                Debug.Log("Click GenericMenu test2:" + (string)userData);
            }, "item 2");
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("SubMenu/test3"), false, delegate(object userData)
            {
                Debug.Log("Click GenericMenu test3:" + (string)userData);
            }, "item 3");
            menu.ShowAsContext();
            curEvent.Use();
        }
        Vector3 cont = new Vector3(50, 50, 0);
        float size = HandleUtility.GetHandleSize(cont);
        Handles.Label(cont, "CLickEvent");
        Handles.Slider(cont + new Vector3(0, 4, 0), Vector3.back, size, ScaleDrawSize(size * .7f, Handles.ConeCap), 0f);

        if (Handles.Button(cont + new Vector3(0, 4, 0), Quaternion.identity, size, size * .7f, Handles.ConeCap))
        {
            Debug.Log("Click This Call Back ConeCap");
        }

        if (Handles.Button(cont + new Vector3(0, -4, 0), Quaternion.identity, size, size * .7f, Handles.CylinderCap))
        {
            Debug.Log("Click This Call Back CylinderCap");
        }

        if (Handles.Button(cont + new Vector3(0, -8, 0), Quaternion.identity, size, size * .7f, Handles.CubeCap))
        {
            Debug.Log("Click This Call Back CubeCap");
        }

        SceneView.RepaintAll();  
    }

    protected static Handles.DrawCapFunction ScaleDrawSize(float scale, Handles.DrawCapFunction srcFunc)
    {
        return (id, pos, rotation, size) => srcFunc(id, pos, rotation, size * scale);
    }
}
