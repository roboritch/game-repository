using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Xml;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Dropdown))]
public class FindAllLevels : MonoBehaviour{

	void Start(){
		dropdown = GetComponent<Dropdown>();
		getLevelNames();



	}

	public void submitlevelPath(){
		Player.Instance.dataPathOfLevelTheUserWantsToLoad = Application.dataPath + "/" + levelFolderName + "/" + dropdown.captionText.text + ".xml";
	}

	private Dropdown dropdown;
	#pragma warning disable
	[SerializeField] private string levelFolderName;

	private void getLevelNames(){
		XmlDocument levelxml = new XmlDocument();
		DirectoryInfo dI = new DirectoryInfo(Application.dataPath + "/" + levelFolderName);
		if(!dI.Exists){
			Debug.LogWarning("level derectory does not exist");
			return;
		}
		FileInfo[] levelFiles = dI.GetFiles();
		List<string> list = new List<string>();
		foreach( var item in levelFiles ){
			if(item.Extension == ".xml"){
				list.Add(item.Name); // add all level names to list
			}
		}
		dropdown.AddOptions(list); // add names to list
	}

}
