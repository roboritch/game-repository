using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System;
using System.IO;

public class SetScreenName : MonoBehaviour{
	[SerializeField] private GameObject screenNameEditor;
	private ScreenName screenName;
	private string screenNameFileLocation;

	void Awake(){
		screenNameFileLocation = Application.dataPath + "/screenName.xml";
		if(!File.Exists(screenNameFileLocation)){ //update as new options are added
			screenName.name = "";
			saveCurrentName();
		}
		loadCurrentName();
	}

	void start(){
		if(screenName.name == ""){
			bringUpScreenNameEditer();
		}
	}

	public void bringUpScreenNameEditer(){
		GameObject temp = Instantiate(screenNameEditor) as GameObject;
		temp.transform.SetParent(transform, false);


	}

	public void receveName(string name){
		saveCurrentName();
	}


	public void saveCurrentName(){
		FileStream stream = null;
		try{
			XmlSerializer serializer = new XmlSerializer(typeof(ScreenName));
			stream = new FileStream(screenNameFileLocation, FileMode.OpenOrCreate);
			serializer.Serialize(stream, screenName);
			stream.Close();
		} catch(Exception ex){
			if(stream != null)
				stream.Close();
		}
	}

	/// <summary>
	/// Loads the currentOptions from disk.
	/// </summary>
	public void loadCurrentName(){
		FileStream stream = null;
		try{
			XmlSerializer serializer = new XmlSerializer(typeof(ScreenName));
			stream = new FileStream(screenNameFileLocation, FileMode.Open);
			ScreenName container = (ScreenName)serializer.Deserialize(stream);
			screenName = container;
			stream.Close();
		} catch(Exception ex){
			if(stream != null)
				stream.Close();
		}
	}
}


[Serializable]
[XmlRoot("screen name")]
public struct ScreenName{
	[XmlElement("name")]
	public string name;
}
