using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Unit acting script.
/// Many of these are controled by the game gui script
/// </summary>
public class UnitActingScript : MonoBehaviour {
	public Image unitColor;
	public Image unitHeadSprite;
	private UnitScript unit;
	public bool enemy; // TODO add some identifyer for enemy units

	public void setUnitSprite(Sprite unitHead,Color color){
		unitColor.color = color;
		unitHeadSprite.sprite = unitHead;
	}
		
	public void setCurrentlyActing(){
		unit.
		//TODO add some animation to show that this unit is acting 
	}
		
	public void setUnit(UnitScript u){
		unit = u;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
