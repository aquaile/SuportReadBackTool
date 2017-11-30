using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeMainMenu : MonoBehaviour {

	public GameObject MainMenu;
	bool isActive = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKeyDown(KeyCode.Tab) ){
			MainMenu.SetActive(!isActive);
			isActive = !isActive;
		}
	}
}
