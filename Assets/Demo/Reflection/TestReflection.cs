using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using Debug = UnityEngine.Debug;
public class OrderInfo {
    public int OrderID;
    public int OrderID1{get;set;}
    public int OrderID2{get;private set;}
    public static int OrderID3 { get; set; }

    public string testStringRet(int id)
    {
        if (id / 10000 == 1) Debug.Log("Good:" + id);
        return "Liming" + id;
    }

    public static string testStringRet2(int id)
    {
        if (id / 10000 == 1) Debug.Log("Good:" + id);
        return "Liming**" + id;
    }

    public static string testStringRet3(string name,int id)
    {
        Debug.Log(name + "Good:" + id);
        return name + "Liming**" + id;
    }

    public void testStringRet4(string name, int id)
    {
        Debug.Log(name + "**********************Good:" + id);
    }

    public string testStringRet5(string name, int id)
    {
        Debug.Log(name + "$$$$$$$$$$$$$$$$$$$$$Good:" + id);
        return "??????????????";
    }
}

public class TestReflection : MonoBehaviour {

    protected BindingFlags bindFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
	// Use this for initialization
	void Start () {
        Test();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Test() {
        Debug.Log(RuntimeEnvironment.GetSystemVersion());

        int count = 1000000;

        OrderInfo testObj = new OrderInfo();
        //PropertyInfo propInfo = typeof(OrderInfo).GetProperty("OrderID");
        PropertyInfo propInfo = typeof(OrderInfo).GetProperty("OrderID2");

        Debug.Log("直接访问花费时间：       ");
        Stopwatch watch1 = Stopwatch.StartNew();

        for (int i = 0; i < count; i++)
            testObj.OrderID = 123;

        watch1.Stop();
        Debug.Log(watch1.Elapsed.ToString());

        SetValueDelegate setter2 = DynamicMethodFactory.CreatePropertySetter(propInfo);
        Debug.Log("EmitSet花费时间：        ");
        Stopwatch watch2 = Stopwatch.StartNew();

        for (int i = 0; i < count; i++)
            setter2(testObj, 123);

        watch2.Stop();

        Debug.Log(watch1.Elapsed.ToString());

       /* FuncDelegate setter22 = DynamicMethodFactory.CreateMethodCaller(typeof(OrderInfo).GetMethod("testStringRet"));
        Debug.Log("EmitSet花费时间：        ");
        Stopwatch watch22 = Stopwatch.StartNew();

        for (int i = 0; i < count; i++)
            setter22(i);

        watch2.Stop();

        Debug.Log(watch2.Elapsed.ToString());*/
        Debug.Log(DynamicMethodFactory.ExpressionTree<OrderInfo>("testStringRet",123));
        Debug.Log(DynamicMethodFactory.ExpressionTree2<OrderInfo>("testStringRet2", 90));
        ReflectMethodFactory.CreateFunctionWrapper(typeof(OrderInfo).GetMethod("testStringRet3")).Call("liuliming", 100);
        ReflectMethodFactory.CreateFunctionWrapper2(typeof(OrderInfo).GetMethod("testStringRet4")).CallInter("liuliming", 100);
        ReflectMethodFactory.CreateFunctionWrapper3(typeof(OrderInfo).GetMethod("testStringRet5")).Call("liuliming", 100);
        //Debug.Log(DynamicMethodFactory.ExpressionTree3<OrderInfo>(OrderInfo.testStringRet2,"testStringRet2", 90));
        Debug.Log("纯反射花费时间：　       ");
        Stopwatch watch3 = Stopwatch.StartNew();

        for (int i = 0; i < count; i++)
            propInfo.SetValue(testObj, 123, null);

        watch3.Stop();
        Debug.Log(watch3.Elapsed.ToString());

        Debug.Log("-------------------");
        Debug.LogFormat("{0} / {1} = {2}",
            watch3.Elapsed.ToString(),
            watch1.Elapsed.ToString(),
            watch3.Elapsed.TotalMilliseconds / watch1.Elapsed.TotalMilliseconds);

        Debug.LogFormat("{0} / {1} = {2}",
            watch3.Elapsed.ToString(),
            watch2.Elapsed.ToString(),
            watch3.Elapsed.TotalMilliseconds / watch2.Elapsed.TotalMilliseconds);

        Debug.LogFormat("{0} / {1} = {2}",
            watch2.Elapsed.ToString(),
            watch1.Elapsed.ToString(),
            watch2.Elapsed.TotalMilliseconds / watch1.Elapsed.TotalMilliseconds);
    }
}
