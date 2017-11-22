/*

視線データのメタ情報を保持する保持する構造体

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttributeData{
	public float degree { get; set; } //視線移動の角度
	public float norm { get; set; } //視線移動の大きさ
	public float velocity { get; set; } //視線移動の速さ

	public AttributeData(float _degree, float _norm, float _velocity){
		degree = _degree;
		norm = _norm;
		velocity = _velocity;
	}
}
