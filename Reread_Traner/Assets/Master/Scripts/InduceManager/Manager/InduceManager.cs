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
	private int INTERVAL = 25; //読み返し誘導のインターバル（ インターバル + 誘導の動作時間 ）
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
	int Height; //実機の横幅
	bool isInduce = false; //判定を60秒に入った1回目だけ行う
	bool isSettled = false; //判定を60秒に入った1回目だけ行う
	public int SettledInterval = 5;
	GameObject timer; //一箇所で管理されている時間を格納するための変数
	public float baseTimeS = 0.0F;
	public float baseTimeL = 0.0F;
	public int baseS;
	public int baseL;

	void Awake(){
		Format();
	}

	// Use this for initialization
	void Start () {
		Width = (int)Screen.width;
		Height = (int)Screen.height;
		timer = GameObject.FindGameObjectsWithTag("timer")[0];
	}
	
	// Update is called once per frame
	void Update () {
		SettledInterval = GameObject.FindGameObjectsWithTag("setup")[0].GetComponent<Setting>().setting.TimeRange;
		induce_type = GameObject.FindGameObjectsWithTag("setup")[0].GetComponent<Setting>().setting.InduceType;
		timelapse = timer.GetComponent<TimeKeeper>().timelapse;
		//一定時間ごとに読み返しの制御を発火　<-　今後、編集状況に反応できるように修正する
		if( (int)timelapse % ( INTERVAL + SettledInterval ) == 0 && ( int ) timelapse != 0 ){
			if( !isInduce ){
				Debug.Log( "Induce" );
				reread_type = collateInduce( Short, Long, SentenceCount, timelapse );
				Debug.Log( "読み返しパターン：" + reread_type );
				actInduce( reread_type , induce_type, EditPosition );
				isInduce = true;
			}
		}else if( (int)timelapse % ( INTERVAL + SettledInterval ) == SettledInterval && ( int ) timelapse != SettledInterval){
			if( !isSettled ){
				Debug.Log( "Settled" );
				IndirectCanvas.GetComponent<IndirectManager>().FormatObj(Col, Row);
				IndirectCanvas.GetComponent<IndirectManager>().FormatObj( 1, 7 );
				Format();
				isSettled = true;
			}
		}else{
			isInduce = false;
			isSettled = false;
		}
		baseTimeS += Time.deltaTime;
		baseTimeL += Time.deltaTime;
	}

	//読み返し誘導の条件判定（短い読み返し, 長い読み返し, 文章数, 経過時間）
	int collateInduce(int short_reread, int long_reread, int sentence_count, float timelapse){
		int S  = short_reread - baseS;
		int L  = long_reread - baseL;
		if( S >= 1 ){
			baseTimeS = 0.0F;
			baseS = short_reread;
		}else{
			baseTimeS += Time.deltaTime;
		}

		if( L >= 1 ){
			baseTimeL = 0.0F;
			baseL = long_reread;
		}else{
			baseTimeL += Time.deltaTime;
		}

		//int x = ( int )( timelapse / INTERVAL ) ;
		float x = ( timelapse % ( 30.0F * 60.0F ) ) / 30.0F;

		//y = -0.015x^2 + 1.700x - 2.350
		//y = 0.0002x3 - 0.0361x2 + 2.5211x - 4.7187
		//y = 0.0006x2 - 0.0722x + 2.5211
		//int ideal_short_reread = ( int )( -0.015F * x * x + 1.700F * x - 2.350F ); //経過時間から理想的な短い読み返しの回数を計算
		float ideal_short_reread =  0.0006F * x * x + 0.0722F * x + 2.5211F ; //経過時間から理想的な短い読み返しの回数を計算
		float strengthS = baseTimeS *  ideal_short_reread / 30.0F;
		//int ideal_short_reread = ( int )( short_coefficient * sentence_count ); //文章数から理想的な短い読み返しの回数を計算
		//y = 0.003x^2 + 0.100x - 1.000
		//y = -0.0003x3 + 0.0265x2 - 0.341x + 1.0587
		//y = -0.0009x2 + 0.0530x - 0.341
		//int ideal_long_reread = ( int )( -0.003F * x * x + 0.100F * x - 1.000F ); //経過時間から理想的な長い読み返しの回数を計算
		float ideal_long_reread =  ( float )( -0.0009F * x * x + 0.0530F * x  - 0.341F ); //経過時間から理想的な長い読み返しの回数を計算
		float strengthL = baseTimeL *  ideal_long_reread / 7.5F;
		//int ideal_long_reread = ( int )( long_coefficient * sentence_count ); //文章数から理想的な長い読み返しの回数を計算
		Debug.Log("長い読み返し：" + ideal_long_reread );
		Debug.Log("短い読み返し：" + ideal_short_reread);
		//if( long_reread < ideal_long_reread ){ return 2; } //長い読み返しの回数が理想値より少なかった場合に視線誘導
		//else if( short_reread < ideal_short_reread ){ return 1; } //短い読み返しの回数が理想値より少なかった場合に視線誘導
		//else { return 0; } //短い読み返しと長い読み返しが共に理想値を超えていた場合に視線誘導を行わない
		if( strengthL > 1 && strengthL > strengthS){ return 2; } //長い読み返しの回数が理想値より少なかった場合に視線誘導
		else if( strengthS > 1 && strengthS > strengthL){ return 1; } //短い読み返しの回数が理想値より少なかった場合に視線誘導
		else { return 0; } //短い読み返しと長い読み返しが共に理想値を超えていた場合に視線誘導を行わない
	}

	//条件を元に読み返し誘導制御する
	void actInduce(int reread_type, int induce_type, Vector2 position){
		//長い読み返し
		if( reread_type == 2 ){
			//直接的誘導
			if( induce_type == 0 ){
				Row = 0;
				Num = 5;
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
				Row = (int) position.y / FontSize;
				Num = 3;
				DirectCanvas.GetComponent<DirectManager>().Row = Row;
				DirectCanvas.GetComponent<DirectManager>().Num = Num;
			}
			//間接的誘導
			else if( induce_type == 1 ){
				Row = 7 - (int)position.y / (Height / 8);
				Col = ( position.x > Width / 2 - Width / 6 ) ? 1 : 0;
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
