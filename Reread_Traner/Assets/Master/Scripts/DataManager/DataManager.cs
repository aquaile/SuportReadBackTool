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
	public List<GazeData> Data;
	GazeManager gm;
	public GameObject GazeManager;

	// Use this for initialization
	void Start () {
		Init();
	}

	//テストデータをセットする
	void Init(){
		Path = "SentenceFiles";
		Article = "ぎゃああああああああああああああああああす。\nばああああああああああああああああああああああ。";
		Short = 10;
		Long = 5;
		Timelapse = 100.0F;
		Name = "Test_Gaze";
		gm = GazeManager.GetComponent<GazeManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if( Time.frameCount == 100 ){
			Data = gm.GetSaveData();
			StartCoroutine(Save(Path, Name));
			StartCoroutine(Load(Path, Name));
			LoadList(Path);
		}
	}

	//ストリーミングアセットからデータをロードする
	IEnumerator Load(string Path, string Name){
		string FilePath = Application.dataPath + "/StreamingAssets/" + Path + "/" + Name + ".json";
		SaveData sd = new SaveData();
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream file = File.Open(FilePath, FileMode.Open);
		string JSON = (string) binaryFormatter.Deserialize(file);
		file.Close();
		sd = JsonUtility.FromJson<SaveData>(JSON);
		yield return new WaitForSeconds(10);
		if( sd != null ){
			Debug.Log("Gaze Data Count：" + sd.Article);
		}else{
			Debug.Log("無理だー");
		}
	}

	//ストリーミングアセットのデータリストを表示する
	void LoadList(string Path){
		string FilePath = Application.dataPath + "/StreamingAssets/" + Path + "/";
		DirectoryInfo directory = new DirectoryInfo(FilePath);
		FileInfo[] info = directory.GetFiles("*.json");
		foreach( FileInfo data in info ){
			Debug.Log("File Name：" + data);
		}
	}

	//ストリーミングアセットにデータをセーブする
	IEnumerator Save(string Path, string Name){
		string FilePath = Application.dataPath + "/StreamingAssets/" + Path + "/" + Name + ".json";
		SaveData sd = SetData(Article, Short, Long, Timelapse, Name, Data);
		string JSON = JsonUtility.ToJson(sd);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream file = File.Create(FilePath);
		binaryFormatter.Serialize(file, JSON);
		file.Close();
		yield return new WaitForSeconds(5);
		Debug.Log("Save Data Article\n" + sd.Article);
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

[SerializeField]
public class SaveData{
	public string Article; //記事内容
	public int Short; //短い読み返しの回数
	public int Long; //長い読み返しの回数
	public float Timelapse; //経過時間
	public string FileName; //保存時のファイルの名前
	public List<GazeData> Gaze; //視線データ
}
