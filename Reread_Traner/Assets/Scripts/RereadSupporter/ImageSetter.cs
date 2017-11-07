using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//振動effectに使用するオブジェクトの設定を行うクラス
public class ImageSetter : MonoBehaviour {

	public Canvas ImageCanvas; //親キャンバス
	float height; //実機の縦幅
	float size; //Imageのサイズ



	// Use this for initialization
	void Start () {
		ImageSetting("Resources");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ImageSetting(string image_path){
		height = (float)Screen.height;

		size = height/15.0F;
		
		GameObject Img = ImageCanvas.transform.Find("Image").gameObject; //Canvas直下のImageを取得
		Img.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/CROSS"); //Imageを指定された画像に設定
		Img.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size); //Imageのサイズ指定
		CanvasSetting(size);
		//Debug.Log(image_path);
	}

	void CanvasSetting(float _size){
		ImageCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(_size, _size); //Canvasのサイズ指定
	}
}
