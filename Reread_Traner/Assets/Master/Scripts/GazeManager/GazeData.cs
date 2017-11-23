/*

視線データを保持する構造体

*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GazeData{
	public float posX { get; set; } //視線x座標
	public float posY { get; set; }  //視線y座標
	public float timelapse { get; set; }  //視線タイムスタンプ
	public float degree { get; set; }  //視線タイムスタンプ
	public float norm { get; set; }  //視線タイムスタンプ
	public float velocity { get; set; }  //視線タイムスタンプ

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
