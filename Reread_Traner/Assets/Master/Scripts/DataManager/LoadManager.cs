using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class LoadManager : MonoBehaviour {

	public GameObject dataManager; //データをむにゃむにゃする
	DataManager dm;
	Button Load;
	public GameObject LoadPanel;
	public GameObject Content;
	public GameObject Toggle;

	// Use this for initialization
	void Start () {
		dm = dataManager.GetComponent<DataManager>();
		Load = LoadPanel.transform.Find("Load").GetComponent<Button>();
		Load.onClick.AddListener(OnClicked);
		FileInfo[] files = dm.LoadList(dm.Path);
		foreach(FileInfo info in files ){
			GameObject prefab = (GameObject)Instantiate(Toggle); //prefabにElementを指定してインスタンス化
			prefab.transform.SetParent(Content.transform, false); //親を直接誘導用のCanvasに指定
			prefab.GetComponent<Toggle>().group = Content.GetComponent<ToggleGroup>();
			char[] split_chars = {'/'}; //文章のSplit対象文字群
			string[] directory = info.FullName.Split(split_chars);
			string name = directory[directory.Length-1];
			name = name.Replace(".json", "");
			prefab.transform.Find("Label").GetComponent<Text>().text = name;
			Debug.Log(name);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnClicked(){
		bool isOn = Content.GetComponent<ToggleGroup>().AnyTogglesOn();
		if( isOn ){
			Toggle load = Content.GetComponent<ToggleGroup>().ActiveToggles().FirstOrDefault();
			string name = load.transform.Find("Label").GetComponent<Text>().text;
			Debug.Log(name);
			StartCoroutine(dm.Load( dm.Path, name ) );
		}
	}
}
