using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterSetting : MonoBehaviour {

	public Canvas ParentCanvas; //Imageを配置する親Canvas
	float width; //実機の縦幅
	float pict_size_x; //Imageのサイズx
	float pict_size_y; //Imageのサイズy
	public Sprite Element; //誘導体として提示する画像

	// Use this for initialization
	void Start () {
		setting(); //画像の詳細を設定
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//配置するImageの詳細設定
	void setting(){
		width = (float)Screen.width; //実機の縦幅を取得
		pict_size_x = (width / 3.0F ) * 2.0F; //画像サイズを指定する
		pict_size_y = 14.0F; //フォントサイズの2倍 <- ちゃんと参照するように修正します
		GameObject Image = ParentCanvas.transform.Find("Image").gameObject; //Canvas直下のImageを取得する
		Image.GetComponent<Image>().sprite = Element; //取得したImageに指定した画像を設定する
		Image.GetComponent<RectTransform>().sizeDelta = new Vector2( pict_size_x, pict_size_y ); //画像サイズを指定
		ParentCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2( pict_size_x, pict_size_y ); //Canvasのサイズを指定
	}
}
