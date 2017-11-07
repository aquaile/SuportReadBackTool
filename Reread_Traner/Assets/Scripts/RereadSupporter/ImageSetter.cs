using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//振動effectに使用するオブジェクトの設定を行うクラス
public class ImageSetter : MonoBehaviour {

	public Canvas ImageCanvas; //親キャンバス



	// Use this for initialization
	void Start () {
		ImageSetting("Resources");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ImageSetting(string image_path){
		GameObject img = ImageCanvas.transform.Find("Image").gameObject;
		Debug.Log(image_path);
	}
}
