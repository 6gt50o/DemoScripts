using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
//拖放事件UnityEvent + 集中代理UnityAction 
//位移匹配 KMP算法

/// <summary>
/// 渲染 逻辑 输入
/// 对齐交互帧
/// 客户端渲染数据（位置方向 位置旋转量）未处理输入事件
/// 渲染处理当前帧 逻辑处理当前帧 移动方向 操作事件（渲染【数据处理】逻辑）
/// 全局游戏状态 + 更新物理事件 + 更新AI （状态逻辑）
/// 渲染服务器派发的数据 上一帧数据，提交的当前帧数据
/// </summary>
public class TUnityAction
{
    public UnityAction<BaseEventData> uEvent;
}

public class UIEventTrigger : EventTrigger {

	// Use this for initialization
	void Start () {
        TUnityAction downAction = new TUnityAction() { uEvent = OnButtonDown };
        // 定义UnityAction，在OnButCreateDefenderDown函数中响应按钮抬起事件  
        TUnityAction upAction = new TUnityAction() { uEvent = OnButtonUp };
        // 定义按钮按下事件Entry  
        EventTrigger.Entry down = new EventTrigger.Entry();
        down.eventID = EventTriggerType.PointerDown;
        down.callback.AddListener(downAction.uEvent);
        // 定义按钮抬起事件Entry  
        EventTrigger.Entry up = new EventTrigger.Entry();
        up.eventID = EventTriggerType.PointerUp;
        up.callback.AddListener(upAction.uEvent);

        this.triggers = new List<EventTrigger.Entry>();
        this.triggers.Add(down);
        this.triggers.Add(up);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnButtonDown(BaseEventData data) {
        Debug.Log("down----" + data.selectedObject);
    }

    void OnButtonUp(BaseEventData data)
    {
        Debug.Log("up----" + data.selectedObject);
    }
}
