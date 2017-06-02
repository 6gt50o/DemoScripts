using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
public partial class CustomEditorTools {

    public static EditorApplication.CallbackFunction updateCall = null;

    protected static Dictionary<string, WaitTime> waitTables = new Dictionary<string, WaitTime>();
    protected static List<string> stopWaits = new List<string>();
    protected class WaitTime {
        public float time;
        public float delay;
        public bool delayfinish;
        public double starttime;
        public double curtime;
        public sbyte status;//-1(stop) 0(yield) 1 resume 2 restart
        public System.Action callback;
    }

    protected enum WaitStaus { 
        Stop = -1,
        Yield,//主动暂停
        Resume,
        Step,//判定到时间 或是 
        Restart,
    }

    public static void WaitToExcute(string name, float stime, float sdelay = 0f)
    {
        if (updateCall == null) {
            updateCall = new EditorApplication.CallbackFunction(WaitTimeCallBack);
            EditorApplication.update += updateCall;
        }

        if (!waitTables.ContainsKey(name))
        {
            waitTables[name] = new WaitTime() { time = stime, delay = sdelay, delayfinish = false, starttime = EditorApplication.timeSinceStartup, curtime = EditorApplication.timeSinceStartup, status = (sbyte)WaitStaus.Restart };
            if (waitTables[name].delay <= 0f) waitTables[name].delayfinish = true; else waitTables[name].delayfinish = false;
        }
    }

    public static void ClearTimeManager() {
        if (updateCall != null)
        {
            EditorApplication.update -= updateCall;
            updateCall = null;
        }
        waitTables.Clear();
        stopWaits.Clear();
    }

    public static void RegisterCallBack(string name,System.Action callback)
    {
        if (waitTables.ContainsKey(name))
        {
            waitTables[name].callback = callback;
        }
    }

    public static void StopWait(string name)
    {
        if (waitTables.ContainsKey(name))
        {
            waitTables[name].status = (sbyte)WaitStaus.Stop;
        }
    }

    public static void YieldWait(string name)
    {
        if (waitTables.ContainsKey(name))
        {
            waitTables[name].status = (sbyte)WaitStaus.Yield;
        }
    }

    public static void ResumeWait(string name)
    {
        if (waitTables.ContainsKey(name))
        {
            waitTables[name].status = (sbyte)WaitStaus.Resume;
        }
    }

    public static void RestartWait(string name)
    {
        if (waitTables.ContainsKey(name))
        {
            waitTables[name].status = (sbyte)WaitStaus.Restart;
        }
    }

    public static bool StepToDoAction(string name)
    {
        if (waitTables.ContainsKey(name))
        {
           return waitTables[name].status == (sbyte)WaitStaus.Step;
        }
        return false;
    }
    //单位秒数
    public static void WaitTimeCallBack() 
    {
        foreach (KeyValuePair<string, WaitTime> wait in waitTables)
        {
            if (wait.Value.status == (sbyte)WaitStaus.Restart)
            {
                wait.Value.curtime = wait.Value.starttime = EditorApplication.timeSinceStartup;
                wait.Value.status = (sbyte)WaitStaus.Resume;
                if (wait.Value.delay <= 0f) wait.Value.delayfinish = true;
                else wait.Value.delayfinish = false;
            }
            else if (wait.Value.status == (sbyte)WaitStaus.Stop)
            {
                wait.Value.callback = null;
                stopWaits.Add(wait.Key);
            }
            else if (wait.Value.status == (sbyte)WaitStaus.Resume)
            {
                wait.Value.curtime = EditorApplication.timeSinceStartup;
                if (!wait.Value.delayfinish)
                {
                    if (wait.Value.curtime - wait.Value.starttime >= wait.Value.delay)
                    {
                        //Debug.Log("TimeDelay:"+wait.Value.curtime + "_" + wait.Value.starttime);
                        
                        //相对大小可尽快执行
                        //wait.Value.starttime = wait.Value.curtime;
                        wait.Value.delayfinish = true;
                    }
                }
                else {
                    if (wait.Value.curtime - wait.Value.starttime >= wait.Value.time)
                    {
                        //Debug.Log("TimeUpdate:" + wait.Value.curtime + "_" + wait.Value.starttime);
                        wait.Value.starttime = wait.Value.curtime;
                        wait.Value.status = (sbyte)WaitStaus.Step;
                        if (wait.Value.callback != null)
                        { 
                            wait.Value.callback();
                            wait.Value.status = (sbyte)WaitStaus.Resume;
                        }
                    }
                }
            }
        }

        foreach (string keydel in stopWaits)
        {
            waitTables.Remove(keydel);
        }
        stopWaits.Clear();
    }
}
