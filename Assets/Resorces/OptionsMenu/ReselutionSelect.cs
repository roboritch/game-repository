using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ReselutionSelect : MonoBehaviour {
	public OptionsScript optionsMain;
	public Dropdown reselutionSelect;

	private Resolution[] res;

	// Use this for initialization
	void Start () {
		reselutionSelect.ClearOptions();
		res = Screen.resolutions;
		List<string> resOptions = new List<string>();
		foreach(Resolution item in res) {
			resOptions.Add(getResText(item));
		}
		reselutionSelect.AddOptions(resOptions);
		reselutionSelect.RefreshShownValue();
		displayResolutionInMenu(Screen.currentResolution);
		reselutionSelect.RefreshShownValue();
	
		reselutionSelect.onValueChanged.AddListener(delegate {
			this.setRes(); // call this option is changed
		});
	}

	private void setRes(){
		optionsMain.setReselution(res[reselutionSelect.value]);
	}

	//TODO not working
	/// <summary>
	/// Displaies the resolution in menu.
	/// </summary>
	/// <param name="res">Res.</param>
	public void displayResolutionInMenu(Resolution res){
		int emergencyBreak = 0;
		while(reselutionSelect.captionText.text.Equals(optionsMain.newOptions.resolution)){
			reselutionSelect.value = reselutionSelect.value + 1;
			if(emergencyBreak++ == 30)
				break;
		}
	}
		
	private string getResText(Resolution item){
		return item.width + " X " + item.height;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
