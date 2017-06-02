using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BaseCustomEditor : Editor {

    //[MenuItem("GameTools/Play&Stop _0")]

    [MenuItem("GameTools/Play&Stop")]
    static void PlayGame()
    {
        CustomEditorTools.PlayGame();
    }


    [MenuItem("GameTools/Pause&Resume")]
    static void PauseGame()
    {
        CustomEditorTools.PauseGame();
    }

    

    [MenuItem("GameTools/Focus")]
    static void FocusGame()
    {
        CustomEditorTools.DoFocusGame();
    }

    [MenuItem("GameTools/JustTest")]
    static void testUItools() {

       GameObject g =  Selection.activeGameObject;
       Debug.Log((PrefabUtility.GetPrefabObject(g) == null) + "_" + (PrefabUtility.GetPrefabParent(g)==null) + "_" + PrefabUtility.GetPrefabType(g));
    }

    [MenuItem("GameTools/Focus Test")]
    static void testFocusWindow()
    {
        var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.ProfilerWindow");
        /*var type = System.Type.GetType("UnityEditor.ProfilerWindow,UnityEditor");*/
        EditorWindow.GetWindow(type, true);

        //var type1 = System.Type.GetType("UnityEditor.GameView,UnityEditor");//唤醒\新建
        //EditorWindow.GetWindow(type1, true);//工具栏 锁死状态
    }

    //[DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    [DrawGizmo(GizmoType.InSelectionHierarchy)]
    static void DrawGameObjectName(Transform transform, GizmoType gizmoType)
    {
        Handles.Label(transform.position, transform.gameObject.name);
    }
}
