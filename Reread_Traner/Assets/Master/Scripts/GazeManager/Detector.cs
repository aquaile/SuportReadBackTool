using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector{
	List<GazeData> Data; //生の視線データを格納する変数
	Calcurator Calc; //注視点処理と属性計算を行うクラス
	List<GazeData> SettledData; //加工済みの属性を持った視線データを格納する変数
	List<GazeData> WritePoints; //書き取りor読み取りとして検出された視線データを格納する変数
	float Width; //全体領域の横幅
	float Height; //全体領域の縦幅
	int PIVOT = 0; //データ取得の開始点
	int INTERVAL = 6000; //一度に取得するデータの個数

	//コンストラクタ
	public Detector(){
		Data = new List<GazeData>();
		Calc = new Calcurator();
		Width = (float)Screen.width;
		Height = (float)Screen.height;
	}
	
	//新しい視線データを構造体に追加していく関数
	public void UpdateGazeData(float posX, float posY, float Timelapse){
		Data.Add(new GazeData(posX, posY, Timelapse));
	}

	//Calcuratorを用い、注視点処理と属性計算が行われた視線データを取得する
	void GetData(){

	}

	//書き取り検出
	void DetectWrite(){

	}
}
