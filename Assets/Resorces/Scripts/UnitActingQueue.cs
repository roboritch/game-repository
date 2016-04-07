using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

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
		UnitActingScript temp = Instantiate(unitActingPrefab).GetComponent<UnitActingScript>();
		temp.transform.SetParent(currentProgramStartPosition);
		//Each unit acting image is 50f apart.
		temp.location.anchoredPosition = new Vector2(0, actingQueue.Count * -50f);
		temp.setUnit(currentlySelectedUnit);
		actingQueue.Enqueue(temp);
		if(actingQueue.Count == 1){
			if(Player.Instance.workingOnline){ //Online check
				Player.Instance.thisPlayersNetworkHelper.Cmd_incNumberOfReadyClients();
			} else{
				if(temp.setCurrentlyActing()){
					Invoke("currentUnitDoneActing", 0.001f);
				}
			}
		} else if(actingQueue.Count > maxVisibleItems){
			temp.setVisible(false);
		}
	}

	/// <summary>
	/// Called by the unit when it is done acting.
	/// </summary>
	/// <param name="currentUnit">Current unit.</param>
	public void currentUnitDoneActing(){

		//Remove the representation of an acting unit.
		if(actingQueue.Count > 0)
			actingQueue.Dequeue().destroyThis();

		if(actingQueue.Count != 0){

			if(!actingQueue.Peek().checkIfUnitIsAlive()){ //gets rid of any units destoroyed before they can act
				currentUnitDoneActing();
				shiftUnitRepresentationsUpOneLevel();
				return;
			}

			shiftUnitRepresentationsUpOneLevel();
			if(Player.Instance.workingOnline) // wait for server to send all clients ready message 
				Player.Instance.thisPlayersNetworkHelper.Cmd_incNumberOfReadyClients();
			else
				actingQueue.Peek().setCurrentlyActing();
		}
	}
	#endregion

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
