using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// controls when a unit will act
/// </summary>
public class UnitActingQueue : MonoBehaviour{
	#pragma warning disable
	[SerializeField] private GameObject unitActingPrefab;
	[SerializeField] private GameObject actingOverlay;
	[SerializeField] private Transform currentProgramStartPosition;

	[SerializeField] private int maxVisibleItems = 3;
	public Queue<UnitActingScript> actingQueue;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start(){
		actingQueue = new Queue<UnitActingScript>();
	}

	#region acting and done acting
	/// <summary>
	/// Adds to unit acting queue.
	/// </summary>
	public void addToUnitActing(UnitScript currentlySelectedUnit){
		UnitActingScript currentUnitAction = Instantiate(unitActingPrefab).GetComponent<UnitActingScript>();
		currentUnitAction.transform.SetParent(currentProgramStartPosition);
		//Each unit acting image is 50f apart.
		currentUnitAction.location.anchoredPosition = new Vector2(0, actingQueue.Count * -50f);
		currentUnitAction.setUnit(currentlySelectedUnit);
		actingQueue.Enqueue(currentUnitAction);
		if(actingQueue.Count == 1){
			if(Player.Instance.workingOnline){ //Online check
				Player.Instance.thisPlayersNetworkHelper.Cmd_incNumberOfReadyClients();
			} else{
				if(!currentUnitAction.setCurrentlyActing()){
					currentUnitDoneActing();
				}
			}
		} else if(actingQueue.Count > maxVisibleItems){
			//TODO update to use canvas masks
			//Hide the action if it goes of screen
			currentUnitAction.setVisible(false);
		}
	}

	/// <summary>
	/// Called by the unit when it is done acting.
	/// </summary>
	/// <param name="currentUnit">Current unit.</param>
	public void currentUnitDoneActing(){
		if(actingQueue.Count >= 1){
			//remove acting unit from the queue once it is done it's action
			actingQueue.Dequeue().destroyThis();

			if(actingQueue.Count != 0){
				if(!actingQueue.Peek().checkIfUnitIsAlive()){ //gets rid of any units destroyed before they can act
					currentUnitDoneActing(); //remove the dead unit by calling this method again
					shiftUnitRepresentationsUpOneLevel();
					return; //dead units don't do anything else
				}

				shiftUnitRepresentationsUpOneLevel();
				if(Player.Instance.workingOnline) // wait for server to send all clients ready message 
				Player.Instance.thisPlayersNetworkHelper.Cmd_incNumberOfReadyClients();
				else
					actingQueue.Peek().setCurrentlyActing();
			}
		}
	}
	#endregion

	/// <summary>
	/// shifts all unit representations up 
	/// </summary>
	private void shiftUnitRepresentationsUpOneLevel(){
		int deactivatedItem = 0;
		//Shift all images up one.
		foreach( var item in actingQueue ){
			item.location.anchoredPosition = new Vector2(0, item.location.anchoredPosition.y + 50f);
			// Check to see if the item is now visible.
			if(deactivatedItem++ < maxVisibleItems){
				item.setVisible(true); 
			}
		}
	}

	#region online action queue

	public void online_AllClientsReportReady(){
		actingQueue.Peek().setCurrentlyActing();
	}

	#endregion
}
