using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector{
	List<GazeData> Data; //生の視線データを格納する変数
	Calcurator Calc; //注視点処理と属性計算を行うクラス
	List<GazeData> SettledData; //加工済みの属性を持った視線データを格納する変数
	List<GazeData> WritePoints; //書き取りor読み取りとして検出された視線データを格納する変数
	float Width; //全体領域の横幅
	float Height; //全体領域の縦幅
	int PIVOT = 0; //データ取得の開始点
	int Short = 0; //長い読み返し
	int Long = 0; //短い読み返し

	//コンストラクタ
	public Detector(){
		Data = new List<GazeData>();
		Calc = new Calcurator();
		Width = (float)Screen.width;
		Height = (float)Screen.height;
	}
	
	//新しい視線データを構造体に追加していく関数
	public void UpdateGazeData(float posX, float posY, float Timelapse){
		Data.Add(new GazeData(posX, posY, Timelapse));
	}

	//Calcuratorを用い、注視点処理と属性計算が行われた視線データを取得する
	List<GazeData> GetData(){
		List<GazeData> temp = new List<GazeData>();
		List<GazeData> result = new List<GazeData>();
		for( int i=PIVOT; i<Data.Count; i++ ){
			temp.Add( Data[i] );
		}
		PIVOT = Data.Count; //データ取得開始点を移動させる <- このやり方は改良の余地あり
		result = Calc.GetSettledData( temp );
		return result;
	}

    //全ての視線データを取得する
    public List<GazeData> GetDataList(){
        List<GazeData> temp = new List<GazeData>();
        for( int i=0; i<Data.Count; i++ ){
            temp.Add(Data[i]);
        }
        return temp;
    }

	//書き取り検出
	List<GazeData> DetectWrite(List<GazeData> data){
		List<GazeData> result = new List<GazeData>();
		for( int i=0; i<data.Count; i++ ){
			if( isWrite( data[i].posX, data[i].posY, data[i].degree ) ){
				result.Add( data[i] );
			}
		}
		return result;
	}

	//書き取り条件の一致確認
    bool isWrite(float posX, float posY, float d)
    {
        //書き取り領域を定義
        float[] area_x = { ((Width * 1.0F) / 3.0F), ((Width * 2.0F) / 3.0F) };
        float[] area_y = { 0.0F, Height };
        //読み取りの角度範囲を定義
        float[] d_range = { 5.0F, -5.0F };
        int conditions = 6; //条件の数
        int match_num = 0; //条件一致の個数
        //角度
        if (d < d_range[0]) match_num += 1;
        if (d > d_range[1]) match_num += 1;
        //範囲
        if ((posX) > area_x[0]) match_num += 1;
        if ((posX) < area_x[1]) match_num += 1;
        if ((posY) > area_y[0]) match_num += 1;
        if ((posY) < area_y[1]) match_num += 1;

        //条件全てを満たした場合にtrueを返す
        if (match_num == conditions) return true;

        return false;
    }

	//読み返し検出
	public int[] RereadCount(){
        Short = 0;
        Long = 0;
		List<GazeData> temp = GetData();
		List<GazeData> write = DetectWrite(temp);
		for( int i=0; i<write.Count-1; i++ ){
			if( isShort( write[i].posX, write[i].posY, write[i].timelapse, write[i+1].posX, write[i+1].posY, write[i+1].timelapse ) ) Short++;
			if( isLong( write[i].posX, write[i].posY, write[i].timelapse, write[i+1].posX, write[i+1].posY, write[i+1].timelapse ) ) Long++;
		}
		int[] result = { Short, Long };
		return result;
	}

	//短い読み返しの条件一致
    bool isShort(float x1, float y1, float t1, float x2, float y2, float t2)
    {
        float TEXT_SIZE = 14.0F; //文字サイズ
        int conditions = 3; //条件の数
        int match_num = 0; //条件一致の個数
        int COL = 7; //同じ行での距離条件
        int ROW = 3; //上下での移動距離条件
        float TIME = 0.5F; //行動の発生間隔に関する時間条件

        //x座標条件
        if (x1 - x2 > TEXT_SIZE * COL) match_num++;
        //y座標条件
        if (y2 - y1 < TEXT_SIZE * ROW) match_num++;
        //時間条件
        if (t2 - t1 > TIME) match_num++;

        //条件全てを満たした場合にtrueを返す
        if (match_num == conditions) return true;

        return false;
    }

    //長い読み返しの条件一致
    bool isLong(float x1, float y1, float t1, float x2, float y2, float t2)
    {
        float TEXT_SIZE = 14.0f; //文字サイズ
        int conditions = 3; //条件の数
        int match_num = 0; //条件一致の個数
        int COL = 5; //同じ行での距離条件
        int ROW = 3; //上下での移動距離条件
        float TIME = 1.0F; //行動の発生間隔に関する時間条件

        //x座標条件
        if (x1 - x2 > TEXT_SIZE * COL) match_num++;
        //y座標条件
        if (y2 - y1 > TEXT_SIZE * ROW) match_num++;
        //時間条件
        if (t2 - t1 > TIME) match_num++;

        //条件全てを満たした場合にtrueを返す
        if (match_num == conditions) return true;

        return false;
    }
}
