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

	//コンストラクタ
	public GazeData(float _posX, float _posY, float _timelapse){
		posX = _posX;
		posY = _posY;
		timelapse = _timelapse;
	}
}
