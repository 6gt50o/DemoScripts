using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class CustomEditorTools {
    public static EditorApplication.HierarchyWindowItemCallback hierarchyCall;

    public static void OpenHierarchyItemGUI()
    {
        if (hierarchyCall == null)
        {
            hierarchyCall = new EditorApplication.HierarchyWindowItemCallback(CallHierarchyItemGUI);
            EditorApplication.hierarchyWindowItemOnGUI += hierarchyCall;
        }
    }

    public static void CloseHierarchyItemGUI()
    {
        if (hierarchyCall != null)
        {
            EditorApplication.hierarchyWindowItemOnGUI -= hierarchyCall;
            hierarchyCall = null;
        }
    }

    public static void CallHierarchyItemGUI(int instanceID, Rect selectionRect)
    {
        Event curEvent = Event.current;
        if (curEvent != null && curEvent.button == 1 && curEvent.type == EventType.MouseUp && selectionRect.Contains(curEvent.mousePosition))
        {
            GameObject selectObj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (selectObj)
            {
                //EditorUtility.DisplayPopupMenu(new Rect(curEvent.mousePosition.x, curEvent.mousePosition.y, 0, 0), "ToolBar/",null);
                //EditorUtility.DisplayPopupMenu(new Rect(curEvent.mousePosition.x, curEvent.mousePosition.y,0,0), "Edit", null);
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
                menu.AddSeparator("");//空白分割
                menu.AddItem(new GUIContent("SubMenu/test3"), false, delegate(object userData)
                {
                    Debug.Log("Click GenericMenu test3:" + (string)userData);
                }, "item 3");
                menu.ShowAsContext();
                curEvent.Use();
            }
        }
    }

}
