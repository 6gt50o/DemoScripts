using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class EditorToolsExtension {

    public static Vector3 SceneScreenToWorldPoint(this SceneView sceneView, Vector3 sceneScreenPoint)
    {
        Camera sceneCamera = sceneView.camera;
        float screenHeight = sceneCamera.orthographicSize * 2f;
        float screenWidth = screenHeight * sceneCamera.aspect;

        //单位像素世界 相对中心点  GUI(左上角)--World
        Vector3 worldPos = new Vector3(
            (sceneScreenPoint.x / sceneCamera.pixelWidth) * screenWidth - screenWidth * 0.5f,
            ((-(sceneScreenPoint.y) / sceneCamera.pixelHeight) * screenHeight + screenHeight * 0.5f),
            0f);

        worldPos += sceneCamera.transform.position;
        worldPos.z = 0f;

        return worldPos;
    }
    /// <summary>
    /// 是否可选参数 ParameterAttributes Ref /Out 
    /// ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Instance | binding);
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public static bool IsSelectedParams(this ParameterInfo param)
    {
        //return param.Attributes == ParameterAttributes.HasDefault || param.Attributes == ParameterAttributes.Optional;
        return param.GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0;
    }

    /// <summary>
    /// 是否弃用属性
    /// </summary>
    /// <param name="mb"></param>
    /// <returns></returns>
    public static bool IsObsolete(Type mb)//MemberInfo Type
    {
        object[] attrs = mb.GetCustomAttributes(true);

        for (int j = 0; j < attrs.Length; j++)
        {
            Type t = attrs[j].GetType();

            if (t == typeof(System.ObsoleteAttribute)) // || t.ToString() == "UnityEngine.WrapperlessIcall")
            {
                return true;
            }
        }
        return false;
    }
}
