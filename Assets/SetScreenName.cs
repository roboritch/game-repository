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
			
		}
	}

	public void bringUpScreenNameEditer(){
		GameObject temp = Instantiate(screenNameEditor) as GameObject;


	}


	public void saveCurrentName(){
		FileStream stream = null;
		try{
			XmlSerializer serializer = new XmlSerializer(typeof(ScreenName));
			stream = new FileStream(screenNameFileLocation, FileMode.Truncate);
			serializer.Serialize(stream, screenName);
			stream.Close();
		} catch(Exception ex){
			Debug.LogError("error in stream save");
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
			Debug.LogError("error in stream load");
		}
	}
}


[Serializable]
[XmlRoot("screen name")]
struct ScreenName{
	[XmlElement("name")]
	public string name;
}