/*

読み返しの回数と文章データを受け取り、読み返し誘導の有無を制御するクラス

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InduceManager : MonoBehaviour {

	Canvas DirectCanvas; //直接誘導用のCanvas
	Canvas IndirectCanvas; //間接誘導用のCanvas
	Canvas EditorCanvas; //文章作成用のCanvas
	private float timelapse; //経過時間
	private int INTERVAL = 30 + 5; //読み返し誘導のインターバル（ インターバル + 誘導の動作時間 ）
	public int reread_type = 0; //誘導する読み返しのタイプ
	public int induce_type = 0; //読み返し誘導のタイプ

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timelapse += Time.deltaTime; //経過時間を計算する
		//一定時間ごとに読み返しの制御を発火　<-　今後、編集状況に反応できるように修正する
		if( (int)timelapse % INTERVAL == 0 ){
			
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
		else if( short_reread < ideal_short_reread ){ return 2; } //短い読み返しの回数が理想値より少なかった場合に視線誘導
		else { return 0; } //短い読み返しと長い読み返しが共に理想値を超えていた場合に視線誘導を行わない
	}

	//条件を元に読み返し誘導制御する
	void actInduce(int reread_type, int induce_type){
		
	}
}
