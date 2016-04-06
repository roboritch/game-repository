using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System;
using System.IO;

public class SetLevelName : MonoBehaviour{
	public GridScript gscript;
	private string levelName;

	public void newLevelName(string levelName){
		gscript.setLevelName(levelName);
	}

	public void levelCall(){
		gscript.getGridInfo();
		gscript.saveGrid();
	}
}


[Serializable]
[XmlRoot("LevelName")]
public struct LevelName{
	[XmlElement("name")]
	public string name;
}