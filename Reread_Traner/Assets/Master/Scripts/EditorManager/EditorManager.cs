/*

文章編集の基本機能と編集位置の計算を行う文章作成スクリプト

Editor：編集位置( x, y ) ->  FunctionManager：第一引数

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorManager : MonoBehaviour {

	//内部変数
	private float width; //実機の横幅
	private float height; //実機の縦幅
	public InputField EditCanvas; //文章編集エリアの取得
	private int char_num; //一行あたりの文字数
	private float font_size; //フォントサイズ
	public float ElapsedTime = 0.0F; //時間経過の取得
	public Vector2 EditPosition = new Vector2(0.0F, 0.0F); //編集位置を保存する変数
	public int SentenceCount = 0; //文章の数を保存する変数

	// Use this for initialization
	void Start () {
		init(); //諸々の設定を全て初期化
	}
	
	// Update is called once per frame
	void Update () {
		ElapsedTime += Time.deltaTime; //経過時間を計算
	}

	//エディターの縦横やパラメータの詳細設定を初期化
	private void init(){
		width  = (float)Screen.width; //実機の横幅を取得
		height = (float)Screen.height; //実機の縦幅を取得
		set_detail(); //エディタの詳細を設定
		set_size(width, height); //エディタサイズを定義
	}

	//エディタの詳細を設定する
	private void set_detail(){
		EditCanvas.GetComponent<InputField>().lineType = InputField.LineType.MultiLineNewline; //複数行の改行を許可
		EditCanvas.GetComponent<InputField>().contentType = InputField.ContentType.Standard; //全ての文字の入力を許可
		EditCanvas.GetComponent<InputField>().transition = InputField.Transition.None; //UIのインタラクティブな応答を未許可
		font_size = (float)EditCanvas.transform.Find("Text").GetComponent<Text>().fontSize; //設定されたフォントサイズを値として取得
	}

	//与えられた引数からエディタサイズをセット
	private void set_size(float width, float height){
		float editor_w = ( width * 2.0F ) / 3.0F; //実機の横幅からエディタの横幅を設定
		float editor_h = height; //実機の縦幅をエディタの縦幅として設定
		char_num = (int)( editor_w / font_size ) - 2; //フォントサイズと横幅から一行あたりの文字数を計算
		Vector2 size = new Vector2(editor_w, editor_h); //エディタサイズをベクトル化
		EditCanvas.GetComponent<RectTransform>().sizeDelta = size; //エディタの縦横を設定
	}

	//テキスト内容の変更時に起動する関数
	public void OnValueChange(){
		string text = EditCanvas.text; //エディタに書かれた全ての文字列を取得
		int caret = EditCanvas.caretPosition; //編集位置までに取得された文字数
		EditPosition = CalcEditPos(caret, text); //編集位置の計算
		SentenceCount = CountSentence(text); //文章の数を計算
	}

	//編集位置の座標をcaretとtextから計算する関数
	private Vector2 CalcEditPos(int caret, string text){
		Vector2 position = new Vector2(0.0F, 0.0F); //計算結果を格納する変数
		char[] split_chars = {'\n'}; //文章のSplit対象文字群
		string[] texts = EditCanvas.text.Split(split_chars); //改行で文章をSplit
		int indent = 0; //編集位置までの改行回数をカウントする
		int num = 0; //編集位置までに費やす文字数
		int temp_row = 0; //文字列が要する行数を一時的に格納する変数
		for(int i=0; i<texts.Length; i++){
			num = texts[i].Length;
			if( num < caret ){
				//numがcaretより小さい場合
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
			indent += 1; //次の文章をチェックする前に改行を+1する
		}
		position = new Vector2(caret * font_size, indent * font_size); //得られたcaretとindentから座標（x, y）を計算
		return position;
	}

	private int CountSentence(string text){
		int count = 0;
		char[] split_chars = {'。', '.', '．'}; //文章のSplit対象文字群
		string[] texts = EditCanvas.text.Split(split_chars); //文章ごとにSplit
		count = texts.Length;
		return count;
	}
}
