using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeEnviron : SupportType {

	public Canvas EffectCanvas;

	const int OBJ_NUM = 8;

	//視線誘導に関連する変数及び定数の定義
	GameObject[,] effects; //視線誘導因子群
	const float EFFECT_VELOCITY = 5.0f; //視線誘導アニメーションの速度

	bool[] isActive;

	// Use this for initialization
	void Start () {
		init();
	}
	
	// Update is called once per frame
	void Update () {
		mode_rotate(0, 0);
		mode_signal(1, 7);
	}

	//初期化
	void init(){
		set_object();
		get_object();
	}

	//視線誘導に用いるオブジェクトを配置
	void set_object(){
		float width = (float)Screen.width; //実機の横幅
		float height = (float)Screen.height; //実機の縦幅
		float posX = (width * 5.0f)/12.0f; //振動体の横位置
		float posY = height / (float)OBJ_NUM; //振動体の高さの間隔

		for(int i=0; i<2; i++){
			for(int j=0; j<OBJ_NUM; j++){
				GameObject effect = (GameObject)Resources.Load("Prefabs/Effect/EnvironEffectImage");
				GameObject prefab = (GameObject)Instantiate(effect);
				prefab.transform.SetParent(EffectCanvas.transform, false);
				if(i==0){
					prefab.GetComponent<RectTransform>().localPosition = new Vector3(posX, posY * j - posY * (OBJ_NUM / 2.0f) + posY / 2.0f, 0.0f);
					prefab.name = "Effect_" + i + "_" + j;
				}
				else if(i==1){
					prefab.GetComponent<RectTransform>().localPosition = new Vector3(posX * (-1.0f), posY * j - posY * (OBJ_NUM / 2.0f) + posY / 2.0f, 0.0f);
					prefab.name = "Effect_" + i + "_" + j;
				}
			}
		}
	}

	//視線誘導因子を取得する
	void get_object(){
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Construct");
		effects = trans_dimensions(2, temp);
	}

	//回転アニメーションによる視線誘導
	void mode_rotate(int col, int row){
		effects[col,row].transform.Rotate(0.0f, 0.0f, EFFECT_VELOCITY);
	}

	//点滅アニメーションによる視線誘導
	void mode_signal(int col, int row){
		effects[col,row].SetActive(false);
	}

	GameObject[,] trans_dimensions(int n, GameObject[] obj){
		int row = OBJ_NUM;
		GameObject[,] temp = new GameObject[n,row];
		for(int i=0; i<n; i++){
			for(int j=0; j<row; j++){
				temp[i,j] = obj[i * row + j];
			}
		}
		return temp;
	}
}