using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SubmitText : MonoBehaviour{
	[SerializeField] SetScreenName Saver;

	public void submitText(){
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)){
			Saver.receveName(GetComponent<InputField>().text);
		}
	}

}
