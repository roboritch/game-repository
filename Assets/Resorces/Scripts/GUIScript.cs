using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GUIScript : MonoBehaviour {

	#region unit selection

  /// <summary>
  /// The currently selected unit.
  /// </summary>
	private UnitScript currentlySelectedUnit;

	/// <summary>
	/// Sets the unit as selected.
	/// The gridblock should call this after it is clicked.
	/// Passing null will deselect a unit.
	/// </summary>
	/// <param name="u">Unit.</param>
	public void setUnitAsSelected(UnitScript u) {
		if (currentlySelectedUnit != u) { //if another or no unit is selected
			currentlySelectedUnit = u;
			if (u != null)
				setButtons (u.getButtonPrefabs ());
			else 
				resetButtions ();	
		}
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
		resetButtions();
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
	public void resetButtions() {
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
    //Each unit acting image is 50f apart.
		temp.transform.localPosition.Set(0, actingQueue.Count * 50f, 0);
		temp.setUnitSprite(currentlySelectedUnit.getUnitHeadSprite(), currentlySelectedUnit.getUnitColor());
		if(actingQueue.Count == 0)
			temp.setCurrentlyActing();
		temp.setUnit(currentlySelectedUnit);
		actingQueue.Enqueue(temp);
	}

	#endregion

	#region new unit selection

	public UnitSelectionScript unitSelectionScript;



	#endregion

  /// <summary>
  /// Start this instance.
  /// </summary>
	void Start() {
		actingQueue = new Queue<UnitActingScript>();
	}


	// Update is called once per frame
  /// <summary>
  /// Update this instance.
  /// </summary>
	void Update() {
	
	}
}
