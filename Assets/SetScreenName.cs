using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System;
using System.IO;

public class SetScreenName : MonoBehaviour {
	private ScreenName screenName;
	private string optionsFileLocation;

	void Awake(){
		optionsFileLocation = Application.dataPath + "/screenName.xml";
		if(!File.Exists(optionsFileLocation)){ //update as new options are added
			screenName.name = "";
			saveCurrentName();
		}
		loadCurrentName();
	}


	public void saveCurrentName(){
		FileStream stream = null;
		try {
			XmlSerializer serializer = new XmlSerializer(typeof(ScreenName));
			stream = new FileStream(optionsFileLocation, FileMode.Truncate);
			serializer.Serialize(stream, screenName);
			stream.Close();
		} catch(Exception ex) {
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
		try {
			XmlSerializer serializer = new XmlSerializer(typeof(ScreenName));
			stream = new FileStream(optionsFileLocation, FileMode.Open);
			ScreenName container = (ScreenName)serializer.Deserialize(stream);
			screenName = container;
			stream.Close();
		}catch(Exception ex){
			if(stream != null)
			stream.Close();
			Debug.LogError("error in stream load");
		}
	}
}


[Serializable]
[XmlRoot("screen name")]
struct ScreenName{
	[XmlElementAttribute("name")]
	public string name;
}