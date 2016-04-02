using UnityEngine;
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
	private Queue<UnitActingScript> actingQueue;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start(){
		actingQueue = new Queue<UnitActingScript>();
	}

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
			temp.setCurrentlyActing();
		} else if(actingQueue.Count > maxVisibleItems){
			temp.setVisible(false);
		}
	}

	/// <summary>
	/// Called by the unit when it is done acting.
	/// </summary>
	/// <param name="currentUnit">Current unit.</param>
	public void currentUnitDoneActing(){
		UnitActingScript temp = actingQueue.Dequeue();
		temp.destroyThis();
		if(actingQueue.Count != 0){
			if(!actingQueue.Peek().setCurrentlyActing()){ //gets rid of any units destoroyed before they can act
				currentUnitDoneActing();
				localOnlineQueueReadyForNextUnit = true;
				return;
			}

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
		localOnlineQueueReadyForNextUnit = true;
	}

	#region online action queue
	private bool localOnlineQueueReadyForNextUnit = true;

	public bool isLocalQueueReadyForAction(){
		return localOnlineQueueReadyForNextUnit;
	}

	public void online_AddUnitToActing(UnitScript unit){
		UnitActingScript temp = Instantiate(unitActingPrefab).GetComponent<UnitActingScript>();
		temp.transform.SetParent(currentProgramStartPosition);
		//Each unit acting image is 50f apart.
		temp.location.anchoredPosition = new Vector2(0, actingQueue.Count * -50f);
		temp.setUnit(unit);
		actingQueue.Enqueue(temp);
		if(actingQueue.Count == 1){
			localOnlineQueueReadyForNextUnit = false;
			temp.setCurrentlyActing();
		} else if(actingQueue.Count > maxVisibleItems){
			temp.setVisible(false);
		}
	}

	#endregion
}
