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
		//width = (float)Screen.currentResolution.width;
		//height = (float)Screen.currentResolution.height;
		width = (float)Screen.width;
		height = (float)Screen.height;
		set_size(width, height); //editor領域の指定
		set_type(); //編集領域の詳細な設定
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

	//editorの設定を定義
	void set_type(){
		input_field.GetComponent<InputField>().lineType = InputField.LineType.MultiLineNewline; //改行あり
		input_field.GetComponent<InputField>().contentType = InputField.ContentType.Standard; //全ての文字入力可
		input_field.GetComponent<InputField>().transition = InputField.Transition.None; //インラタクティブなし
	}

	//テキストの変更を検知
	public void OnValueChanged(){
		Debug.Log("OnValue：" + input_field.text);
	}

	//テキストの確定を検知
	public void EndEdit(){
		Debug.Log("EndEdit：" + input_field.text);
	}
}
