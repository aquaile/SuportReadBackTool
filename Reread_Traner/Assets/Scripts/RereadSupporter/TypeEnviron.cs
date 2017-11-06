using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeEnviron : SupportType {

	Canvas EffectCanvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void initialize(Canvas canvas){
		EffectCanvas = canvas;
	}
}
