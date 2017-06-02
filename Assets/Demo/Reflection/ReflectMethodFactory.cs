using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public interface ICallFunction
{
    void Call(object target, object val);

    void CallInter(object target, object val);
}
//泛型实例（构造参数）+ 实例方法必须有实例参数 ，静态传null+  动态表达式树
//为实例方法 M1 创建一个表示开放式实例方法的 D1 类型的委托。必须在调用委托时传递实例。
//为静态方法 M2 创建一个表示开放式静态方法的 D2 类型的委托。
//DeclaringType 归属类
//统一集中 基础脚本
public class ReflectMethodFactory {

    public class SetterWrapper<TTarget, TValue, TReturn> : ICallFunction
    {
        private Func<TTarget, TValue, TReturn> _setter;
        private Action<TTarget, TValue> _setter2;

        public delegate TReturn TestFunc(TTarget t, TValue v);
        //private TestFunc _setter;

        public SetterWrapper(MethodInfo methodinfo,int i)
        {
            Debug.Log(typeof(TTarget).Name + typeof(TValue).Name + typeof(TReturn).Name);
            if (methodinfo == null)
                throw new ArgumentNullException("methodinfo");
            if (i == 0)
            {
                _setter = (Func<TTarget, TValue, TReturn>)Delegate.CreateDelegate(typeof(Func<TTarget, TValue, TReturn>), null, methodinfo);
                //_setter = (TestFunc)Delegate.CreateDelegate(typeof(TestFunc),null,methodinfo);
            }
            else {
                _setter = (Func<TTarget, TValue, TReturn>)Delegate.CreateDelegate(typeof(Func<TTarget, TValue, TReturn>), Activator.CreateInstance(typeof(OrderInfo)), methodinfo);
                //_setter = (TestFunc)Delegate.CreateDelegate(typeof(TestFunc),null,methodinfo);
            }
        }

        public SetterWrapper(MethodInfo methodinfo,bool use)
        {
            Debug.Log(typeof(TTarget).Name + typeof(TValue).Name + typeof(TReturn).Name);
            if (methodinfo == null)
                throw new ArgumentNullException("methodinfo");
            _setter2 = (Action<TTarget, TValue>)Delegate.CreateDelegate(typeof(Action<TTarget, TValue>), new OrderInfo(),methodinfo);
            //_setter = (TestFunc)Delegate.CreateDelegate(typeof(TestFunc),null,methodinfo);
        }

        public void Call(object target, object val)
        {
            Debug.Log((TReturn)(_setter((TTarget)target, (TValue)val)));
        }

        public void CallInter(object target, object val)
        {
            _setter2((TTarget)target, (TValue)val);
        }
    }

    public static ICallFunction CreateFunctionWrapper(MethodInfo methodinfo)
    {
        if (methodinfo == null)
            throw new ArgumentNullException("methodinfo");
        Type instanceType = typeof(SetterWrapper<,,>).MakeGenericType(methodinfo.GetParameters()[0].ParameterType, methodinfo.GetParameters()[1].ParameterType,methodinfo.ReturnType);
        return (ICallFunction)Activator.CreateInstance(instanceType, methodinfo,0);//泛型实例 传参
    }

    public static ICallFunction CreateFunctionWrapper3(MethodInfo methodinfo)
    {
        if (methodinfo == null)
            throw new ArgumentNullException("methodinfo");
        Type instanceType = typeof(SetterWrapper<,,>).MakeGenericType(methodinfo.GetParameters()[0].ParameterType, methodinfo.GetParameters()[1].ParameterType, methodinfo.ReturnType);
        return (ICallFunction)Activator.CreateInstance(instanceType, methodinfo,3);//泛型实例 传参
    }

    public static ICallFunction CreateFunctionWrapper2(MethodInfo methodinfo)
    {
        if (methodinfo == null)
            throw new ArgumentNullException("methodinfo");
        Type instanceType = typeof(SetterWrapper<,,>).MakeGenericType(methodinfo.GetParameters()[0].ParameterType, methodinfo.GetParameters()[1].ParameterType,typeof(int));
        return (ICallFunction)Activator.CreateInstance(instanceType, methodinfo,true);//泛型实例 传参
    }

}
