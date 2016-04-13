using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SubmitNumber : MonoBehaviour{

	void Start(){
		setPlayerAlliance();
	}

	public void submitAlliance(){
		setPlayerAlliance();
	}

	private void setPlayerAlliance(){
		int ddnumber = int.Parse(GetComponent<Dropdown>().captionText.text);
		Player.Instance.setMainMenuAlliance(ddnumber);
		GetComponent<Image>().color = allianceToColor(ddnumber);
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
