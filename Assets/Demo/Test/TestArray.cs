using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///预编译== 》宏替换 + 删除注释 + 条件编译（预编译指令）.i .s 不可调试，没有额外分配地址，没有参数压栈 转换返回类型 没有类型安全性能损耗（正确类型或能转化）
///汇编 语法检查+ 编译优化
///编译 .o
///汇编编译整合一个阶段 
///==》独立编译安全类型 1.未解析符号表（引用未定义，需要外调其他编译单元的符号以及其出现地址）
///2.已定义符号表 当前编译单元已经定义，提供可被外调的符号及其地址
///3.地址重定向表 对自身编译单元的引用记录
///==》内联函数的替换展开 编译器申请建议 对于直接拷贝源码还是汇编取决于编译器规则，有内联标记不会重复
/// 没有额外分配地址，没有参数压栈 转换返回类型 没有类型安全性能损耗
/// 
///链接符号表
///链接器从左到右依次扫描 所有目标文件 和 库文件
//维护U(unsigned引用未定义的符号)E（当前将要加入可执行文件的所有目标文件）D（已有已加入的目标文件已定义的符号）
//循环执行fixed point,多余的丢弃（库文件）
/// <summary>
/// 换新引用数组，长度变更 应是重新new一份内存，指针变化
/// 值向量Vector,只是比较数值
/// Returns a copy of the vertex positions or assigns a new vertex positions array.
/// public Vector3[] vertices { get; set; }
/// 
/// 
/// 
/// ~取反^异或，unchecked整形溢出
/// 带符号右移，循环溢出取余（循环2<<33 == 2的1，33%32 == 》 2<<32==0或1？编译器不同，有的高位舍弃 有的取余为2的0，-1即为32-1=31循环）
/// (int long)算术 (uint ulong)逻辑位移 左移一致，右移与符号一致，补0还是补1
/// 
/// dll==>公共语言库CLR(IL),中间字节汇编码==>不同平台底层基础库--》解释运行
/// C/C++重新编译（依赖平台基础库）cpu识别机器码
/// il2cpp==>转编译c++ -->vm托管内存，创建线程
/// </summary>
public class TestArray : MonoBehaviour {
    public int[] test1 = new int[]{1,2,3};
    public int[] test2= new int[]{1,2,3};
    public int[] test3 = new int[] { 1, 2, 3 };

    public Vector3 a = Vector3.zero;
    public Vector3 b = Vector3.zero;
    public Vector3 c = Vector3.zero;

    public Vector3[] ass;
    public Vector3[] bss;
    public Vector3[] css;

	// Use this for initialization
	void Start () {
        /*unchecked {
            int avs = (2 << 30) * 4;
            Debug.Log(avs);
        }
        int _avs = unchecked((2 << 30) * 4);*/

        /*checked
        {
            int avs = (2 << 30) * 4;
            Debug.Log(avs);
        }
        int _avs = checked((2 << 30) * 4);*/

        //Debug.Log(_avs);
        long __a = 1;
        Debug.Log((__a << 32));//unchecked
        Debug.Log((((int)__a) << 32));//unchecked
        Debug.Log(1<<34);//2^2
        Debug.Log(1 << -1);//255-224 = 31
        Debug.Log(1 << -2);
        Debug.Log(1 << 30);
        test3 = new int[] { 1, 2, 3 };
        test2 = new int[] { 1, 2, 3 };
        test1 = new int[] { 1, 2, 3 };

        Debug.Log((test1 == test2)+"_" + test1.Equals(test2));
        test2 = test1;
        Debug.Log(test1 == test2);
        test1[1] = 100;
        Debug.Log((test1 == test2) + "_" + test2[1]);
        test1 = new int[4];
        Debug.Log((test1 == test2) + "_" + test2[1]);

        Debug.Log((a==b) + "_" + a.Equals(b));
        a = b;
        b.x = 0f;
        Debug.Log((a == b) + "_" + a.Equals(b) + "_" + a.x);

        ass = new Vector3[] { Vector3.zero, Vector3.one, Vector3.right };
        bss = new Vector3[] { Vector3.zero, Vector3.one, Vector3.right };
        css = new Vector3[] { Vector3.zero, Vector3.one, Vector3.right };
        Debug.Log((ass == bss) + "_" + ass.Equals(bss));
        ass = bss;
        ass[1].x = 100f;
        Debug.Log((ass == bss) + "_" + bss[1].x);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
