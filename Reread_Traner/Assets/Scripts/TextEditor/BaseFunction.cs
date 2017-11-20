using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//デバック
using UnityEngine.UI;

public class BaseFunction : MonoBehaviour {
	float width; //実機の横幅
	float height; //実機の縦幅

	public InputField input_field; //テキストエリア

	private int char_num; //改行文字数

	private float font_size; //フォントサイズ

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
		set_type(); //編集領域の詳細な設定
		set_size(width, height); //editor領域の指定
	}

	//editor領域のサイズを指定する関数
	void set_size(float w, float h){
		//引数で渡された画面サイズをもとにeditor領域のサイズを設定
		float window_w = (w * 2.0F) / 3.0F;
		float window_h = h;
		char_num = (int)(window_w / font_size) - 2; //一行あたりの文字数を計算
		//Debug.Log(char_num);

		//設定したeditor領域のサイズに変更
		Vector2 window_size = new Vector2(window_w, window_h);
		input_field.GetComponent<RectTransform>().sizeDelta = window_size;
	}

	//editorの設定を定義
	void set_type(){
		input_field.GetComponent<InputField>().lineType = InputField.LineType.MultiLineNewline; //改行あり
		input_field.GetComponent<InputField>().contentType = InputField.ContentType.Standard; //全ての文字入力可
		input_field.GetComponent<InputField>().transition = InputField.Transition.None; //インラタクティブなし
		font_size = (float)input_field.transform.Find("Text").GetComponent<Text>().fontSize; //フォントサイズを取得
		Debug.Log(font_size);
	}

	//テキストの変更を検知
	public void OnValueChanged(){
		Debug.Log("OnValue：" + input_field.text);
		char[] delimiterChars = { '\n'}; // Splitの対象文字列
		string[] text = input_field.text.Split(delimiterChars); //改行位置をスプリットして配列を生成
		int caret = input_field.caretPosition; //編集位置までに入力されている文字数
		Debug.Log(calcu_edit_pos(caret, text));
	}

	//テキストの確定を検知
	public void EndEdit(){
		Debug.Log("EndEdit：" + input_field.text);
	}

	//編集位置の座標を推定する関数（handled：直前の文字列情報, recent：最新の文字列情報）
	Vector2 calcu_edit_pos(int caret, string[] text){
		Vector2 result = new Vector2(0.0f, 0.0f);
		int indent = 0; //編集位置までの改行回数をカウントする
		int num = 0; //編集位置までに費やす文字数
		int temp_row = 0; //文字列が要する行数を一時的に格納する変数
		Debug.Log(caret);
		Debug.Log(text.Length);
		for(int i=0; i<text.Length; i++){
			num = text[i].Length;
			if( num < caret ){
				//numがcaretより小さい場合
				Debug.Log("ここって通ってる？");
				caret = caret - ( num + 1 ); //caretを文字数分引く
				temp_row = Mathf.CeilToInt( num / char_num ); //要した行数を計算
				indent = indent + temp_row;
			}else{
				if( caret > char_num ){
					//caretが一行あたりの文字数をオーバーした場合
					indent = indent + caret / char_num; //行数を計算
					caret = caret % char_num; //横からの文字数を計算
					break;
				}else{
					//caretが一行あたりの文字数をオーバーしなかった場合
					break;
				}
			}
			indent += 1;
		}
		result = new Vector2(caret * font_size, indent * font_size);
		return result;
	}
}

