using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeCalcurator
{
    List<Gaze> proc_data; //加工したデータを格納する変数
    List<DataAttributeModel> attr_data; //データ属性を格納する変数
    float TEXT_SIZE = 14.0f; //文字サイズ
    float FIX_DIST = 2 * 14.0f; //固視微動の判定領域

    //コンストラクタ
    public GazeCalcurator()
    {
    }

    //処理済みデータを取得する関数
    public List<Gaze> get_proc_data(List<Gaze> raw_data)
    {
        fixation(raw_data);
        return proc_data;
    }

    //データ属性を取得する関数
    public List<DataAttributeModel> get_attr_data()
    {
        attr_data = new List<DataAttributeModel>();
        if (proc_data == null)
        {
            return null;
        }
        calcuration();
        return attr_data;
    }

    //属性値を計算する関数
    void calcuration()
    {
        //注目点
        float cx, cy, cts;
        //比較点
        float nx, ny, nts;
        for (int i = 0; i < proc_data.Count - 1; i++)
        {
            cx = proc_data[i].posX; //注目点x
            cy = proc_data[i].posY; //注目点y
            cts = proc_data[i].timestamp; //注目点ts
            nx = proc_data[i + 1].posX; //比較点x
            ny = proc_data[i + 1].posY; //比較点y
            nts = proc_data[i + 1].timestamp; //注目点ts
            float norm = get_norm(cx, cy, nx, ny);
            float velocity = get_velocity(cx, cy, nx, ny, cts, nts);
            float degree = get_degree(cx, cy, nx, ny);
            attr_data.Add(new DataAttributeModel(degree, norm, velocity));
        }
    }

    //移動量を計算
    float get_norm(float cx, float cy, float nx, float ny)
    {
        float norm = dist(cx, cy, nx, ny);
        return norm;
    }

    //移動速度を計算
    float get_velocity(float cx, float cy, float nx, float ny, float cts, float nts)
    {
        float velocity = dist(cx, cy, nx, ny) / (Mathf.Abs(nts - cts) * 100.0f);
        return velocity;
    }

    //移動方向の計算
    float get_degree(float cx, float cy, float nx, float ny)
    {
        float vx = (cx - nx) / dist(cx, cy, nx, ny); //方向x
        float vy = (cy - ny) / dist(cx, cy, nx, ny); //方向y
                                                     //基準となる単位ベクトル
        float bx = 0.0f;
        float by = 1.0f;
        //角度の計算
        float degree;
        if (vx < 0)
        {
            //角度180度以下
            degree = Mathf.Acos(bx * vy + vx * by) * (180 / Mathf.PI) - 180;
        }
        else
        {
            //角度180度以上
            degree = Mathf.Acos(bx * vy + vx * by) * (180 / Mathf.PI);
        }
        return degree;
    }

    //視線データから固始微動を除く関数
    void fixation(List<Gaze> raw_data)
    {
        proc_data = new List<Gaze>();
        for (int i = 0; i < raw_data.Count - 1; i++)
        {
            float cx = raw_data[i].posX; //注目点x
            float cy = raw_data[i].posY; //注目点y
            float cts = raw_data[i].timestamp; //注目点ts
            float nx = raw_data[i + 1].posX; //比較点x
            float ny = raw_data[i + 1].posY; //比較点x
            float nts = raw_data[i + 1].timestamp; //比較点ts
            float px; //処理済みx
            float py; //処理済みy
            float ts; //処理済みts
            if (proc_data.Count < 1)
            { //処理データがまだない場合
                if (dist(cx, cy, nx, ny) < FIX_DIST)
                {
                    px = (cx + nx) / 2.0f; //2点の平均値をx座標にする
                    py = (cy + ny) / 2.0f; //2点の平均値をy座標にする
                    ts = nts; //比較点ntsをtsとする
                    proc_data.Add(new Gaze(px, py, ts));
                }
                else
                {
                    px = cx; //注目点cxをx座標にする
                    py = cy; //注目点cyをy座標にする
                    ts = cts; //注目点ctsをtsとする
                    proc_data.Add(new Gaze(px, py, ts));
                }
            }
            else
            { //処理データがある場合
                float tempX = proc_data[proc_data.Count - 1].posX; //処理済みデータの最後尾x
                float tempY = proc_data[proc_data.Count - 1].posY; //処理済みデータの最後尾y
                if (dist(cx, cy, tempX, tempY) < FIX_DIST)
                {
                    px = (cx + 2 * tempX) / 3.0f;
                    py = (cy + 2 * tempY) / 3.0f;
                    Gaze temp = new Gaze(px, py, cts);
                    proc_data.Insert(proc_data.Count - 1, temp);
                }
                else
                {
                    px = cx; //注目点cxをx座標にする
                    py = cy; //注目点cyをy座標にする
                    ts = cts; //注目点ctsをtsとする
                    proc_data.Add(new Gaze(px, py, ts));
                }
            }
        }
    }

    float dist(float x1, float y1, float x2, float y2)
    {
        float dist = Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(y2 - y1, 2));
        return dist;
    }
}
