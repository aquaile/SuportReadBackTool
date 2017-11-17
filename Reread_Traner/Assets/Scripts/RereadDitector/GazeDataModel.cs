using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GazeDataModel
{
    private List<Gaze> data; //受信した視線データを格納する変数

    //コンストラクタ
    public GazeDataModel()
    {
        data = new List<Gaze>();
    }

    //視線データを受信したら値を変数に追加する関数
    public void update(float px, float py, string ts)
    {
        data.Add(new Gaze(px, py, ts));
    }

    //データの要請があった場合に返す関数（取り出す値の個数、開始点）
    public List<Gaze> get_data(int num, int pivot)
    {
        List<Gaze> gaze = new List<Gaze>(); //返送するデータを格納
                                            //求める個数に満たなかった場合はnullを返す
        if (data.Count < pivot + num)
        {
            //num = data.size() - pivot; //デバッグ用：取り出す個数をデータ全てにする
            return null;
        }
        for (int i = num; i < pivot + num; i++)
        {
            float px = data[i].posX; //i番目のx座標
            float py = data[i].posY; //i番目のy座標
            float ts = data[i].timestamp; //i番目のタイムスタンプ
            gaze.Add(new Gaze(px, py, ts));
        }
        return gaze;
    }
}

