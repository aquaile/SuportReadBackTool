using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//デバック
using UnityEngine.UI;

public class BaseFunction : MonoBehaviour {
	float width; //実機の横幅
	float height; //実機の縦幅

	public InputField input_field; //テキストエリア

	// Use this for initialization
	void Start () {
		init();
	}
	
	// Update is called once per frame
	void Update () {
	}

	//初期設定
	void init(){
		//値の挿入
		width = (float)Screen.currentResolution.width;
		height = (float)Screen.currentResolution.height;
		set_size(width, height); //editor領域の指定
	}

	//editor領域のサイズを指定する関数
	void set_size(float w, float h){
		//引数で渡された画面サイズをもとにeditor領域のサイズを設定
		float window_w = (w * 2.0F) / 3.0F;
		float window_h = h;

		//設定したeditor領域のサイズに変更
		Vector2 window_size = new Vector2(window_w, window_h);
		input_field.GetComponent<RectTransform>().sizeDelta = window_size;
	}

	void learn_input_field(){
		input_field.asteriskChar = "$!£%&*"[0];
	}
}
