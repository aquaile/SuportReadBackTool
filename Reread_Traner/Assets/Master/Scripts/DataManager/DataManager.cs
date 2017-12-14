using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class DataManager : MonoBehaviour {

	public string Path; //保存ファイルまでのパス
	GazeManager gm; //関数を起動するため
	public GameObject GazeManager; //GazeMangerを動かしているゲームオブジェクトを取得
	List<GazeData> PreviousData; //ロードした視線データを保管する
	public GameObject EditorManager;
	EditorManager em;
	GameObject timer; //一箇所で管理されている時間を格納するための変数
	[SerializeField]
	SaveData TempData; //ロード時やセーブ時に結果を保存する変数

	// Use this for initialization
	void Start () {
		Init();
	}

	//ゲームオブジェクトを取得・設定する
	public void Init(){
		Path = "SentenceFiles";
		gm = GazeManager.GetComponent<GazeManager>();
		em = EditorManager.GetComponent<EditorManager>();
		timer = GameObject.FindGameObjectsWithTag("timer")[0];
		TempData.Date = System.DateTime.Now.ToString("yyyy/MM/dd");
	}
	
	// Update is called once per frame
	void Update () {

	}

	//ストリーミングアセットからデータをロードする
	public IEnumerator Load(string Path, string Name){
		string FilePath = Application.dataPath + "/StreamingAssets/" + Path + "/" + Name + ".json"; // 保存先Pathの設定
		SaveData sd = new SaveData(); //セーブ用のクラスのインスタンス化
		FileStream file = File.Open(FilePath, FileMode.Open); //保存先にファイルを開く

		// JSONデータをバイナリとして保存したいときに使う奴
		// BinaryFormatter binaryFormatter = new BinaryFormatter();
		// string JSON = (string) binaryFormatter.Deserialize(file);

		//JSONデータをそのままの形式で保存するときに使う奴
		StreamReader sr = new StreamReader(file);
		string JSON = sr.ReadToEnd();
		file.Close();

		//クラスデータのJSON化
		sd = JsonUtility.FromJson<SaveData>(JSON);
		yield return null; //一度返す

		//このScriptの保存用データをそれぞれ書き換える
		TempData = DeepCopy(sd);

		//他のScriptにロードしたデータを書き込む
		timer.GetComponent<TimeKeeper>().timelapse = TempData.Timelapse;
		gm.Short = TempData.Short;
		gm.Long = TempData.Long;
		em.EditCanvas.text = TempData.Article;
		Debug.Log("ロードが完了しました");
	}

	// SaveDataクラスをディープコピーする
	SaveData DeepCopy(SaveData obj){
		SaveData copy = new SaveData();
		copy.FileName = obj.FileName;
		copy.Article = obj.Article;
		copy.Gaze = obj.Gaze;
		copy.Long = obj.Long;
		copy.Short = obj.Short;
		copy.Timelapse = obj.Timelapse;
		copy.Date = obj.Date;
		return copy;
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
		TempData.Gaze = gm.GetSaveData(); //視線データの取得
		TempData.Short = gm.Short; //短い読み返しの取得
		TempData.Long = gm.Long; //長い読み返しの取得
		TempData.Timelapse = timer.GetComponent<TimeKeeper>().timelapse;
		TempData.Article = em.EditCanvas.text;
		TempData.FileName = Name;
		TempData.CountTime = gm.ct;
		yield return null;
		string FilePath = Application.dataPath + "/StreamingAssets/" + Path + "/" + Name + ".json";
		SaveData sd = SetData(TempData);
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

	//PHPにJSONをpostする
	public IEnumerator PostJSON(){
		string Path = "http://fukuchi.nkmr.io/study/test.php"; //アクセス先
		SaveData temp = new SaveData();
		WWWForm form = new WWWForm();
		yield return null;
		SaveData _savedata = SetData(TempData);
		string _json = JsonUtility.ToJson(_savedata);
		yield return null;
		form.AddField( "json", _json );
		WWW post = new WWW( Path, form );
		yield return post;
		if( post != null ){
			Debug.Log( post.text );
		}
	}

	//セーブする情報をクラスに保存する
	SaveData SetData(SaveData obj){
		SaveData data = new SaveData();
		data.Article = obj.Article;
		data.Short = obj.Short;
		data.Long = obj.Long;
		data.Timelapse = obj.Timelapse;
		data.FileName = obj.FileName;
		data.Gaze = obj.Gaze;
		data.Date = obj.Date;
		data.CountTime = obj.CountTime;
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
	public string Date; //日付
	public List<CountTime> CountTime;
}
