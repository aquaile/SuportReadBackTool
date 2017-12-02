using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class DataManager : MonoBehaviour {

	public string Path; //保存ファイルまでのパス
	public string Article; //内容
	public int Short; //短い読み返し
	public int Long; //長い読み返し
	public float Timelapse; //経過時間
	public string Name; //ファイル名
	public List<GazeData> Data; //視線データ
	GazeManager gm; //関数を起動するため
	public GameObject GazeManager; //GazeMangerを動かしているゲームオブジェクトを取得
	List<GazeData> PreviousData; //ロードした視線データを保管する
	public GameObject EditorManager;
	EditorManager em;
	GameObject timer; //一箇所で管理されている時間を格納するための変数

	// Use this for initialization
	void Start () {
		Init();
	}

	//テストデータをセットする
	public void Init(){
		Path = "SentenceFiles";
		gm = GazeManager.GetComponent<GazeManager>();
		em = EditorManager.GetComponent<EditorManager>();
		timer = GameObject.FindGameObjectsWithTag("timer")[0];
	}
	
	// Update is called once per frame
	void Update () {

	}

	//ストリーミングアセットからデータをロードする
	public IEnumerator Load(string Path, string Name){
		string FilePath = Application.dataPath + "/StreamingAssets/" + Path + "/" + Name + ".json";
		SaveData sd = new SaveData();
		//BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream file = File.Open(FilePath, FileMode.Open);
		//string JSON = (string) binaryFormatter.Deserialize(file);
		StreamReader sr = new StreamReader(file);
		string JSON = sr.ReadToEnd();
		Debug.Log(JSON);
		file.Close();
		sd = JsonUtility.FromJson<SaveData>(JSON);
		yield return new WaitForSeconds(5);
		//このScriptの保存用データをそれぞれ書き換える
		Name = sd.FileName;
		Long = sd.Long;
		Short = sd.Short;
		Article = sd.Article;
		Timelapse = sd.Timelapse;
		//他のScriptにロードしたデータを書き込む
		timer.GetComponent<TimeKeeper>().timelapse = Timelapse;
		gm.Short = Short;
		gm.Long = Long;
		em.EditCanvas.text = Article;
		Debug.Log("ロードが完了しました");
	}

	//ストリーミングアセットのデータリストを表示する
	public FileInfo[] LoadList(string Path){
		string FilePath = Application.dataPath + "/StreamingAssets/" + Path + "/";
		DirectoryInfo directory = new DirectoryInfo(FilePath);
		FileInfo[] info = directory.GetFiles("*.json");
		return info;
	}

	//ストリーミングアセットにデータをセーブする
	public IEnumerator Save(string Path, string Name){
		Data = gm.GetSaveData();
		Short = gm.Short;
		Long = gm.Long;
		Timelapse = timer.GetComponent<TimeKeeper>().timelapse;
		Article = em.EditCanvas.text;
		yield return null;
		string FilePath = Application.dataPath + "/StreamingAssets/" + Path + "/" + Name + ".json";
		SaveData sd = SetData(Article, Short, Long, Timelapse, Name, Data);
		string JSON = JsonUtility.ToJson(sd);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream file = File.Create(FilePath);
		StreamWriter ws = new StreamWriter(file);
		ws.WriteLine(JSON);
		ws.Flush();
		//binaryFormatter.Serialize(file, JSON);
		file.Close();
		yield return new WaitForSeconds(5);
		Debug.Log("セーブが完了しました");
	}

	//セーブする情報をクラスに保存する
	SaveData SetData(string _article, int _short, int _long, float _timelapse, string _name, List<GazeData> _gaze){
		SaveData data = new SaveData();
		data.Article = _article;
		data.Short = _short;
		data.Long = _long;
		data.Timelapse = _timelapse;
		data.FileName = _name;
		data.Gaze = _gaze;
		return data;
	}
}

[Serializable]
public class SaveData{
	public string Article; //記事内容
	public int Short; //短い読み返しの回数
	public int Long; //長い読み返しの回数
	public float Timelapse; //経過時間
	public string FileName; //保存時のファイルの名前
	public List<GazeData> Gaze; //視線データ
}
