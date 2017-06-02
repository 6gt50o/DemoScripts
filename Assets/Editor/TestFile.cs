using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class TestFile : Editor {
    //引用元表 唯一覆盖接口无需转换
    //RawImage UITexture 加载速度快
    //定位九宫36顶点sliced 外边框剔除 + 上下左右拉伸 左右上下拉伸 + 中间定位 tiled平铺 直接填充slice
	[MenuItem("Assets/File")]
	static void _Start () {
        string test = System.IO.File.ReadAllText("Assets/test2.txt");
        test = test.Replace("//", "\r\n");
        System.IO.File.WriteAllText("Assets/test2.txt",test);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
