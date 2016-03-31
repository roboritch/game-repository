using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SubmitNumber : MonoBehaviour{

	public void OnStart(){
		Player.Instance.playerAlliance = int.Parse(GetComponent<InputField>().text);
	}

	public void submitAlliance(){
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)){
			Player.Instance.playerAlliance = int.Parse(GetComponent<InputField>().text);
		}
	}

}
