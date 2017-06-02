#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorOnly : MonoBehaviour
{

    [HideInInspector]
    public string tag = "Untagged";
    void OnDrawGizmos()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag(tag))
        {
            UnityEditor.Handles.Label(go.transform.position, tag);
        }
    }
}

[CustomEditor(typeof(EditorOnly))]
public class EditorOnlyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorOnly gizmos = target as EditorOnly;
        EditorGUI.BeginChangeCheck();
        gizmos.tag = EditorGUILayout.TagField("Tag for Objects:", gizmos.tag);
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(gizmos);
        }
    }
}
#endif