using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RereadDetector
{
    GazeDataModel gdm; //視線データを格納するモデル
    GazeCalcurator gc; //視線データの加工
    DataAttributeModel dam; //データ属性を格納するモデル
    int PIVOT = 0; //データ取得の開始点
    int INTERVAL = 6000; //データ取得の個数
    List<Gaze> raw_data; //生データ格納変数
    List<Gaze> proc_data; //加工したデータを格納する変数
    List<Gaze> write_data; //書き取りに関するデータを格納する変数
    List<DataAttributeModel> attr_data; //データ属性を格納する変数
	float width; //実機の横幅
	float height; //実機の縦幅

    //コンストラクタ
    public RereadDetector()
    {
        gdm = new GazeDataModel();
        gc = new GazeCalcurator();
		width = (float)Screen.width;
		height = (float)Screen.height;
    }

    //更新
    public void update(float px, float py, string ts)
    {
        gdm.update(px, py, ts);
    }

    //必要なデータの取得
    void get_data()
    {
        raw_data = new List<Gaze>();
        proc_data = new List<Gaze>();
        raw_data = gdm.get_data(PIVOT, INTERVAL); //生データを取得
        if (raw_data != null)
        {
            PIVOT += INTERVAL; //データ取得の開始点を移動
            proc_data = gc.get_proc_data(raw_data); //生データから処理データを取得
            attr_data = gc.get_attr_data(); //属性データを取得
        }
    }

    //書き取りの検出
    void check_write()
    {
        get_data();
        if (raw_data != null)
        {
            //必要なデータの取得ができた場合
            Debug.Log(proc_data.Count + "：" + PIVOT);
            write_data = new List<Gaze>();
            float cx, cy, cts; //注目点
            float cd; //角度

            //データを順繰りにチェック
            for (int i = 0; i < proc_data.Count - 1; i++)
            {
                cx = proc_data[i].posX;
                cy = proc_data[i].posY;
                cts = proc_data[i].timestamp;
                cd = attr_data[i].degree;
                //書き取り条件との一致をチェック
                if (isWrite(cd, cx, cy))
                {
                    write_data.Add(new Gaze(cx, cy, cts));
                }
            }
        }
        else
        {
            //必要なデータが取得できなかった場合
            //println("please wait");
        }
    }

    //読み返し検出
    public void read_counter()
    {
        int short_read = 0; //短い読み返しの回数
        int long_read = 0; //長い読み返しの回数
        check_write();
        if (write_data != null)
        {
            //println(write_data.size());
            //display_write();
            float cx, cy, cts; //注目点
            float nx, ny, nts; //比較点
                               //データを順繰りにチェック
            for (int i = 0; i < write_data.Count - 1; i++)
            {
                cx = write_data[i].posX;
                cy = write_data[i].posY;
                cts = write_data[i].timestamp;
                nx = write_data[i + 1].posX;
                ny = write_data[i + 1].posY;
                nts = write_data[i + 1].timestamp;
                if (isShort(cx, cy, cts, nx, ny, nts)) short_read++; //短い読み返しをカウント
                if (isLong(cx, cy, cts, nx, ny, nts)) long_read++; //長い読み返しをカウント
            }
            Debug.Log("短い読み返し：" + short_read + ", 長い読み返し：" + long_read);
        }
    }

    /*
    //書き取りの軌跡を表示
	void display_write()
    {
        //画面サイズに合うようにデータを変換するためのリサイズ変数
        float resize_x = float(displayWidth) / 1920.0;
        float resize_y = float(displayHeight) / 1080.0;
        //定点サイズ
        int r = 5;
        noFill();
        beginShape();
        for (int i = 0; i < write_data.size(); i++)
        {
            vertex(write_data.get(i).posX * resize_x, write_data.get(i).posY * resize_y);
            ellipse(write_data.get(i).posX * resize_x, write_data.get(i).posY * resize_y, r, r);
        }
        endShape();
    }
	*/

    //書き取り条件の一致確認
    bool isWrite(float d, float px, float py)
    {
        //画面サイズに合うようにデータを変換するためのリサイズ変数
        float resize_x = (float)width / 1920.0f;
        float resize_y = (float)height / 1080.0f;
        //書き取り領域を定義
        float[] area_x = { ((1920.0f * 1.0f) / 3.0f) * resize_x, ((1920.0f * 2.0f) / 3.0f) * resize_x };
        float[] area_y = { 150, 500 };
        //読み取りの角度範囲を定義
        float[] d_range = { 5.0f, -5.0f };
        int conditions = 6; //条件の数
        int match_num = 0; //条件一致の個数
                           //角度
        if (d < d_range[0]) match_num += 1;
        if (d > d_range[1]) match_num += 1;
        //範囲
        if ((px * resize_x) > area_x[0]) match_num += 1;
        if ((px * resize_x) < area_x[1]) match_num += 1;
        if ((py * resize_y) > area_y[0]) match_num += 1;
        if ((py * resize_y) < area_y[1]) match_num += 1;

        //条件全てを満たした場合にtrueを返す
        if (match_num == conditions) return true;

        return false;
    }

    //短い読み返しの条件一致
    bool isShort(float cx, float cy, float cts, float nx, float ny, float nts)
    {
        float TEXT_SIZE = 14.0f; //文字サイズ
        int conditions = 3; //条件の数
        int match_num = 0; //条件一致の個数
        int COL = 7; //同じ行での距離条件
        int ROW = 3; //上下での移動距離条件
        int TIME = 3; //行動の発生間隔に関する時間条件

        //x座標条件
        if (cx - nx > TEXT_SIZE * COL) match_num++;
        //y座標条件
        if (ny - cy < TEXT_SIZE * ROW) match_num++;
        //時間条件
        if (nts - cts > TIME) match_num++;

        //条件全てを満たした場合にtrueを返す
        if (match_num == conditions) return true;

        return false;
    }

    //長い読み返しの条件一致
    bool isLong(float cx, float cy, float cts, float nx, float ny, float nts)
    {
        float TEXT_SIZE = 14.0f; //文字サイズ
        int conditions = 3; //条件の数
        int match_num = 0; //条件一致の個数
        int COL = 5; //同じ行での距離条件
        int ROW = 3; //上下での移動距離条件
        int TIME = 3; //行動の発生間隔に関する時間条件

        //x座標条件
        if (cx - nx > TEXT_SIZE * COL) match_num++;
        //y座標条件
        if (ny - cy > TEXT_SIZE * ROW) match_num++;
        //時間条件
        if (nts - cts > TIME) match_num++;

        //条件全てを満たした場合にtrueを返す
        if (match_num == conditions) return true;

        return false;
    }
}
