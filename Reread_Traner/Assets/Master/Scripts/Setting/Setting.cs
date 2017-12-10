using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Setting : MonoBehaviour {
	public SetUpData setting = new SetUpData();
	// Use this for initialization
	void Start () {
		Load();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Load(){
		string FilePath = Application.dataPath + "/StreamingAssets/Setting/setting.json"; // 保存先Pathの設定
		SetUpData temp = new SetUpData(); //セーブ用のクラスのインスタンス化
		FileStream file = File.Open(FilePath, FileMode.Open); //保存先にファイルを開く

		// JSONデータをバイナリとして保存したいときに使う奴
		// BinaryFormatter binaryFormatter = new BinaryFormatter();
		// string JSON = (string) binaryFormatter.Deserialize(file);

		//JSONデータをそのままの形式で保存するときに使う奴
		StreamReader sr = new StreamReader(file);
		string JSON = sr.ReadToEnd();
		file.Close();

		temp = JsonUtility.FromJson<SetUpData>(JSON);

		setting = temp;
		
	}
}

[Serializable]
public class SetUpData {
	public int InduceType; // 誘導タイプ
	public int TimeRange; //誘導時間
	public int CorrectionLevel; //誘導強度
}
