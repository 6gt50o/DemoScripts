using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTween : MonoBehaviour {
    public Image image;
    public Text text;
    public Button btn;
	// Use this for initialization
	void Start () {
		
	}
    //反转打字text.DOText transform.DOLocalMoveX(120f, 4f, false).From(true); //反转相对本地
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp(KeyCode.A))
        {
            image.DOColor(Color.red, 1f).OnComplete(OnColorComplete);
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            image.DOBlendableColor(Color.yellow, 1f).OnComplete(OnColorComplete2);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            text.DOBlendableColor(Color.green, 1f).OnComplete(OnTextComplete2);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            text.DOText("99999999snnnnnnnnnnnnnnnnnnnnndjjds  jjjjjjjjjjjjjjssw",5f,false,ScrambleMode.None);
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            image.transform.DOLocalMoveX(120f, 4f, false).From(true); //反转相对本地
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            image.transform.DOLocalMoveX(120f, 4f, false).From(false);//反转相对世界
        }
	}

    void OnColorComplete()
    {
        Debug.Log("OnComplete");
    }
    void OnColorComplete2()
    {
        Debug.Log("OnComplete2");
    }
    void OnTextComplete2()
    {
        Debug.Log("OnTextComplete2");
    }

}
