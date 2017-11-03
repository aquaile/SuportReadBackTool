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
		learn_input_field();
	}
	
	// Update is called once per frame
	void Update () {
	}

	//初期設定
	void init(){
		//値の挿入
		width = (float)Screen.currentResolution.width;
		height = (float)Screen.currentResolution.height;
	}

	void learn_input_field(){
		input_field.asteriskChar = "$!£%&*"[0];
	}
}
