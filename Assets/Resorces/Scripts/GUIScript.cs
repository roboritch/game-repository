using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GUIScript : MonoBehaviour{
	//Selected unit and timer attributes.
	/// <summary>
	/// The currently selected unit.
	/// </summary>
	private UnitScript currentlySelectedUnit;
	private TimerDisplay timer;

	//Button attributes.
	public ActionButtonInfo[] actionButtonInfo;
	//All this must be set in the inspector.
	//If there are null pointer exeptions check the refrences in unity.
	public Transform[] buttonLocations;
	//Maximum number of buttons to set.
	private int MAX_BUTTONS = 6;
	public Button resetUnitActions;
	#pragma warning disable
	[SerializeField] private defaultTextHolder buttonDescription;

	//Unit infromation text attributes.
	//The currently selected unit will change these themselves.
	public defaultTextHolder attack;
	public defaultTextHolder currentSize;
	public defaultTextHolder maxSize;
	public defaultTextHolder moveActions;
	public defaultTextHolder extraInfo1;
	public defaultTextHolder extraInfo2;
	public defaultTextHolder extraInfo3;
	public UnitSelectionScript unitSelectionScript;

	//Unit acting attributes.
	private UnitActingQueue unitActingQueue;

	/// <summary>
	/// Gets the unit acting queue.
	/// Only to be used by networking
	/// </summary>
	/// <returns>The unit acting queue.</returns>
	public UnitActingQueue getUnitActingQueue(){
		return unitActingQueue;
	}

	#region Unit Selection and Timer

	/// <summary>
	/// Sets the unit as selected.
	/// The gridblock should call this after it is clicked.
	/// Passing null will deselect a unit.
	/// </summary>
	/// <param name="u">Unit.</param>
	public void setSelectedUnit(UnitScript u){
		//If another or no unit is selected.
		if(currentlySelectedUnit != u){
			currentlySelectedUnit = u;
			if(u != null){
				setButtons(u.getButtonPrefabs());
				timer.setUnitUsingTimer(u);
				timer.show();
			} else{ 
				resetButton();
				timer.hide();
			}
		}
	}

	public void updateTimer(){
		if(currentlySelectedUnit != null)
			timer.setTimer();
	}

	public UnitScript getCurUnit(){
		return currentlySelectedUnit;
	}

	private void updateUnitInformation(){
		if(currentlySelectedUnit != null){
			displayUnitInformation();
		} else{
			resetUnitInformation();
		}
	}

	private void displayUnitInformation(){
		maxSize.addNewTextToDefalt(currentlySelectedUnit.getUnitMaxSize().ToString());
		moveActions.addNewTextToDefalt(currentlySelectedUnit.movmentRemaning().ToString());
		currentSize.addNewTextToDefalt(currentlySelectedUnit.getLength().ToString());
	}

	private void resetUnitInformation(){
		maxSize.setDefault();
		moveActions.setDefault();
		currentSize.setDefault();
	}

	#endregion

	#region Buttons

	public void setButtons(GameObject[] buttonPrefabs){
		resetButton();
		if(buttonPrefabs.Length > MAX_BUTTONS){
			Debug.LogWarning("Too many buttons to set!");
			return;
		}

		actionButtonInfo = new ActionButtonInfo[buttonPrefabs.Length];
		GameObject temp;
		for(int x = 0; x < buttonPrefabs.Length; x++){
			temp = Instantiate(buttonPrefabs[x]) as GameObject;
			//To get an action when the button is pressed that action must be stored with the buttons.
			actionButtonInfo[x] = temp.GetComponent<ActionButtonInfo>(); 
			temp.transform.SetParent(buttonLocations[x].transform);
			RectTransform rt = temp.GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2();
			rt.anchoredPosition = new Vector2();
			rt.localScale = Vector3.one;
			//This must be used or the last vertion of x will be called.
			int localx = x;

			temp.GetComponent<Button>().onClick.AddListener(() =>{
				this.runDisplayForThisActionButton(localx);
			});

			#region display description event triggers
			EventTrigger ET = temp.AddComponent<EventTrigger>();
			//Pointer enter.
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.callback.AddListener(delegate{
				dispayDescription(localx);	
			});
			entry.eventID = EventTriggerType.PointerEnter;
			ET.triggers.Add(entry);
			//Pointer exit.
			entry = new EventTrigger.Entry();
			entry.callback.AddListener(delegate{
				removeDescription(localx);	
			});
			entry.eventID = EventTriggerType.PointerExit;
			ET.triggers.Add(entry);
			#endregion
		}
	}

	private void dispayDescription(int ABINumber){
		buttonDescription.addNewTextToDefalt(actionButtonInfo[ABINumber].getDescriptionText());
	}

	private void removeDescription(int ABINumber){
		buttonDescription.setDefault();
	}

	private void runDisplayForThisActionButton(int ABINumber){
		currentlySelectedUnit.setTempAction(actionButtonInfo[ABINumber].getNewInstanceOfAction(currentlySelectedUnit), true);
	}


	/// <summary>
	/// Only needs to be called when no new buttons are being set.
	/// Resets the buttons.
	/// </summary>
	public void resetButton(){
		if(actionButtonInfo != null){
			for(int x = 0; x < actionButtonInfo.Length; x++){
				foreach( Transform child in actionButtonInfo[x].transform.GetComponentsInChildren<Transform>() )
					Destroy(child.gameObject);
				Destroy(actionButtonInfo[x].gameObject);
			}
			actionButtonInfo = null;
		}
	}

	public void resetUnitAction(){
		if(currentlySelectedUnit != null)
			currentlySelectedUnit.resetActionQueue(false);
	}

	#endregion

	#region Unit Action

	public void userWantsUnitToAct(){ 
		unitToAct(currentlySelectedUnit);
	}

	public void unitToAct(UnitScript unit){
		if(unit != null){
			if(unit.getNumberOfActionsInQueue() != 0){
				if(unit.IsActing){
					Debug.LogWarning("unit is already acting"); 
				} else{
					//Check if unit is already in queue.
					bool unitInQueue = false;
					foreach(UnitActingScript uas in unitActingQueue.actingQueue){
						if(uas.unit==unit)
							unitInQueue = true;
						break;
					}
					if(unitInQueue){
						Debug.LogWarning("Unit is already in unit queue!");
						return;
					}
					if(Player.Instance.workingOnline == true){
						GridLocation cU_Locaiton = unit.getCurrentBlockHeadLocation().gridLocation;
						Player.Instance.thisPlayersNetworkHelper.sendAUnitsActingQueueToAllClients((ushort)cU_Locaiton.x, (ushort)cU_Locaiton.y);
					} else{
						unitActingQueue.addToUnitActing(unit);
					}
				}
			} else{
				Debug.LogWarning("unit does not have any actions queued"); 
			}
		} else{
			//TODO Give user feedback of this
			Debug.LogWarning("no unit selected");
		}
	}

	public void unitIsDoneActing(UnitScript unit){
		unitActingQueue.currentUnitDoneActing();
	}

	#endregion

	void Start(){
		InvokeRepeating("updateTimer", 0f, 0.1f);
		unitActingQueue = GetComponentInChildren<UnitActingQueue>();
		if(timer == null){
			timer = GameObject.Find("Unit Timer").GetComponent<TimerDisplay>();
		}
	}
		
	// Update is called once per frame.
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update(){
		updateUnitInformation();
		if(currentlySelectedUnit != null){
			if(Input.GetKeyDown(KeyCode.Alpha1) == true){
				runDisplayForThisActionButton(0);
			}
			if(Input.GetKeyDown(KeyCode.Alpha2) == true){
				runDisplayForThisActionButton(1);
			}
		}
	}
}
