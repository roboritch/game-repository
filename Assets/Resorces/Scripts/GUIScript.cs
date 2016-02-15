using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIScript : MonoBehaviour {
	public Button[] buttons;
	private int MAX_BUTTONS = 8;

	// Use this for initialization
	void Start() {
		buttons = new Button[MAX_BUTTONS];

	}
	
	// Update is called once per frame
	void Update() {
	
	}
}
