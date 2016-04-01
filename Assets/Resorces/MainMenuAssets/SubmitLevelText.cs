using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SubmitLevelText : MonoBehaviour{
	[SerializeField] SetLevelName Saver;

	public void submitText(){
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)){
			Saver.newLevelName(GetComponent<InputField>().text);
			// Should save submission of new level
			Saver.levelCall();
		}
	}
		
}
