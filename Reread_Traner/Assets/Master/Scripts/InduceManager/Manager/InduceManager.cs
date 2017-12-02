/*

読み返しの回数と文章データを受け取り、読み返し誘導の有無を制御するクラス

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InduceManager : MonoBehaviour {

	public Canvas DirectCanvas; //直接誘導用のCanvas
	public Canvas IndirectCanvas; //間接誘導用のCanvas
	public GameObject SystemManager; //文章作成用のCanvas
	float timelapse; //経過時間
	private int INTERVAL = 55 + 5; //読み返し誘導のインターバル（ インターバル + 誘導の動作時間 ）
	public int reread_type = 0; //誘導する読み返しのタイプ
	public int induce_type = 0; //読み返し誘導のタイプ
	public int Short;
	public int Long;
	public int SentenceCount;
	public Vector2 EditPosition;
	public int Row; //読み返しを誘導する行目
	public int Col; //読み返しを誘導する列目
	public int Num; //読み返しを誘導する行数
	int FontSize = 14; //フォントサイズ
	int Width; //実機の横幅
	bool isInduce = false; //判定を60秒に入った1回目だけ行う
	bool isSettled = false; //判定を60秒に入った1回目だけ行う
	GameObject timer; //一箇所で管理されている時間を格納するための変数

	// Use this for initialization
	void Start () {
		Width = (int)Screen.width;
		timer = GameObject.FindGameObjectsWithTag("timer")[0];
	}
	
	// Update is called once per frame
	void Update () {
		timelapse = timer.GetComponent<TimeKeeper>().timelapse;
		//一定時間ごとに読み返しの制御を発火　<-　今後、編集状況に反応できるように修正する
		if( (int)timelapse % INTERVAL == 0 ){
			if( !isInduce ){
				Debug.Log( "Induce" );
				reread_type = collateInduce( Short, Long, SentenceCount, timelapse );
				actInduce( /* reread_type */ 2 , induce_type, EditPosition );
				isInduce = true;
			}
		}else if( (int)timelapse % ( INTERVAL ) == 5 ){
			if( !isSettled ){
				Debug.Log( "Settled" );
				Format();
				isSettled = true;
			}
		}else{
			isInduce = false;
			isSettled = false;
		}
	}

	//読み返し誘導の条件判定（短い読み返し, 長い読み返し, 文章数, 経過時間）
	int collateInduce(int short_reread, int long_reread, int sentence_count, float timelapse){
		float short_coefficient = 0.0F; //モデル化した短い読み返しの傾向から読み返しの有無を計算する式の係数
		float long_coefficient = 0.0F; //モデル化した長い読み返しの傾向から読み返しの有無を計算する式の係数
		int ideal_short_reread = ( int )( short_coefficient * timelapse ); //経過時間から理想的な短い読み返しの回数を計算
		//int ideal_short_reread = ( int )( short_coefficient * sentence_count ); //文章数から理想的な短い読み返しの回数を計算
		int ideal_long_reread = ( int )( long_coefficient * timelapse ); //経過時間から理想的な長い読み返しの回数を計算
		//int ideal_long_reread = ( int )( long_coefficient * sentence_count ); //文章数から理想的な長い読み返しの回数を計算
		if( long_reread < ideal_long_reread ){ return 2; } //長い読み返しの回数が理想値より少なかった場合に視線誘導
		else if( short_reread < ideal_short_reread ){ return 1; } //短い読み返しの回数が理想値より少なかった場合に視線誘導
		else { return 0; } //短い読み返しと長い読み返しが共に理想値を超えていた場合に視線誘導を行わない
	}

	//条件を元に読み返し誘導制御する
	void actInduce(int reread_type, int induce_type, Vector2 position){
		//長い読み返し
		if( reread_type == 2 ){
			//直接的誘導
			if( induce_type == 0 ){
				Row = 7;
				Num = 2;
				DirectCanvas.GetComponent<DirectManager>().Row = Row;
				DirectCanvas.GetComponent<DirectManager>().Num = Num;
			}
			//間接的誘導
			else if( induce_type == 1 ){
				Row = 7;
				Col = 1;
				IndirectCanvas.GetComponent<IndirectManager>().Row = Row;
				IndirectCanvas.GetComponent<IndirectManager>().Col = Col;
			}
		}
		//短い読み返し
		else if( reread_type == 1 ){
			//直接的誘導
			if( induce_type == 0 ){
				Row = (int)position.y % FontSize;
				Num = 1;
				DirectCanvas.GetComponent<DirectManager>().Row = Row;
				DirectCanvas.GetComponent<DirectManager>().Num = Num;
			}
			//間接的誘導
			else if( induce_type == 1 ){
				Row = (int)position.y % FontSize;
				Col = ( position.x > Width / 2 ) ? 0 : 1;
				IndirectCanvas.GetComponent<IndirectManager>().Row = Row;
				IndirectCanvas.GetComponent<IndirectManager>().Col = Col;
			}
		}
		//何もしない
		else if( reread_type == 0 ){
			//何もしない
		}
	}

	//指定を全て初期化する関数
	void Format(){
		Row = -1;
		Col = -1;
		Num = -1;
		DirectCanvas.GetComponent<DirectManager>().Row = Row;
		DirectCanvas.GetComponent<DirectManager>().Num = Num;
		IndirectCanvas.GetComponent<IndirectManager>().Row = Row;
		IndirectCanvas.GetComponent<IndirectManager>().Col = Col;
	}
}
