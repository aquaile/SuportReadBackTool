/*

画像の諸々の詳細を設定するクラス

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSetting : MonoBehaviour {

	public Canvas ParentCanvas; //Imageを配置する親Canvas
	float height; //実機の縦幅
	float pict_size; //Imageのサイズ
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
		height = (float)Screen.height; //実機の縦幅を取得
		pict_size = height / 15.0F; //画像サイズを指定する
		GameObject Image = ParentCanvas.transform.Find("Image").gameObject; //Canvas直下のImageを取得する
		Image.GetComponent<Image>().sprite = Element; //取得したImageに指定した画像を設定する
		Image.GetComponent<RectTransform>().sizeDelta = new Vector2( pict_size, pict_size ); //画像サイズを指定
		ParentCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2( pict_size, pict_size ); //Canvasのサイズを指定
	}
}
