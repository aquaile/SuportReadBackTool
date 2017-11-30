using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour {

	public GameObject dataManager; //データをむにゃむにゃする
	DataManager dm;
	Button Save;
	public GameObject SavePanel;

	// Use this for initialization
	void Start () {
		dm = dataManager.GetComponent<DataManager>();
		Save = SavePanel.transform.Find("Save").GetComponent<Button>();
		Save.onClick.AddListener(OnClicked);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnClicked(){
		Debug.Log(dm.Short);
		StartCoroutine( dm.Save( dm.Path, dm.Name ) );
	}
}
