using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeEnviron : SupportType {

	public Canvas EffectCanvas;

	const int OBJ_NUM = 8;

	// Use this for initialization
	void Start () {
		set_object();
	}
	
	// Update is called once per frame
	void Update () {
		
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
				}
				else if(i==1){
					prefab.GetComponent<RectTransform>().localPosition = new Vector3(posX * (-1.0f), posY * j - posY * (OBJ_NUM / 2.0f) + posY / 2.0f, 0.0f);
				}
			}
		}
	}
}