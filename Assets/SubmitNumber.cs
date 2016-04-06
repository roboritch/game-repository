using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SubmitNumber : MonoBehaviour{

	void Start(){
		Player.Instance.playerAlliance = int.Parse(GetComponent<Dropdown>().captionText.text);
		GetComponent<Image>().color = allianceToColor(Player.Instance.playerAlliance);
	}

	public void submitAlliance(){
		Player.Instance.playerAlliance = int.Parse(GetComponent<Dropdown>().captionText.text);
		GetComponent<Image>().color = allianceToColor(Player.Instance.playerAlliance);
	}

	private Color allianceToColor(int col){
		switch(col){
		case 0:
			return Color.red;
		case 1:
			return Color.blue;
		case 2:
			return Color.yellow;
		case 3:
			return Color.green;
		default:
			return Color.white;
		}


	}

}
