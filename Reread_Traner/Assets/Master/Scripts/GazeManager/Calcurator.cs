using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calcurator{
	List<GazeData> FixData; //注視点処理後の視線データを格納するリスト
	List<GazeData>　SettledData; //属性計算後の視線データを格納するリスト
	private float TEXT_SIZE = 14.0F; //フォントサイズ <- 参照するように直す
	private float FIXATION_DIST = 14.0F; //注視判定距離（一文字）
	private int FIXATION_NUM = 4; //注視判定時間

	//コンストラクタ
	public Calcurator(){	}

	//注視点処理を行った視線データを取得する
	private List<GazeData> GetFixData(List<GazeData> data){
		FixData = FixationSaccade(data);
		return FixData;
	}

	//注視点処理
	private List<GazeData> FixationSaccade(List<GazeData> data){
		List<GazeData> result = new List<GazeData>(); //結果保存用リスト
		List<GazeData> temp = new List<GazeData>(); //チェックしている点を格納するリスト
		float T1 = FIXATION_DIST; //minimun fixation
		float T2 = FIXATION_DIST * 2.0F; //large fixation
		//処理
		for( int i=0; i<data.Count; i+=FIXATION_NUM ){
			//連続する4点を格納する
			for( int j=0; j<temp.Count; j++ ){
				temp.Add(data[ i + j ]);
			}
			//minimun fixation 処理
			if( MinimunFixation(temp, T1) ){
				//large fixation 処理
				int count = 0; //確認済みデータの個数
				int consecutive = 0; //連続数
				while( consecutive < 3 ){
					if( LargeFixation( temp, data[i + FIXATION_NUM + count], T2 ) ){
						temp.Add(data[i + FIXATION_NUM + count]);
						count++;
						consecutive = 0; //条件を満たさない点が3点連続しなかった場合に値を初期化する
					}else{
						consecutive++;
						count++;
					}
				}
				Vector2 point = GetCentroid( temp );
				result.Add( new GazeData( point.x, point.y, temp[temp.Count-1].timelapse ) );
				i+=count;
			}else{
				for( int j=0; j<temp.Count; j++){
					result.Add( new GazeData( data[ i + j ].posX, data[ i + j ].posY, data[ i + j ].timelapse ) );
				}
			}
		}
		return result;
	}

	//4点全ての距離がT1以下の時にtrueを返す
	private bool MinimunFixation(List<GazeData> data, float t1){
		int isMinimun = 0; //0の時に条件一致、それ以外は条件不一致
		//全ての点の組み合わせで距離を求め、条件一致を確認する
		if( Dist( data[0].posX, data[0].posY, data[1].posX, data[1].posY ) > t1 ){ isMinimun++; }
		if( Dist( data[0].posX, data[0].posY, data[2].posX, data[2].posY ) > t1 ){ isMinimun++; }
		if( Dist( data[0].posX, data[0].posY, data[3].posX, data[3].posY ) > t1 ){ isMinimun++; }
		if( Dist( data[1].posX, data[1].posY, data[2].posX, data[2].posY ) > t1 ){ isMinimun++; }
		if( Dist( data[1].posX, data[1].posY, data[3].posX, data[3].posY ) > t1 ){ isMinimun++; }
		if( Dist( data[2].posX, data[2].posY, data[3].posX, data[3].posY ) > t1 ){ isMinimun++; }
		if( isMinimun == 0 ){ return true; } //条件に一致していた場合にtrueを返す
		return false;
	}

	//minimun判定された点との距離が全てT2以下の時にtrueを返す
	private bool LargeFixation(List<GazeData> data, GazeData point, float t2){
		int isLarge = 0; //0の時に条件一致、それ以外は条件不一致
		//全てのminimunと比較
		for( int i=0; i<data.Count; i++ ){
			if( Dist( data[i].posX, data[i].posY, point.posX, point.posY ) > t2 ){ isLarge++; }
		}
		if( isLarge == 0 ){ return true; } //条件に一致していた場合にtrueを返す
		return false;
	}

	//視線データリストから重心を求める
	private Vector2 GetCentroid(List<GazeData> data){
		Vector2 result = new Vector2(0.0F, 0.0F); //結果保存用
		float posX = 0.0F;
		float posY = 0.0F;
		for( int i=0; i<data.Count; i++ ){
			posX += data[i].posX;
			posY += data[i].posY;
		}
		result = new Vector2( posX / data.Count, posY / data.Count );
		return result;
	}


	//属性計算を行った視線データを取得する
	public List<GazeData> GetSettledData(List<GazeData> data){
		FixData = GetFixData(data);
		SettledData = Calcurate(FixData);
		return SettledData;
	}

	//属性計算処理
	private List<GazeData> Calcurate(List<GazeData> data){
		List<GazeData> result = new List<GazeData>();
		for( int i=0; i <data.Count-1; i++ ){
			float Norm = GetNorm( data[i].posX, data[i].posY, data[ i + 1 ].posX, data[ i + 1 ].posY );
			float Velocity = GetVelocity( Norm, data[i].timelapse, data[ i + 1 ].timelapse );
			float Degree = GetDegree( data[i].posX, data[i].posY, data[ i + 1 ].posX, data[ i + 1 ].posY );
			SettledData.Add( new GazeData( data[i].posX, data[i].posY, data[i].timelapse, Degree, Norm, Velocity ) );
		}
		return result;
	}

	//2点間での移動量を取得する
	private float GetNorm(float x1, float y1, float x2, float y2){
		float result = Dist(x1, y1, x2, y2);
		return result;
	}

	//2点間での移動速度を取得する
	private float GetVelocity(float norm, float t1, float t2){
		float result = norm / (Mathf.Abs(t1 - t2) * 100.0F);
		return result;
	}

	//2点で構成された直線の移動方向を取得する
	private float GetDegree(float x1, float y1, float x2, float y2){
		float vx = ( x1 - x2 ) / Dist(x1, y1, x2, y2);
		float vy = ( y1 - y2 ) / Dist(x1, y1, x2, y2);

		float base_x = 0.0F;
		float base_y = 1.0F;

		float result;
		if( vx < 0 ){
			result = Mathf.Acos(base_x * vy + vx * base_y) * (180 / Mathf.PI) - 180;
		}else{
			result = Mathf.Acos(base_x * vy + vx * base_y) * (180 / Mathf.PI);
		}
		return result;
	}

	//2点間の距離を求める関数
	private float Dist(float x1, float y1, float x2, float y2){
		float Dist = Mathf.Sqrt( Mathf.Pow(x2 - x1, 2 ) + Mathf.Pow( y2 - y1, 2 ) );
		return Dist;
	}
}
