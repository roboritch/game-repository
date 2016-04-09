using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System;
using System.IO;
using UnityEngine.UI;

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

	/// <summary>
	/// Saves the game 
	/// </summary>
	public void submitText(){
		newLevelName(GetComponent<InputField>().text);
		// Should save submission of new level
		levelCall();
	}

	public void loadLevel(){
		gscript.loadGrid(Application.dataPath + "/levels" + "/" + GetComponent<InputField>().text + ".xml");
	}

}


[Serializable]
[XmlRoot("LevelName")]
public struct LevelName{
	[XmlElement("name")]
	public string name;
}