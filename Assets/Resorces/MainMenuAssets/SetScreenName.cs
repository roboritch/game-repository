using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Xml.Serialization;
using System;
using System.IO;

[RequireComponent(typeof(InputField))]
public class SetScreenName : MonoBehaviour{
	[SerializeField] private GameObject screenNameEditor;
	private ScreenName screenName;
	private string screenNameFileLocation;
	private InputField userInputField;

	void Awake(){
		userInputField = GetComponent<InputField>();
		screenNameFileLocation = Application.dataPath + "/screenName.xml";
		if(!File.Exists(screenNameFileLocation)){ //update as new options are added
			screenName.name = "";
			saveCurrentName();
		}
	}

	void Start(){
		loadCurrentName();
	}

	public void receveName(string name){
		screenName.name = name.ToUpper();
		saveCurrentName();
		updatePlayerName();
	}

	public void updatePlayerName(){
		userInputField.text = screenName.name;
		Player.Instance.setPlayerName(screenName.name);
	}

	#region save and load
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
			//throw ex;
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
			Debug.LogWarning("name Load Error");
			return;
		}
		updatePlayerName();
	}
	#endregion
}


[Serializable]
[XmlRoot("screen name")]
public struct ScreenName{
	[XmlElement("name")]
	public string name;
}