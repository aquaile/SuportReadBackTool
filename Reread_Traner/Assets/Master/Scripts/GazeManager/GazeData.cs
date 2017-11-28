/*

視線データを保持する構造体

*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct GazeData{
	public float posX; //視線x座標
	public float posY; //視線y座標
	public float timelapse; //視線タイムスタンプ
	public float degree; //視線タイムスタンプ
	public float norm; //視線タイムスタンプ
	public float velocity; //視線タイムスタンプ

	//コンストラクタ（属性無し）
	public GazeData(float _posX, float _posY, float _timelapse){
		posX = _posX;
		posY = _posY;
		timelapse = _timelapse;
		degree = 0.0F;
		norm = 0.0F;
		velocity = 0.0F;
	}

	//コンストラクタ（属性有り）
	public GazeData(float _posX, float _posY, float _timelapse, float _degree, float _norm, float _velocity){
		posX = _posX;
		posY = _posY;
		timelapse = _timelapse;
		degree = _degree;
		norm = _norm;
		velocity = _velocity;
	}
}
