using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeManager : MonoBehaviour {

	Detector detect = new Detector(); //読み返し検出器
	public float timelaple = 0.0F; //経過時間
	float Width; //編集領域の横幅
	float Height; //編集領域の縦幅
	float testX; //x座標
	float testY; //y座標
	int[] Count = new int[2]; //カウント
	public int Short; //長い読み返し
	public int Long; //短い読み返し
	LoadCSV csv; //CSVを読み込むクラス
	string Path = "yamaura"; //読み込むCSVファイルまでのパス
	bool isCalc = false; //計算中の判定

	// Use this for initialization
	void Start () {
		Setup();
	}
	
	// Update is called once per frame
	void Update () {
		timelaple += Time.deltaTime;
		testX = TransValue(csv.getFloat( Time.frameCount, 0 ), 0);
		testY = TransValue(csv.getFloat( Time.frameCount, 1 ), 1);
		detect.UpdateGazeData(testX, testY, timelaple);
		if( (int)timelaple % 10 == 0){
			if( !isCalc ){
				Count = detect.RereadCount();
				Short = Count[0];
				Long = Count[1];
				Debug.Log(Short + "：" + Long);
				isCalc = true;
			}else{
				
			}
		}else{
			isCalc = false;
		}
	}

	//テストデータを変換
	float TransValue(float value, int type){
		float resize;
		if( type == 0 ){
			 resize = Width / 1920.0F;
			 return value * resize;
		}else{
			resize = Height / 1080.0F;
			return value * resize;
		}
	}

	void Setup(){
		Width = (float)Screen.width;
		Height = (float)Screen.height;
		csv = new LoadCSV(Path);
	}
}
