using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//視線の観測と読み返しのカウントを司るクラス

public class GazeStream : MonoBehaviour {

	LoadCSV gaze_data;

	string path = "yamaura";
	RereadDetector rbd; //読み返し検出クラス

	int num = 0; //読み取り開始データ番号
	//データ格納変数
	float px, py;
	string ts; 

	// Use this for initialization
	void Start () {
		rbd = new RereadDetector();
		gaze_data = new LoadCSV(path);
	}
	
	// Update is called once per frame
	void Update () {
		for (int i=0; i<100; i++) {
			if (num >= gaze_data.getRowCount()-1) {
				//Debug.Log("check all data");
				break;
			}
			px = gaze_data.getFloat(num, 0);
			py = gaze_data.getFloat(num, 1);
			ts = gaze_data.getString(num, 2);
			rbd.update(px, py, ts);
			num++;
		}
		rbd.read_counter();
	}
}
