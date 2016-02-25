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
	public void setUnitAsSelected(UnitScript u) {
		currentlySelectedUnit = u;
	}

	public UnitScript getCurUnit() {
		return currentlySelectedUnit;
	}

	#endregion

	#region buttons

	//all this must be set in the inspecter
	//if there are null pointer exeptions check the refrences in unity
	public ActionScript[] actions;
	public Transform[] buttonLocations;
	// buttons the unit passes to this script
	private int MAX_BUTTONS = 6;
	public Button resetUnitActions;

	public void setButtons(GameObject[] buttonPrefabs) {
		resetButtions();
		if(buttonPrefabs.Length > MAX_BUTTONS) {
			Debug.LogWarning("too many buttons to set!");
			return;
		}

		GameObject temp;
		for(int x = 0; x < buttonPrefabs.Length; x++) {
			temp = Instantiate(buttonPrefabs[x]) as GameObject;
			actions[x] = temp.GetComponent<ActionScript>();
			temp.transform.SetParent(buttonLocations[x].transform);
		}
	}

	/// <summary>
	/// Only needs to be called when no new button are being set
	/// Resets the buttions.
	/// </summary>
	public void resetButtions() {
		for(int x = 0; x < actions.Length; x++) {
			if(actions != null) {
				foreach(GameObject child in actions[x].transform) {
					Destroy(child.gameObject);
				}
				Destroy(actions[x].gameObject);
				actions[x] = null;
			}
		}
	}

	public void resetUnitAction() {
		currentlySelectedUnit.resetActionQueue(this);
	}



	#endregion

	#region unit infromation text vars

	//the currently selected unit will change these themselves
	public defaultTextHolder attack;
	public defaultTextHolder currentSize;
	public defaultTextHolder maxSize;
	public defaultTextHolder moveActions;
	public defaultTextHolder extraInfo1;
	public defaultTextHolder extraInfo2;
	public defaultTextHolder extraInfo3;

	#endregion

	#region unit acting

	public GameObject unitActingPrefab;
	public Queue<UnitActingScript> actingQueue;
	public Transform currentProgramStartPosition;

	/// <summary>
	/// Adds to unit acting queue.
	/// </summary>
	public void addToUnitActing() {
		UnitActingScript temp = Instantiate(unitActingPrefab).GetComponent<UnitActingScript>(); 		
		temp.transform.SetParent(currentProgramStartPosition);
		temp.transform.localPosition.Set(0, actingQueue.Count * 50f, 0);//each unit acting image is 50f apart
		temp.setUnitSprite(currentlySelectedUnit.getUnitHeadSprite(), currentlySelectedUnit.getUnitColor());
		if(actingQueue.Count == 0) {
			temp.setCurrentlyActing();
		}
		temp.setUnit(currentlySelectedUnit);
		actingQueue.Enqueue(temp);
	}

	#endregion

	#region new unit selection

	public UnitSelectionScript unitSelectionScript;



	#endregion

	void Start() {
		actingQueue = new Queue<UnitActingScript>();
	}


	// Update is called once per frame
	void Update() {
	
	}
}
