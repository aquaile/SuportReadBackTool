using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class OptionManager : MonoBehaviour {
	GameObject setup;
	public int TimeRange;
	public int InduceType;
	public int CorrectionLevel;
	public GameObject TimeRageSlider;
	public GameObject CorrectionLevelSlider;
	public GameObject Content;
	public GameObject OptionPanel;
	Button Quitbtn;

	// Use this for initialization
	void Start () {
		init();
	}

	// Update is called once per frame
	void Update () {
	}

	void init(){
		setup = GameObject.FindGameObjectsWithTag("setup")[0];
		Quitbtn = OptionPanel.transform.Find("Quit").GetComponent<Button>();

		ChangeSetupData();

		Quitbtn.onClick.AddListener(OnClicked);
		TimeRageSlider.GetComponent<Slider>().onValueChanged.AddListener (delegate { TimeRangeChange ();});
		CorrectionLevelSlider.GetComponent<Slider>().onValueChanged.AddListener (delegate { CorrectionLevelChange ();});
		Content.GetComponent<ToggleGroup>().ActiveToggles().FirstOrDefault().onValueChanged.AddListener (delegate { InduceTypeChange ();});
	}

	void ChangeSetupData(){
		setup.GetComponent<Setting>().setting.TimeRange = TimeRange;
		setup.GetComponent<Setting>().setting.InduceType = InduceType;
		setup.GetComponent<Setting>().setting.CorrectionLevel = CorrectionLevel;
	}

	public void TimeRangeChange(){
		TimeRange = (int)TimeRageSlider.GetComponent<Slider>().value;
		ChangeSetupData();
	}

	public void CorrectionLevelChange(){
		CorrectionLevel = (int)CorrectionLevelSlider.GetComponent<Slider>().value;
		ChangeSetupData();
	}

	public void InduceTypeChange(){
		bool isOn = Content.GetComponent<ToggleGroup>().AnyTogglesOn();
		if( isOn ){
			Toggle load = Content.GetComponent<ToggleGroup>().ActiveToggles().FirstOrDefault();
			string name = load.transform.Find("Label").GetComponent<Text>().text;
			if(name == "Indirect"){
				InduceType = 1;
			}else{
				InduceType = 0;
			}
		}
		ChangeSetupData();
	}

	void OnClicked(){
		Save();
	}

	void Save(){
		string FilePath = Application.dataPath + "/StreamingAssets/Setting/setting.json";
		string JSON = JsonUtility.ToJson(setup.GetComponent<Setting>().setting);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream file = File.Create(FilePath);
		StreamWriter ws = new StreamWriter(file);
		ws.WriteLine(JSON);
		ws.Flush();
		//binaryFormatter.Serialize(file, JSON);
		file.Close();	
	}
}
