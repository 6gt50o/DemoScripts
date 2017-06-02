using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;

//[InitializeOnLoad]
public static class PreactionInitLoad {


    //[PostProcessBuild(1)]
    //public static void OnPostProcessBuild(BuildTarget target, string path)
    //{
    //    Debug.Log("预编译" + target.ToString());
    //}

    //[PostProcessScene(100)]
    //static void ProcessScene()
    //{
    //    Debug.Log("预编译场景");
    //}
    public static bool isOK = false;

    [InitializeOnLoadMethod]
    static void Init()
    {
        //SceneView.onSceneGUIDelegate = delegate(SceneView sceneView)
        //{
        //    Event e = Event.current;
        //    if (Selection.activeTransform)
        //    {
        //        if (e.keyCode == KeyCode.D)
        //        //if (Event.current.Equals(Event.KeyboardEvent("D")))
        //        {
        //            //Debug.Log("点击事件3");
        //            if (e.type == EventType.Repaint)
        //            {
        //                //Debug.Log("点击事件2");
        //                return;
        //            }
        //        }
        //    }
        //};

        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android && !isOK)
        {
            isOK = true;
            EditorUserBuildSettings.activeBuildTargetChanged = delegate() {
                Debug.Log("Switch To Android!");
            };
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
        }

    }

    //static PreactionInitLoad()
    // {
    //     SceneView.onSceneGUIDelegate += OnSceneGUI;
    // }
 
    // private static void OnSceneGUI(SceneView sceneView)
    // {
    //     // Do your general-purpose scene gui stuff here...
    //     // Applies to all scene views regardless of selection!
 
    //     // You'll need a control id to avoid messing with other tools!
    //     int controlID = GUIUtility.GetControlID(FocusType.Passive);
 
    //     if (Event.current.GetTypeForControl(controlID) == EventType.keyDown)
    //     {
    //         if (Event.current.keyCode == KeyCode.D)
    //         {
    //             Debug.Log("1 pressed!");
    //             // Causes repaint & accepts event has been handled
    //             //Event.current.Use();
    //         }
    //     }
    // }
}
