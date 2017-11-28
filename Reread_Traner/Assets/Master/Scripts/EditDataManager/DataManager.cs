using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataManager : MonoBehaviour {

	public string Path;
	public string Article;
	public int Short;
	public int Long;
	public float Timelapse;
	public string Name;

	// Use this for initialization
	void Start () {
		Init();
		Save(Path, Name);
		Load(Path, Name);
		LoadList(Path);
	}

	//テストデータをセットする
	void Init(){
		Path = "SentenceFiles";
		Article = "ぎゃああああああああああああああああああす。\nばああああああああああああああああああああああ。";
		Short = 10;
		Long = 5;
		Timelapse = 100.0F;
		Name = "Test_Date";
	}
	
	// Update is called once per frame
	void Update () {

	}

	//ストリーミングアセットからデータをロードする
	void Load(string Path, string Name){
		string FilePath = Application.dataPath + "/StreamingAssets/" + Path + "/" + Name + ".json";
		SaveData sd;
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream file = File.Open(FilePath, FileMode.Open);
		string JSON = (string) binaryFormatter.Deserialize(file);
		file.Close();
		sd = JsonUtility.FromJson<SaveData>(JSON);
		Debug.Log(sd.Article);
	}

	//ストリーミングアセットのデータリストを表示する
	void LoadList(string Path){
		string FilePath = Application.dataPath + "/StreamingAssets/" + Path + "/";
		DirectoryInfo directory = new DirectoryInfo(FilePath);
		FileInfo[] info = directory.GetFiles("*.json");
		foreach( FileInfo data in info ){
			Debug.Log(data);
		}
	}

	//ストリーミングアセットにデータをセーブする
	void Save(string Path, string Name){
		string FilePath = Application.dataPath + "/StreamingAssets/" + Path + "/" + Name + ".json";
		SaveData sd = SetData(Article, Short, Long, Timelapse, Name);
		string JSON = JsonUtility.ToJson(sd);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream file = File.Create(FilePath);
		binaryFormatter.Serialize(file, JSON);
		file.Close();
	}

	//セーブする情報をクラスに保存する
	SaveData SetData(string _article, int _short, int _long, float _timelapse, string _name){
		SaveData data = new SaveData();
		data.Article = _article;
		data.Short = _short;
		data.Long = _long;
		data.Timelapse = _timelapse;
		data.FileName = _name;
		return data;
	}
}

[SerializeField]
public class SaveData{
	public string Article; //記事内容
	public int Short; //短い読み返しの回数
	public int Long; //長い読み返しの回数
	public float Timelapse; //経過時間
	public string FileName; //保存時のファイルの名前
}
