using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestWindow : BaseCustromEditorWindow {

    //protected override void CallSceneFuncGUI(SceneView sceneView)
    //{
    //    CustomEditorTools.CallSceneFuncGUI(sceneView);
    //}

    //protected override void CallHierarchyItemGUI(int instanceID, Rect selectionRect)
    //{
    //    CustomEditorTools.CallHierarchyItemGUI(instanceID, selectionRect);
    //}

    [MenuItem("GameTools/TestWindow _9")]
    static void Test()
    {
        TestWindow t = EditorWindow.GetWindow<TestWindow>("TestWindow", true);
        t.Show();
    }
}
