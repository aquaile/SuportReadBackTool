using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour {

	public GameObject dataManager; //データをむにゃむにゃする
	DataManager dm;
	Button Load;
	public GameObject LoadPanel;

	// Use this for initialization
	void Start () {
		dm = dataManager.GetComponent<DataManager>();
		Load = LoadPanel.transform.Find("Load").GetComponent<Button>();
		Load.onClick.AddListener(OnClicked);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnClicked(){
		Debug.Log(dm.Short);
		StartCoroutine(dm.Load( dm.Path, dm.Name ) );
	}
}
