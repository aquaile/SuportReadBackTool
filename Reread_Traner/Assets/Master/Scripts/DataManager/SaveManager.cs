﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour {

	public GameObject dataManager; //データをむにゃむにゃする
	DataManager dm;
	Button Savebtn;
	public GameObject SavePanel;
	public InputField NameField;

	// Use this for initialization
	void Start () {
		dm = dataManager.GetComponent<DataManager>();
		Savebtn = SavePanel.transform.Find("Save").GetComponent<Button>();
		Savebtn.onClick.AddListener(OnClicked);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnClicked(){
		string name = NameField.text;
		Debug.Log(name);

		//ここコルーチンするのよくないと思う
		StartCoroutine( dm.Save( dm.Path, name ) );
		StartCoroutine( dm.PostJSON() );
	}
}
