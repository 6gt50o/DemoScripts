using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
/// <summary>
/// 进制幂指数 进制转换对应
/// 混合声音通道流 暴露参数 pitch（速度）和volume（大小）右键Pitch,点击Expose to script
/// AudioMixer 资源
/// AudioMixerSnapshot 数据 加载默认第一个
/// 
/// 续航空格 仍为同一行 光滑镜面 粗糙漫反射
/// 法线深度偏移 金属 光泽
/// 泛型实例 将泛型定义转成封闭泛型。定义参数（不定参数 不特殊处理 直接传入数组）
/// instanceType = genericTypeDefinition.MakeGenericType(list.ToArray());
/// type.GetGenericArguments()[0]
/// </summary>
public class AudioMixerInfo : MonoBehaviour {

    public AudioMixer mixer;
    public AudioMixerGroup group;
    public AudioMixerSnapshot snap;
	// Use this for initialization
	void Start () {
        mixer = Resources.Load<AudioMixer>("New");
        group = Resources.Load<AudioMixerGroup>("New");
        snap = Resources.Load<AudioMixerSnapshot>("New");
        mixer.SetFloat("x",10f);
        mixer.SetFloat("y", 10f);
        mixer.SetFloat("z", 10f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
