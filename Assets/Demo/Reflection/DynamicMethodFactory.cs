using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

public delegate void SetValueDelegate(object target, object arg);
public delegate string FuncDelegate(int msg);

public static class DynamicMethodFactory
{
    //伪汇编 即时缓存局部变量
    public static SetValueDelegate CreatePropertySetter(PropertyInfo property)
    {
        if (property == null)
            throw new ArgumentNullException("property");

        if (!property.CanWrite)
            return null;

        MethodInfo setMethod = property.GetSetMethod(true);//即使不公开也需要获取

        DynamicMethod dm = new DynamicMethod("PropertySetter", null,
            new Type[] { typeof(object), typeof(object) }, property.DeclaringType, true);

        ILGenerator il = dm.GetILGenerator();

        if (!setMethod.IsStatic)
        {
            il.Emit(OpCodes.Ldarg_0);
        }
        il.Emit(OpCodes.Ldarg_1);

        EmitCastToReference(il, property.PropertyType);
        if (!setMethod.IsStatic && !property.DeclaringType.IsValueType)
        {
            il.EmitCall(OpCodes.Callvirt, setMethod, null);
        }
        else
            il.EmitCall(OpCodes.Call, setMethod, null);

        il.Emit(OpCodes.Ret);
        return (SetValueDelegate)dm.CreateDelegate(typeof(SetValueDelegate));
    }

    public static FuncDelegate CreateMethodCaller(MethodInfo method)
    {
        if (method == null)
            throw new ArgumentNullException("method");
        ParameterInfo[] allParameterInfos = method.GetParameters();
        Type[] allParaTypes = new Type[allParameterInfos.Length];
        int id =0;
        foreach (ParameterInfo para in allParameterInfos)
        {
            allParaTypes[id] = para.ParameterType;
            id++;
        }
        //DynamicMethod dm = new DynamicMethod("MethodCaller", method.ReturnType,
        //    allParaTypes, method.DeclaringType, true);
        DynamicMethod dm = new DynamicMethod("MethodCaller", typeof(string),
    new Type[]{typeof(int)}, method.DeclaringType, true);
        ILGenerator il = dm.GetILGenerator();

        if (!method.IsStatic)
        {
            il.Emit(OpCodes.Ldarg_0);
        }
        il.Emit(OpCodes.Ldarg_1);

        EmitCastToReference(il, method.ReturnType);

        if (!method.IsStatic && !method.DeclaringType.IsValueType)
        {
            il.EmitCall(OpCodes.Callvirt, method, null);
        }
        else
            il.EmitCall(OpCodes.Call, method, null);

        il.Emit(OpCodes.Ret);
        return (FuncDelegate)dm.CreateDelegate(typeof(FuncDelegate));
    }

    public static string ExpressionTree<T>(string propertyName, int propertyValue) where T : new()
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        ParameterExpression value = Expression.Parameter(typeof(int), "y");

        MethodInfo setter = typeof(T).GetMethod(propertyName,BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

        MethodCallExpression call = Expression.Call(parameter,setter, value);

        var lambda = Expression.Lambda<Func<T,int, string>>(call, parameter,value);
        var exp = lambda.Compile();
        return exp(new T(),propertyValue);
    }
    //静态
    public static string ExpressionTree2<T>(string propertyName, int propertyValue) where T : new()
    {
        ParameterExpression value = Expression.Parameter(typeof(int), "x");

        MethodCallExpression call = Expression.Call(typeof(T), propertyName, null,value);

        var lambda = Expression.Lambda<Func<int, string>>(call,value);
        var exp = lambda.Compile();
        return exp(propertyValue);
    }

    public static string ExpressionTree3<T>(FuncDelegate dele, string propertyName, int propertyValue) where T : new()
    {
        ParameterExpression parameter = Expression.Parameter(typeof(FuncDelegate), "x");
        ParameterExpression value = Expression.Parameter(typeof(int), "y");
        MethodInfo setter = typeof(T).GetMethod(propertyName, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

        MethodCallExpression call = Expression.Call(parameter, setter, value);

        var lambda = Expression.Lambda<Func<FuncDelegate, int, string>>(call, parameter, value);
        var exp = lambda.Compile();
        return exp(dele,propertyValue);
    }

    private static void EmitCastToReference(ILGenerator il, Type type)
    {
        if (type.IsValueType)
            il.Emit(OpCodes.Unbox_Any, type);
        else
            il.Emit(OpCodes.Castclass, type);
    }
}