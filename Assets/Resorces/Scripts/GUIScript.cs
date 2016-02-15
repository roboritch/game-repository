using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIScript : MonoBehaviour {

	#region unit selection
	private UnitScript currentlySelectedUnit;
	/// <summary>
	/// Sets the unit as selected.
	/// The gridblock should call this after it is clicked
	/// Passing null will deselect a unit
	/// </summary>
	/// <param name="u">Unit.</param>
	public void setUnitAsSelected(UnitScript u){
		currentlySelectedUnit = u;
	}
	public UnitScript getCurUnit(){
		return currentlySelectedUnit;
	}
	#endregion

	#region buttons
	//all this must be set in the inspecter
	//if there are null pointer exeptions check the refrences in unity
	public Button[] buttons; // buttons the unit passes to this script
	private int MAX_BUTTONS = 6;
	public Button resetUnitActions;
	public void resetUnitAction(){
		//currentlySelectedUnit.resetActionQueue(); TODO get this working in unitScript
	}


	#endregion

	#region unit infromation text var
	//the currently selected unit will change these themselves
	public defaultTextHolder attack;
	public defaultTextHolder currentSize;
	public defaultTextHolder maxSize;
	public defaultTextHolder moveActions;
	public defaultTextHolder extraInfo1;
	public defaultTextHolder extraInfo2;
	public defaultTextHolder extraInfo3;
	#endregion


	//TODO setup unit acting
	#region unit acting

	#endregion

	void Start() {
		
	}


	// Update is called once per frame
	void Update() {
	
	}
}
