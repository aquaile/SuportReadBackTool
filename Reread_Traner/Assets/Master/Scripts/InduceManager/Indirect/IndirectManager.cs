using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndirectManager : MonoBehaviour {

	public Canvas IndirectCanvas; //実際に操作するCanvas
	public int NUM = 8; //視線誘導を行う物体の数
	public GameObject Element; //視線誘導を行う物体
	private GameObject[,] Elements; //視線誘導を行う物体を格納する変数
	public float VELOCITY = 5.0F; //視線誘導アニメーションの速度
	private bool[] isActive; //視線誘導の点滅を制御
	public int MotionType = 1; //誘導の種類
	public int Col; //誘導を行う誘導体の列
	public int Row; //誘導を行う誘導体の行

	// Use this for initialization
	void Start () {
		SetElement(NUM);
	}
	
	// Update is called once per frame
	void Update () {
		MotionManage(MotionType, Col, Row); //誘導の管理
	}

	//視線誘導に用いるオブジェクトを配置
	void SetElement(int num){
		float width = (float)Screen.width; //実機の横幅
		float height = (float)Screen.height; //実機の縦幅
		float posX = ( width * 5.0F ) / 12.0F; //誘導体のx座標
		float posY = height / (float)num; //誘導体のy座標

		//2 * num でオブジェクトを配置
		for( int col=0; col<2; col++ ){
			for( int i=0; i<num; i++ ){
				GameObject prefab = (GameObject)Instantiate(Element); //prefabにElementを指定してインスタンス化
				prefab.transform.SetParent(IndirectCanvas.transform, false); //親を直接誘導用のCanvasに指定
				if( col == 0 ){
					//左側
					prefab.GetComponent<RectTransform>().localPosition = new Vector3( posX, posY * i - posY * ( num / 2.0F ) + posY / 2.0F, 0.0F ); //配置
					prefab.name = "Element_" + col + "_" + i;
				}
				else if( col == 1 ){
					//右側
					prefab.GetComponent<RectTransform>().localPosition = new Vector3( posX * ( -1.0F ), posY * i - posY * ( num / 2.0F ) + posY / 2.0F, 0.0F ); //配置
					prefab.name = "Element_" + col + "_" + i;
				}
			}
		}

		GetElement(num);
	}

	//視線誘導用の物体を取得する
	void GetElement(int num){
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Indirect");
		Elements = TransDimension(2, temp, num); //二次元配列に置換
	}

	//一次元配列を二次元配列に変換
	GameObject[,] TransDimension(int n, GameObject[] Elements, int num){
		GameObject[,] temp = new GameObject[n, num]; //結果を返すゲームオブジェクトの二次元配列
		for( int i=0; i<n; i++ ){
			for( int j=0; j<num; j++ ){
				temp[ i, j ] = Elements[ i * num + j ];
				temp[ i, j ].SetActive(false);
			}
		}
		return temp;
	}

	//視線誘導の動作を制御
	void MotionManage(int type, int col, int row){
		if( type == 0 ){
			//回転
			if( col != -1 && row != -1 ){
				Rotate(col, row);
			}
		}
		else if( type == 1 ){
			//点滅
			if( col != -1 && row != -1 ){
				Flush(col, row);
			}
		}
	}

	//回転誘導
	void Rotate(int col, int row){
		Elements[ col, row ].SetActive(true);
		Elements[ col, row ].transform.Rotate( 0.0F, 0.0F, VELOCITY); //回転させる
	}

	public void FormatObj(int col, int row){
		if( col != -1 && row != -1 ){
			Elements[ col, row ].SetActive(false);
		}
	}

	//点滅誘導
	void Flush(int col, int row){
		bool Flag = Elements[ col, row ].activeSelf;
		int INTERVAL = 20;
		//一定時間で点滅させる　<- 今後修正する
		if( Time.frameCount % INTERVAL == 0 ){
			Elements[ col, row ].SetActive(!Flag);
		}
	}

}
