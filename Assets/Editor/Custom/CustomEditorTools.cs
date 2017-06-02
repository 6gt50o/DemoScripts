using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
/// <summary>
/// 竟然可以 任意static partial，一部分有一部分类没有,必须全部public
/// 断言Asset相当于宏
/// </summary>
public partial class CustomEditorTools
{

    protected static UnityEditor.EditorWindow gameViewWindow;
    protected static void FocusGameViewWindow()
    {
        System.Type gameViewType = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        //object gameViewObj = System.Activator.CreateInstance(gameViewType);
        //System.Reflection.MethodInfo GameViewMethod = gameViewType.GetMethod("OnFocus",
        //                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance);
        //GameViewMethod.Invoke(gameViewObj, null);

        System.Reflection.MethodInfo GameViewMethod = gameViewType.GetMethod("GetMainGameView",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance);
        object mainGameView = GameViewMethod.Invoke(null, null);

        if (mainGameView == null)
        {
            mainGameView = EditorWindow.GetWindow(gameViewType, false);
        }

        GameViewMethod = gameViewType.GetMethod("OnFocus",
                       System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance);
        GameViewMethod.Invoke(mainGameView, null);

        //UnityEditor.EditorWindow.FocusWindowIfItsOpen(gameViewType);
        gameViewWindow = UnityEditor.EditorWindow.focusedWindow;
    }

    /// <summary>
    /// 菜单勾选状态
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="checkStatus"></param>
    public static void ToggleCheckStatus(string itemName, bool checkStatus)
    {
        Menu.SetChecked(itemName,checkStatus);
    }

    public static bool GetCheckStatus(string itemName,bool checkStatus)
    {
        return Menu.GetChecked(itemName);
    }

    public static void DoFocusGame()
    {
        if (gameViewWindow == null)
        {
            FocusGameViewWindow();
        }
        gameViewWindow.maximized = !gameViewWindow.maximized;
        gameViewWindow.Show();
    }

    public static void PauseGame()
    {
        if (!EditorApplication.ExecuteMenuItem("Edit/Pause"))
        {
            Debug.LogError("Execute Fail");
        }
    }

    public static void PlayGame()
    {
        if (!EditorApplication.ExecuteMenuItem("Edit/Play"))
        {
            Debug.LogError("Execute Fail");
        }
    }

    public static void MoveSceneCameraFocus(GameObject gameObject)
    {
        if (gameObject)
        {
            EditorGUIUtility.PingObject(gameObject);
            MoveSceneCameraFocus(gameObject.transform.position);
        }       
    }

    public static void MoveSceneCameraFocus(Vector3 position)
    {
        SceneView lastScenView = SceneView.lastActiveSceneView;
        lastScenView.rotation = Quaternion.Euler(90, 0, 0);//视觉上 向前推
        lastScenView.pivot = position;
        lastScenView.size = 5f;
        lastScenView.orthographic = true;
        lastScenView.Repaint();
    }

    public static void MoveSceneCamera2DView(float zDistance)
    {
        if (zDistance < 0f) zDistance = -zDistance;
        else if(zDistance==0f)  zDistance  = -5f;
        SceneView lastScenView = SceneView.lastActiveSceneView;
        lastScenView.in2DMode = true;
        lastScenView.LookAt(Vector3.back * zDistance);
        lastScenView.Repaint();
    }

    /// <summary>
    /// 创建泛型菜单
    /// </summary>
    /// <param name="menuItems"></param>

    public class GenericMenuInfo
    {
        public string itemName;
        public bool itemChecked;
        public GenericMenu.MenuFunction menuCallback;
        public GenericMenu.MenuFunction2 menuCallback2;
        public object userdata;
    }

    public static void DisplayGenericMenu(string[] menuItems,GenericMenu.MenuFunction[] callbacks)
    {
        Debug.Assert(menuItems.Length == callbacks.Length);
        GenericMenu menu = new GenericMenu();
        for (int i = 0; i < menuItems.Length; i++)
        {
            menu.AddItem(new GUIContent(menuItems[i]), false, callbacks[i]);
        }
        menu.ShowAsContext();
    }

    public static void DisplayGenericMenu(GenericMenuInfo[] menuItems)
    {
        GenericMenu menu = new GenericMenu();
        for (int i = 0; i < menuItems.Length; i++)
        {
            if (string.IsNullOrEmpty(menuItems[i].itemName))
            {
                menu.AddSeparator("");//空百分割符
            }
            else {
                if (menuItems[i].menuCallback != null)
                {
                    menu.AddItem(new GUIContent(menuItems[i].itemName), menuItems[i].itemChecked, menuItems[i].menuCallback);
                }
                else
                {
                    menu.AddItem(new GUIContent(menuItems[i].itemName), menuItems[i].itemChecked, menuItems[i].menuCallback2, menuItems[i].userdata);
                }   
            }        
        }
        menu.ShowAsContext();
    }

    /// <summary>
    /// 进度条
    /// </summary>
    /// <param name="title"></param>
    /// <param name="info"></param>
    /// <param name="progress"></param>
    public static void DisplayCancelableProgressBar(string title, string info, float progress)
    {
        if (progress >= 0f && progress < 1f)
        {
            if (EditorUtility.DisplayCancelableProgressBar(title, info, progress)) {
                if (DisplayDialog("警告", "正在进行中", "确认终止","取消"))
                {
                    ClearProgressBar();
                }
            }
        }
        else {
            ClearProgressBar();
        }
    }

    public static void DisplayProgressBar(string title, string info, float progress)
    {
        if (progress >= 0f && progress < 1f)
        {
            EditorUtility.DisplayProgressBar(title, info, progress);
        }
        else
        {
            ClearProgressBar();
        }
    }

    public static void ClearProgressBar()
    {
        EditorUtility.ClearProgressBar();
    }
    
    /// <summary>
    /// 提示框
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="sure"></param>
    /// <param name="cancel"></param>
    /// <returns></returns>
    public static bool DisplayDialog(string title, string message, string sure, string cancel = "Cancel")
    {
        return EditorUtility.DisplayDialog(title, message, sure, cancel);
    }

    public static int DisplayDialogComplex(string title, string message, string sure, string cancel, string alt)
    {
        return EditorUtility.DisplayDialogComplex(title, message, sure, cancel, alt);
    }

    public static string[] FindAssetGUIDs<T>(params string[] args) where T : UnityEngine.Object
    {
        string typename = null;
        if (typeof(T).IsSubclassOf(typeof(Texture)))
        {
            typename = "Texture";
        }
        else typename = typeof(T).Name;
        return FindAssetGUIDs(typename, args);
    }

    //DefaultAsset MonoScript SceneAsset ScriptableObject
    public static string[] FindAssetGUIDs(string filters, string[] args)
    {
        return AssetDatabase.FindAssets("t:" + filters, args);
    }

    public static T[] FindAssets<T>(params string[] args) where T : UnityEngine.Object
    {
        string typename = null;
        if (typeof(T).IsSubclassOf(typeof(Texture)))
        {
            typename = "Texture";
        }
        else typename = typeof(T).Name;
        Object[] _allAssets = FindAssets(typename, args);
        T[] allAssets = new T[_allAssets.Length];
        for (int index = 0; index < _allAssets.Length; index++)
        {
            allAssets[index] = _allAssets[index] as T;
        }
        return allAssets;
    }

    public static Object[] FindAssets(string filters,string[] args)
    {
        string[] findGuids = FindAssetGUIDs("t:" + filters, args);
        Object[] allAssets = new Object[findGuids.Length];
        for (int index = 0; index < findGuids.Length; index++)
        {
            allAssets[index] = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(findGuids[index]));
        }
        return allAssets;
    }

    /// <summary>
    /// 定位资源
    /// </summary>
    /// <param name="assetpath"></param>
    public static void RelocateResProjectPath(string assetpath)
    {
        AssetImporter ai = AssetImporter.GetAtPath(assetpath);
        if (ai) RelocateResProjectPath(ai.GetInstanceID());
    }

    public static void RelocateResProjectPath(Object curasset)
    {
        if (curasset) RelocateResProjectPath(curasset.GetInstanceID());
    }

    public static void RelocateResProjectPath(int instanceId)
    {
        Selection.activeInstanceID = instanceId;
        //EditorUtility.FocusProjectWindow();
        System.Type projectType = System.Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
        //System.Reflection.MethodInfo projectViewMethod = projectType.GetMethod("ShowSelectedObjectsInLastInteractedProjectBrowser",
        //System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance);
        //projectViewMethod.Invoke(null, null);

        System.Reflection.MethodInfo projectViewMethod = projectType.GetMethod("FrameObjectPrivate",
             System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance);
        System.Reflection.FieldInfo projectViewField = projectType.GetField("s_LastInteractedProjectBrowser", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        object projectViewObj = projectViewField.GetValue(null);
        projectViewMethod.Invoke(projectViewObj, new object[] { Selection.activeInstanceID, true, false });
    }

    /// <summary>
    /// Create an undo point for the specified objects.
    /// </summary>

    public static void RegisterUndo(string name, params Object[] objects)
    {
        if (objects != null && objects.Length > 0)
        {
            UnityEditor.Undo.RecordObjects(objects, name);

            foreach (Object obj in objects)
            {
                if (obj == null) continue;
                EditorUtility.SetDirty(obj);
            }
        }
    }
    
    //代理方法判定静态实例 区分静态实例，函数与函数指针
    //EditorUtility.DisplayPopupMenu(new Rect(mousePosition.x, mousePosition.y, 0, 0), "ToolBar/",null);
    public static void ToggleListenToolBars(MethodInfo method)
    { 
        System.Type toosType = typeof(Tools);
        FieldInfo toolfield = toosType.GetField("onToolChanged", BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
        System.Delegate toolDelegate = (System.Delegate)toolfield.GetValue(null);
        Debug.Log("fields:" + toolfield.FieldType);
        System.Delegate mDelegate = System.Delegate.CreateDelegate(toolfield.FieldType, method);
        if (toolDelegate != null && System.Array.IndexOf(toolDelegate.GetInvocationList(), mDelegate) >= 0) 
            toolfield.SetValue(null, System.Delegate.Remove(toolDelegate, mDelegate));
        else 
            toolfield.SetValue(null, System.Delegate.Combine(toolDelegate, mDelegate));

    }

    public static void HideToolBars()
    {
        Tools.current = Tool.None;
        Tools.viewTool = ViewTool.None;
        Tools.hidden = true;
        ToolsRefresh();
    }

    public static void StartToolBars()
    {
        Tools.current = Tool.View;
        Tools.viewTool = ViewTool.Pan;
        Tools.hidden = false;
        /*Tools.pivotMode = PivotMode.Center;
        Tools.pivotRotation = PivotRotation.Global;*/
        ToolsRefresh();
    }

    public static void ToolsRefresh()
    {
        MethodInfo info = typeof(Tools).GetMethod("RepaintAllToolViews", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
        info.Invoke(null, null);
    }

    /// <summary>
    /// Gets the internal class ID of the specified type.
    /// </summary>

    public static int GetClassID(System.Type type)
    {
        GameObject go = EditorUtility.CreateGameObjectWithHideFlags("Temp", HideFlags.HideAndDontSave);
        Component script = go.AddComponent(type);
        SerializedObject ob = new SerializedObject(script);
        int classID = ob.FindProperty("m_Script").objectReferenceInstanceIDValue;
        Debug.Log("classID:" + classID);
        TDestroy(go);
        return classID;
    }

    public static void TDestroy(Object assetObject)
    {
        if (assetObject is GameObject)
        {
            //任意资源（获取资源实体Prefab）父源（获取父源对象GameObject） 完整实例PreafbInstance
            //PreafbInstance 都非空
            //Preafb 有资源但无父源  MissingPrefabInstance
            //DisconnectedPrefabInstance 资源为空父源非空
            //MissingPrefabInstance 父源为空资源源非空 Preafb
            if (!PrefabUtility.GetPrefabParent(assetObject) || PrefabUtility.GetPrefabType(assetObject) == PrefabType.MissingPrefabInstance)
            {
                Object.DestroyImmediate(assetObject);
            }
        }
        assetObject = null;
    }

    /// <summary>
    /// Gets the internal class ID of the specified type. 唯一类ID
    /// </summary>

    public static int GetClassID<T>() where T : MonoBehaviour { return GetClassID(typeof(T)); }

    /// <summary>
    /// Convenience function that replaces the specified MonoBehaviour with one of specified type.
    /// </summary>

    public static SerializedObject ReplaceClass(MonoBehaviour mb, System.Type type)
    {
        int id = GetClassID(type);
        SerializedObject ob = new SerializedObject(mb);
        ob.Update();
        ob.FindProperty("m_Script").objectReferenceInstanceIDValue = id;
        ob.ApplyModifiedProperties();
        ob.Update();
        return ob;
    }

    /// <summary>
    /// Convenience function that replaces the specified MonoBehaviour with one of specified class ID.
    /// </summary>

    public static SerializedObject ReplaceClass(MonoBehaviour mb, int classID)
    {
        SerializedObject ob = new SerializedObject(mb);
        ob.Update();
        ob.FindProperty("m_Script").objectReferenceInstanceIDValue = classID;
        ob.ApplyModifiedProperties();
        ob.Update();
        return ob;
    }

    /// <summary>
    /// Convenience function that replaces the specified MonoBehaviour with one of specified class ID.
    /// </summary>

    public static void ReplaceClass(SerializedObject ob, int classID)
    {
        ob.FindProperty("m_Script").objectReferenceInstanceIDValue = classID;
        ob.ApplyModifiedProperties();
        ob.Update();
    }

    /// <summary>
    /// Convenience function that replaces the specified MonoBehaviour with one of specified class ID.
    /// </summary>

    public static void ReplaceClass(SerializedObject ob, System.Type type)
    {
        ReplaceClass(ob, GetClassID(type));
    }

    /// <summary>
    /// Convenience function that replaces the specified MonoBehaviour with one of specified type.
    /// </summary>

    public static T ReplaceClass<T>(MonoBehaviour mb) where T : MonoBehaviour { return ReplaceClass(mb, typeof(T)).targetObject as T; }

    /// <summary>
    /// Load the asset at the specified path.
    /// </summary>

    public static Object LoadAsset(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        return AssetDatabase.LoadMainAssetAtPath(path);
    }

    /// <summary>
    /// Convenience function to load an asset of specified type, given the full path to it.
    /// </summary>

    public static T LoadAsset<T>(string path) where T : Object
    {
        if (string.IsNullOrEmpty(path)) return null;
        return AssetDatabase.LoadAssetAtPath<T>(path);
        /*Object obj = LoadAsset(path);
        if (obj == null) return null;

        T val = obj as T;
        if (val != null) return val;

        if (typeof(T).IsSubclassOf(typeof(Component)))
        {
            if (obj.GetType() == typeof(GameObject))
            {
                GameObject go = obj as GameObject;
                return go.GetComponent(typeof(T)) as T;
            }
        }
        return null;*/
    }

    /// <summary>
    /// Get the specified object's GUID.
    /// </summary>

    public static string ObjectToGUID(Object obj)
    {
        string path = AssetDatabase.GetAssetPath(obj);
        return (!string.IsNullOrEmpty(path)) ? AssetDatabase.AssetPathToGUID(path) : null;
    }

    static MethodInfo s_GetInstanceIDFromGUID;

    /// <summary>
    /// Convert the specified GUID to the InstanceId of an object reference.
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public static int GUIDToInstanceId(string guid)
    {
        if (string.IsNullOrEmpty(guid)) return -1;

        if (s_GetInstanceIDFromGUID == null)
            s_GetInstanceIDFromGUID = typeof(AssetDatabase).GetMethod("GetInstanceIDFromGUID", BindingFlags.Static | BindingFlags.NonPublic);
        return (int)s_GetInstanceIDFromGUID.Invoke(null, new object[] { guid });
    }

    /// <summary>
    /// Convert the specified GUID to an object reference.
    /// </summary>

    public static Object GUIDToObject(string guid)
    {
        int id = GUIDToInstanceId(guid);
        if (id != 0) return EditorUtility.InstanceIDToObject(id);
        string path = AssetDatabase.GUIDToAssetPath(guid);
        if (string.IsNullOrEmpty(path)) return null;
        return AssetDatabase.LoadAssetAtPath(path, typeof(Object));
    }

    /// <summary>
    /// Convert the specified GUID to an object reference of specified type.
    /// </summary>

    public static T GUIDToObject<T>(string guid) where T : Object
    {
        Object obj = GUIDToObject(guid);
        if (obj == null) return null;

        System.Type objType = obj.GetType();
        if (objType == typeof(T) || objType.IsSubclassOf(typeof(T))) return obj as T;

        if (objType == typeof(GameObject) && typeof(T).IsSubclassOf(typeof(Component)))
        {
            GameObject go = obj as GameObject;
            return go.GetComponent(typeof(T)) as T;
        }
        return null;
    }

    public static string GetHierarchy(GameObject obj)
    {
        if (obj == null) return "";
        string path = obj.name;

        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = obj.name + "/" + path;
        }
        return path;
    }


#if UNITY_EDITOR
    static int mSizeFrame = -1;
    static System.Reflection.MethodInfo s_GetSizeOfMainGameView;
    static Vector2 mGameSize = Vector2.one;

    /// <summary>
    /// Size of the game view cannot be retrieved from Screen.width and Screen.height when the game view is hidden.
    /// </summary>

    static public Vector2 screenSize
    {
        get
        {
            int frame = Time.frameCount;

            if (mSizeFrame != frame || !Application.isPlaying)
            {
                mSizeFrame = frame;

                if (s_GetSizeOfMainGameView == null)
                {
                    System.Type type = System.Type.GetType("UnityEditor.GameView,UnityEditor");
                    s_GetSizeOfMainGameView = type.GetMethod("GetSizeOfMainGameView",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                }
                mGameSize = (Vector2)s_GetSizeOfMainGameView.Invoke(null, null);
            }
            return mGameSize;
        }
    }
#else
	/// <summary>
	/// Size of the game view cannot be retrieved from Screen.width and Screen.height when the game view is hidden.
	/// </summary>

	static public Vector2 screenSize { get { return new Vector2(Screen.width, Screen.height); } }
#endif

    /// <summary>
    /// Convenience function that converts Class + Function combo into Class.Function representation.
    /// </summary>
    public static string GetFuncName(object obj, string method)
    {
        if (obj == null) return "<null>";
        string type = obj.GetType().ToString();
        int period = type.LastIndexOf('/');
        if (period > 0) type = type.Substring(period + 1);
        return string.IsNullOrEmpty(method) ? type : type + "/" + method;
    }

    public static void ToggleSearchUpdateCallBack(EditorApplication.CallbackFunction callback) {
        if (EditorApplication.searchChanged != null && System.Array.IndexOf(EditorApplication.searchChanged.GetInvocationList(), callback) >= 0)
        {
            EditorApplication.searchChanged = (EditorApplication.CallbackFunction)System.Delegate.Remove(EditorApplication.searchChanged, callback);
        }
        else {
            EditorApplication.searchChanged = (EditorApplication.CallbackFunction)System.Delegate.Combine(EditorApplication.searchChanged, callback);
        }
    }

    public static void ToggleModifierKeysCallBack(EditorApplication.CallbackFunction callback)
    {
        if (EditorApplication.modifierKeysChanged != null && System.Array.IndexOf(EditorApplication.modifierKeysChanged.GetInvocationList(), callback) >= 0)
        {
            EditorApplication.modifierKeysChanged = (EditorApplication.CallbackFunction)System.Delegate.Remove(EditorApplication.modifierKeysChanged, callback);
        }
        else
        {
            EditorApplication.modifierKeysChanged = (EditorApplication.CallbackFunction)System.Delegate.Combine(EditorApplication.modifierKeysChanged, callback);
        }
    }

    public static void ToggleProjectWindowItemOnGUI() {
        ToggleProjectWindowItemOnGUI(ProjectWindowItemOnGUICallBack);
    }
    public static void ToggleProjectWindowItemOnGUI(EditorApplication.ProjectWindowItemCallback callback)
    {
        if (EditorApplication.projectWindowItemOnGUI != null && System.Array.IndexOf(EditorApplication.projectWindowItemOnGUI.GetInvocationList(), callback) >= 0)
        {
            EditorApplication.projectWindowItemOnGUI = (EditorApplication.ProjectWindowItemCallback)System.Delegate.Remove(EditorApplication.projectWindowItemOnGUI, callback);
        }
        else
        {
            EditorApplication.projectWindowItemOnGUI = (EditorApplication.ProjectWindowItemCallback)System.Delegate.Combine(EditorApplication.projectWindowItemOnGUI, callback);
        }
    }
    protected static void ProjectWindowItemOnGUICallBack(string guid, Rect selectionRect)
    {
        Event curEvent = Event.current;
        if (selectionRect.Contains(curEvent.mousePosition))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (!string.IsNullOrEmpty(path))
            {
                AssetImporter ai = AssetImporter.GetAtPath(path);
                if (ai != null)
                {
                    DisplayGenericMenu(new string[]{"1","3","2/4"},new GenericMenu.MenuFunction[3]);
                }
            }    
        }         
    }

    public static void AddCurseRect(Rect rect) {
        EditorGUIUtility.AddCursorRect(rect,MouseCursor.Link);
    }

}
