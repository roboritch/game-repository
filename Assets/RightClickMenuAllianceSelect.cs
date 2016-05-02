using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RightClickMenuAllianceSelect : MonoBehaviour{

	void Start(){
		Invoke("setAllianceForSaving", 0.02f);
	}

	[SerializeField] private ContextCanvas context;

	public void setAllianceForSaving(){
		int alliance = int.Parse(GetComponent<Dropdown>().captionText.text);
		GetComponent<Image>().color = allianceToColor(alliance);
		context.setAllianceNumberForSpawnSpot(alliance);
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
