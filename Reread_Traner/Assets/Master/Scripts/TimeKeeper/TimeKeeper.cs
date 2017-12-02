using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeKeeper : MonoBehaviour {
	public float timelapse;

	// Use this for initialization
	void Start () {
		timelapse = 0.0F;
	}
	
	// Update is called once per frame
	void Update () {
		timelapse += Time.deltaTime;
	}
}
