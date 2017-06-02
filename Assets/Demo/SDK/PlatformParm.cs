using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformParm : MonoBehaviour {

    public int size;
    public int width;
    private AndroidJavaObject _androidSDK;

    private AndroidJavaObject androidSDK
    {
        get
        {
            if (_androidSDK == null)
            {
                using (AndroidJavaClass _unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    _androidSDK = _unityClass.GetStatic<AndroidJavaObject>("currentActivity");
                }
                //_androidSDK = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            }
            return _androidSDK;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        size = getAndroidKeyBoardSize();
	}

    public int getAndroidKeyBoardSize()
    {
        //int width = 0;
        AndroidJavaObject View = androidSDK.Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");
        AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect");
        View.Call("getWindowVisibleDisplayFrame", Rct);
        int __height = Rct.Call<int>("height");//横竖屏返回的都是键盘的高度   
        width = Rct.Call<int>("width");
        View.Dispose();
        Rct.Dispose();
        return __height;
    }

    void OnGUI()
    {
        GUILayout.Label("size1:"+ size);
        GUILayout.Label("width:" + width);
        GUILayout.Label("W:" + Screen.width);
        GUILayout.Label("H:" + Screen.height);
    }
}
