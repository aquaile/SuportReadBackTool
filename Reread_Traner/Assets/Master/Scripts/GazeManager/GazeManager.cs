using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Tobii関連設定
//using Tobii.Gamimg;
public class GazeManager : MonoBehaviour {

	Detector detect = new Detector(); //読み返し検出器
	float timelapse = 0.0F; //経過時間
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
	GameObject timer; //一箇所で管理されている時間を格納するための変数

	//Tobii関連の記述
	//private GazePoint _lastHandledPoint = GazePoint.Invalid;
	//private GazePoint _endPoint;

	// Use this for initialization
	void Start () {
		Setup();
		timer = GameObject.FindGameObjectsWithTag("timer")[0];
	}
	
	// Update is called once per frame
	void Update () {
		timelapse = timer.GetComponent<TimeKeeper>().timelapse;
		testX = TransValue(csv.getFloat( Time.frameCount, 0 ), 0);
		testY = TransValue(csv.getFloat( Time.frameCount, 1 ), 1);
		//GetAndSave();
		detect.UpdateGazeData(testX, testY, timelapse);
		if( (int)timelapse % 10 == 0){
			if( !isCalc ){
				Count = detect.RereadCount();
				Short += Count[0];
				Long += Count[1];
				Debug.Log(Short + "：" + Long);
				isCalc = true;
			}
		}else{
			isCalc = false;
		}
	}

	//Tobiiから視線データを取得 + Modelへのデータ送信
	/*
	void GetAndSave(){
		IEnumerable<GazePoint> pointsSinceHandled = Tobii.GetGazePointsSince(_lastHandledPoint);
		foreach(GazePoint point ini pointsSinceHandled){
			Vector2 gp = point.Screen;
			float ts = point.Timestamp;
			_endPoint = point;
			//ここを後々書き換えたい
			testX = gp.x;
			testY = gp.y;
			timelapse = timer.GetComponent<TimeKeeper>().timelapse;
			detect.UpdateGazeData(testX, testY, timelapse);
		}
		_lastHandledPoint = _endPoint;
	}
	*/

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

	//保存用の視線データを取得
	public List<GazeData> GetSaveData(){
		List<GazeData> data = detect.GetDataList();
		return data;
	}

	void Setup(){
		Width = (float)Screen.width;
		Height = (float)Screen.height;
		csv = new LoadCSV(Path);
	}
}
