using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SubmitLevelText : MonoBehaviour{
	[SerializeField] SetLevelName Saver;

	/// <summary>
	/// Saves the game 
	/// </summary>
	public void submitText(){
		Saver.newLevelName(GetComponent<InputField>().text);
		// Should save submission of new level
		Saver.levelCall();
	}


		
}
