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
	public bool enemy; // TODO add some identifyer for enemy units

	public void setUnitSprite(Sprite unitHead,Color color){
		unitColor.color = color;
		unitHeadSprite.sprite = unitHead;
	}
		
	public void setCurrentlyActing(){
		//TODO add some animation to show that this unit is acting 
	}
		

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
