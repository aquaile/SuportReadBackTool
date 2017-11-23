using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadCSV{
	private TextAsset csv_file;
	List<string[]> data = new List<string[]>();
	int row = 0;
	
	public LoadCSV(string path){
		csv_file = Resources.Load( "TestDatas/Gaze/" + path ) as TextAsset;
		StringReader reader = new StringReader(csv_file.text);
		while(reader.Peek() > -1) {
			string line = reader.ReadLine();
			data.Add(line.Split(','));
			row++;
		}
	}

	public float getFloat(int row, int col){
		return float.Parse(data[row][col]);
	}

	public string getString(int row, int col){
		return data[row][col];
	}

	public int getRowCount(){
		return row;
	}
}
