﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GUIScript : MonoBehaviour {

	#region unit selection and timer

  /// <summary>
  /// The currently selected unit.
  /// </summary>
	private UnitScript currentlySelectedUnit;
	private TimerDisplay timer;

	/// <summary>
	/// Sets the unit as selected.
	/// The gridblock should call this after it is clicked.
	/// Passing null will deselect a unit.
	/// </summary>
	/// <param name="u">Unit.</param>
	public void setSelectedUnit(UnitScript u) {
		if (currentlySelectedUnit != u) { //if another or no unit is selected
			currentlySelectedUnit = u;
			if(u != null){
				setButtons(u.getButtonPrefabs());
				timer.setUnitUsingTimer(u);
				timer.show();
			} else { 
				resetButton();
				timer.hide();
			}
		}
	}

	public void updateTimer(){
		if(currentlySelectedUnit != null)	
			timer.setTimer();
	}

	public UnitScript getCurUnit() {
		return currentlySelectedUnit;
	}

	#endregion

	#region buttons


	public ActionButtonInfo[] actionButtonInfo;
	//All this must be set in the inspector.
	//If there are null pointer exeptions check the refrences in unity.
	public Transform[] buttonLocations;
	//Maximum number of buttons to set.
	private int MAX_BUTTONS = 6;
	public Button resetUnitActions;

	public void setButtons(GameObject[] buttonPrefabs) {
		resetButton();
		if(buttonPrefabs.Length > MAX_BUTTONS) {
			Debug.LogWarning("Too many buttons to set!");
			return;
		}

		actionButtonInfo = new ActionButtonInfo[buttonPrefabs.Length];
		GameObject temp;
		for(int x = 0; x < buttonPrefabs.Length; x++) {
			temp = Instantiate(buttonPrefabs[x]) as GameObject;
			//To get an action when the button is pressed that action must be stored with the buttons.
			actionButtonInfo[x] = temp.GetComponent<ActionButtonInfo>(); 
			temp.transform.SetParent(buttonLocations[x].transform);
			RectTransform rt = temp.GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2();
			rt.anchoredPosition = new Vector2();
			int localx = x; //this must be used or the last vertion of x will be called
			temp.GetComponent<Button>().onClick.AddListener(() => {
				this.runDisplayForThisActionButton(localx);
			});
		}
	}


	private void runDisplayForThisActionButton(int ABINumber) {
		currentlySelectedUnit.tempAction = actionButtonInfo[ABINumber].getNewInstanceOfAction(currentlySelectedUnit);
		currentlySelectedUnit.tempAction.displayUserSelection();
	}


	/// <summary>
	/// Only needs to be called when no new buttons are being set.
	/// Resets the buttons.
	/// </summary>
	public void resetButton() {
		if(actionButtonInfo != null)
			for(int x = 0; x < actionButtonInfo.Length; x++) {
				foreach(GameObject child in actionButtonInfo[x].transform)
					Destroy(child.gameObject);
				Destroy(actionButtonInfo[x].gameObject);
				actionButtonInfo = null;
			}
	}

	public void resetUnitAction() {
		currentlySelectedUnit.resetActionQueue(this);
	}



	#endregion

	#region unit infromation text vars

	//The currently selected unit will change these themselves.
	public defaultTextHolder attack;
	public defaultTextHolder currentSize;
	public defaultTextHolder maxSize;
	public defaultTextHolder moveActions;
	public defaultTextHolder extraInfo1;
	public defaultTextHolder extraInfo2;
	public defaultTextHolder extraInfo3;

	#endregion

	#region new unit selection

	public UnitSelectionScript unitSelectionScript;



	#endregion

	private UnitActingQueue unitActingQueue;
	public void unitIsActing(UnitScript unit){
		unitActingQueue.addToUnitActing(unit);
	}

	public void unitIsDoneActing(UnitScript unit){
		unitActingQueue.currentUnitDoneActing(unit);
	}

	void Start(){
		InvokeRepeating("updateTimer",0f,0.2f);
		unitActingQueue = GetComponentInChildren<UnitActingQueue>();
		if(timer == null){
			timer = GameObject.Find("Unit Timer").GetComponent<TimerDisplay>();
		}
	}


	// Update is called once per frame
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update() {
	
	}
}
