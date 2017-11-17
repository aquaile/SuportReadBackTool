using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaze
{
    public float posX;
    public float posY;
    public float timestamp;

    //コンストラクタ（ x座標、y座標, タイムスタンプ<文字列> ）
    public Gaze(float px, float py, string ts)
    {
        posX = px;
        posY = py;
        timestamp = str2sec(ts);
    }
    //コンストラクタ（ x座標、y座標, タイムスタンプ<浮動小数> ）
    public Gaze(float px, float py, float ts)
    {
        posX = px;
        posY = py;
        timestamp = ts;
    }
    //String型のタイムスタンプをFloat型に変換する
    float str2sec(string str)
    {
        string ts = str.Substring(4); //日付データを取り除いたタイムスタンプを取得
        float h = float.Parse(ts.Substring(0, 2)) * 60.0f * 60.0f; //時間
        float m = float.Parse(ts.Substring(2, 2)) * 60.0f; //分
        float s = float.Parse(ts.Substring(4, 2)); //秒
        float ms = float.Parse(ts.Substring(6, 2)) / 100.0f; //ミリ秒を有効数字3桁表現
        return h + m + s + ms;
    }
}

