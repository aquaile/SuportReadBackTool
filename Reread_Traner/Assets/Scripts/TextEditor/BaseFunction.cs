using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//デバック
using UnityEngine.UI;

public class BaseFunction : MonoBehaviour {
	float width; //実機の横幅
	float height; //実機の縦幅

	public InputField input_field; //テキストエリア

	private TextInfo handled; //編集前のテキストを保持

	private int char_num; //改行文字数

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
		char_num = (int)(window_w / 14.0F) - 2; //一行あたりの文字数を計算
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
	}

	//テキストの変更を検知
	public void OnValueChanged(){
		Debug.Log("OnValue：" + input_field.text);
		char[] delimiterChars = { '\n'}; // Splitの対象文字列
		string[] text = input_field.text.Split(delimiterChars); //改行位置をスプリットして配列を生成
		int[] indent = new int[text.Length]; //それぞれの文字列が費やす行数を格納
		//文字列ごとの行数の計算
		for(int i=0; i<indent.Length; i++){
			indent[i] = text[i].Length / char_num;
			if(text[i].Length % char_num != 0 || text[i].Length == 0){
				indent[i] += 1;
			}
		}
		handled.text = text;
		handled.indent = indent;
		
	}

	//テキストの確定を検知
	public void EndEdit(){
		Debug.Log("EndEdit：" + input_field.text);
	}

	//編集位置の座標を推定する関数（handled：直前の文字列情報, recent：最新の文字列情報）
	Vector2 caluc_edit_pos(TextInfo handled, TextInfo recent){
		Vector2 result = new Vector2(0.0f, 0.0f); //計算結果
		if(handled.indent.Length >= recent.indent.Length){
			//文字列が削除された場合
			for(int i=0; i<recent.indent.Length; i++){
				if(recent.text[i] != handled.text[i]){
					//文字列が一致しなかった場合

				}
				//文字列の不一致がなかった場合
			}
		}else{
			//文字列が追加された場合
			for(int i=0; i<handled.indent.Length; i++){
				if(recent.text[i] != handled.text[i]){
					//文字列が一致しなかった場合

				}
			}
			//文字列の不一致がなかった場合
		}
		return result;
	}
}


//テキストの情報を保持するクラス
class TextInfo{
	public string[] text; //文字列
	public int[] indent; //行数
}
