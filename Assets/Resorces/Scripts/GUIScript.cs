using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

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
	public GameObject unitActingPrefab;
	public Queue<UnitActingScript> actingQueue;
	public Transform currentProgramStartPosition;


	public void addToUnitActing(){
		UnitActingScript temp = Instantiate(unitActingPrefab).GetComponent<UnitActingScript>(); 		
		temp.transform.SetParent(currentProgramStartPosition);
		temp.transform.localPosition.Set(0, actingQueue.Count*50f,0);//each unit acting image is 50f apart
		temp.setUnitSprite(currentlySelectedUnit.getUnitHeadSprite,currentlySelectedUnit.getUnitColor);
		if(actingQueue.Count == 0){
			temp.setCurrentlyActing();
		}
		temp.setUnit(temp);
		actingQueue.Enqueue(temp);

	}

	#endregion

	void Start() {
		actingQueue = new Queue<UnitActingScript>();
	}


	// Update is called once per frame
	void Update() {
	
	}
}
