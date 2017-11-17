using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataAttributeModel
{
    //内部変数
    public float degree; //角度
    public float norm;  //大きさ
    public float velocity;  //速度 
                            //コンストラクタ（x方向、y方向、角度、大きさ、速度）
    public DataAttributeModel(float deg, float nor, float vel)
    {
        degree = deg;
        norm = nor;
        velocity = vel;
    }
}

