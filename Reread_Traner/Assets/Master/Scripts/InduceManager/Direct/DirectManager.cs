using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectManager : MonoBehaviour {

	public Canvas DirectCanvas; //実際に操作するCanvas
	public GameObject Element; //実際に誘導を行う物体
	public int Row; //フィルタする行目
	public int Num; //フィルタを行う行数
	public int FilerType; //フィルタのタイプ
	private GameObject[] Elements; //フィルタを格納する変数
	private float FontSize = 14.0F; //フォントサイズ <- あとで参照するようにする

	// Use this for initialization
	void Start () {
		SetElement();
	}
	
	// Update is called once per frame
	void Update () {
		FilterManage(FilerType, Row, Num);
	}

	//フィルタを配置
	void SetElement(){
		float width = (float)Screen.width; //実機の横幅
		float height = (float)Screen.height; //実機の縦幅
		float filter_w = ( width * 2.0F ) / 3.0F; //フィルタの横幅
		int rows = (int)height / (int)FontSize; //全体の行数
		for( int i=0; i<rows; i++ ){
			GameObject prefab = (GameObject)Instantiate(Element); //prefabにElementを指定してインスタンス化
			prefab.transform.SetParent(DirectCanvas.transform, false); //親を直接誘導用のCanvasに指定
			prefab.GetComponent<RectTransform>().localPosition = new Vector3( 0.0F, i * FontSize - height / 2.0F, 0.0F ); //配置
			prefab.name = "row_" + i;
		}

		GetElement(rows);
	}

	void GetElement(int num){
		Elements = GameObject.FindGameObjectsWithTag("Direct");
	}

	void FilterManage(int type, int row, int num){
		if( type == 0 ){
			//通常
			for( int i=0; i<num; i++ ){
				if( num != -1 ){
					Elements[ ( Elements.Length - 1 ) - ( row + i ) ].SetActive(false);
				}else{
					for( int j=0; j<Elements.Length; j++ ){
						Elements[j].SetActive(true);
					}
				}
			}
		}else{

		}
	}
}
