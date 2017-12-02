
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour {

	public Canvas EditorCanvas; //文章編集用のCanvas
	public GameObject GazeManager; //視線監視用のゲームオブジェクト
	public GameObject InduceManager; //誘導制御用のゲームオブジェクト
	float timelapse = 0.0F; //経過時間
	public int INTERVAL = 60; //読み返し誘導の判断を行う間隔
	bool isDecision = false; //判定を60秒に入った1回目だけ行う
	int Short = 0; //短い読み返しを保存する
	int Long = 0; //長い読み返しを保存する
	int SentenceCount = 0; //文章数
	Vector2 EditPosition = new Vector2( 0.0F, 0.0F ); //編集座標
	GameObject timer; //一箇所で管理されている時間を格納するための変数

	// Use this for initialization
	void Start () {
		timer = GameObject.FindGameObjectsWithTag("timer")[0];
	}
	
	// Update is called once per frame
	void Update () {
		timelapse = timer.GetComponent<TimeKeeper>().timelapse;
		if( (int)timelapse % 10 == 0){
			if( !isDecision ){
				Short = GazeManager.GetComponent<GazeManager>().Short;
				Long = GazeManager.GetComponent<GazeManager>().Long;
				SentenceCount = EditorCanvas.GetComponent<EditorManager>().SentenceCount;
				EditPosition = EditorCanvas.GetComponent<EditorManager>().EditPosition;
				InduceManager.GetComponent<InduceManager>().Short = Short;
				InduceManager.GetComponent<InduceManager>().Long = Long;
				InduceManager.GetComponent<InduceManager>().SentenceCount = SentenceCount;
				InduceManager.GetComponent<InduceManager>().EditPosition = EditPosition;
				isDecision = true;
			}
		}else{
			isDecision = false;
		}
	}
}
