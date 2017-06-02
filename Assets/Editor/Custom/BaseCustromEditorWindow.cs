using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class BaseCustromEditorWindow : EditorWindow, IHasCustomMenu
{
    protected  EditorApplication.HierarchyWindowItemCallback hierarchyCall;
    protected SceneView.OnSceneFunc sceneFuc;
    protected bool forbidTools = false;
    private bool useResumeTools = false;
    private bool usePreference = false;

    private GUIStyle lockButtonStyle;
    protected bool locked = false;

    public void ShowButton(Rect position)
    {
        if (lockButtonStyle == null) lockButtonStyle = "IN LockButton";
        locked = GUI.Toggle(position, locked, GUIContent.none, lockButtonStyle);
    }
    //嵌套接口（public xx 或是 父类.xx(不能有权限修饰符)）
    void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
    {
        menu.AddItem(new GUIContent("Lock"), locked, () =>
        {
            locked = !locked;
        });
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void OnDestroy()
    {
        OnClear();
    }

    protected virtual void OnClear()
    {
        CloseHierarchyItemGUI();
        CloseSceneViewGUI();
        CustomEditorTools.ClearTimeManager();
    }

    protected void UseToolBars()
    {
        if (forbidTools)
        {
            CustomEditorTools.HideToolBars();
            useResumeTools = false;
        }
        else
        {
            if (!useResumeTools)
            {
                CustomEditorTools.StartToolBars();
                useResumeTools = true;
            }
        }
    }

    protected void OpenSceneViewGUI()
    {
        if (sceneFuc == null)
        {
            sceneFuc = new SceneView.OnSceneFunc(CallSceneFuncGUI);
            SceneView.onSceneGUIDelegate += sceneFuc;
        }
    }

    protected void CloseSceneViewGUI()
    {
        if (sceneFuc != null)
        {
            SceneView.onSceneGUIDelegate -= sceneFuc;
            sceneFuc = null;
        }
    }

    protected void OpenHierarchyItemGUI()
    {
        if (hierarchyCall == null)
        {
            hierarchyCall = new EditorApplication.HierarchyWindowItemCallback(CallHierarchyItemGUI);
            EditorApplication.hierarchyWindowItemOnGUI += hierarchyCall;
        }
    }

    protected void CloseHierarchyItemGUI()
    {
        if (hierarchyCall != null)
        {
            EditorApplication.hierarchyWindowItemOnGUI -= hierarchyCall;
            hierarchyCall = null;
        }
    }

    protected virtual void CallSceneFuncGUI(SceneView sceneView)
    {
        UseToolBars();
        CustomEditorTools.CallSceneFuncGUI(sceneView);
    }

    protected virtual void CallHierarchyItemGUI(int instanceID, Rect selectionRect)
    {
        CustomEditorTools.CallHierarchyItemGUI(instanceID, selectionRect);
    }

    static void OnToolChangedFunc(Tool from, Tool to)
    {
        Debug.Log(from.ToString() + "_" + to.ToString());
    }

    protected virtual void OnGUI()
    {
        CustomEditorTools.DrawAdvancedList("ListTest", new string[] { "1", "2", "3" }, "2");
        CustomEditorTools.DrawList("ListTest2", new string[] { "1", "2", "3" }, "2");
        CustomEditorTools.DrawHeader("Liming");
        CustomEditorTools.DrawSeparator();
        usePreference = EditorGUILayout.BeginToggleGroup("Preference", usePreference);
        if (usePreference)
        {
            forbidTools = EditorGUILayout.ToggleLeft("ForbidTools", forbidTools, GUILayout.ExpandWidth(true));
        }
        EditorGUILayout.EndToggleGroup();

        if (GUILayout.Button("useHierarchyItem"))
        {
            OpenHierarchyItemGUI();
        }

        if (GUILayout.Button("useSceneFuncGUI"))
        {
            OpenSceneViewGUI();
        }


        if (GUILayout.Button("TestToolBars"))
        {
            System.Reflection.MethodInfo method = typeof(BaseCustromEditorWindow).GetMethod("OnToolChangedFunc", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            CustomEditorTools.ToggleListenToolBars(method);
        }

        if (GUILayout.Button("TestDiaglog"))
        {
            CustomEditorTools.WaitToExcute("---WaitToExcute", 5f, 5f);
            CustomEditorTools.RegisterCallBack("---WaitToExcute", delegate {
                Debug.Log("--------------------------------->>>>");
            });
        }

        if (GUILayout.Button("TestDiaglog2"))
        {
            CustomEditorTools.MoveSceneCamera2DView(10f);
        }
        if (GUILayout.Button("TestDiaglog3"))
        {
            CustomEditorTools.MoveSceneCameraFocus(Vector3.back * 10);
        }
        if (GUILayout.Button("TestDiaglog4"))
        {
            CustomEditorTools.GetClassID(typeof(TestArray));
        }

        if (GUILayout.Button("TestDiaglog5"))
        {
            GameObject o = EditorUtility.CreateGameObjectWithHideFlags("name",HideFlags.None,typeof(TestArray));
            CustomEditorTools.ReplaceClass(o.GetComponent<TestArray>(),typeof(TestMove));
        }

        if (GUILayout.Button("TestDiaglog6"))
        {
            TestArray t = AssetDatabase.LoadAssetAtPath<TestArray>("Assets/Resources/Test3.prefab");
            //Debug.Log((t==null) + "_" + t.name);
        }

        if (GUILayout.Button("TestDiaglog7"))
        {
           /* EditorApplication.searchChanged += delegate()
            {
                Debug.Log("--1-----------------");
            };*/
            CustomEditorTools.ToggleSearchUpdateCallBack(t);
        }
        if (GUILayout.Button("TestDiaglog8")) {
            CustomEditorTools.ToggleProjectWindowItemOnGUI();
        }

        if (GUILayout.Button("TestDiaglog9"))
        {
            CustomEditorTools.ToggleModifierKeysCallBack(t);
        }

        //CustomEditorTools.AddCurseRect(new Rect(10,10,50,50));
    }


    void t()
    {
        Debug.Log("777777777777777777777777");
    }
}
