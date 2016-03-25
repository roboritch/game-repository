using UnityEngine;
using System.Collections;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

// http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer


public class OptionsScript : MonoBehaviour{
	
	private string optionsFileLocation;
	private OptionsInfo currentOptions;
	// options being used now
	public OptionsInfo newOptions;
	// options being set by user

	#region options UI script refrences
	public ReselutionSelect reselutionSelect;
	#endregion

	void Awake(){
		optionsFileLocation = Application.dataPath + "/options.xml";
		if(!File.Exists(optionsFileLocation)){ //update as new options are added
			newOptions.resolution = Screen.currentResolution;
			saveCurrentOptions();
		}

		loadCurrentOptions();
		newOptions = currentOptions; // this is not a refrence pass it is a new copy
	}

	#region save load and test
	public void saveCurrentOptions(){
		FileStream stream = null;
		try{
			XmlSerializer serializer = new XmlSerializer(typeof(OptionsInfo));
			stream = new FileStream(optionsFileLocation, FileMode.Create);
			serializer.Serialize(stream, newOptions); // new options to disk here
			stream.Close();
		} catch(Exception ex){
			Debug.LogError(ex.ToString());
			if(stream != null)
				stream.Close();
		}
		currentOptions = newOptions;
	}

	/// <summary>
	/// Loads the currentOptions from disk.
	/// </summary>
	public void loadCurrentOptions(){
		FileStream stream = null;
		try{
			XmlSerializer serializer = new XmlSerializer(typeof(OptionsInfo));
			stream = new FileStream(optionsFileLocation, FileMode.Open);
			OptionsInfo container = (OptionsInfo)serializer.Deserialize(stream);
			currentOptions = container;
			stream.Close();
			//}catch(Exception ex){
		} catch{
			if(stream != null)
				stream.Close();
			//Debug.LogError(ex.ToString());
		}
	}

	//update as new options are added
	/// <summary>
	/// Applies the new options.
	/// </summary>
	public void applyNewOptions(){
		Screen.SetResolution(newOptions.resolution.width, newOptions.resolution.height, true);
	}

	/// <summary>
	/// This will reload currentOptions if newOptions has
	/// </summary>
	public void revertToLastOptions(){
		newOptions = currentOptions;
		applyNewOptions();
	}
	#endregion

	#region options
	/// <summary>
	/// Sets the reselution in the temporary options.
	/// </summary>
	/// <param name="res">reselution</param>
	public void setReselution(Resolution res){
		newOptions.resolution = res;
	}
		
	#endregion

}