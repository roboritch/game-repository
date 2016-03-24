using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitActingQueue : MonoBehaviour {

	void Start() {
		actingQueue = new Queue<UnitActingScript>();
	}

	#region unit acting
	#pragma warning disable
	[SerializeField] private GameObject unitActingPrefab;
	[SerializeField] private GameObject actingOverlay;
	[SerializeField] private Transform currentProgramStartPosition;

	[SerializeField] private int maxVisibleItems = 3;
	private Queue<UnitActingScript> actingQueue;
	/// <summary>
	/// Adds to unit acting queue.
	/// </summary>
	public void addToUnitActing(UnitScript currentlySelectedUnit) {
		UnitActingScript temp = Instantiate(unitActingPrefab).GetComponent<UnitActingScript>();
		temp.transform.SetParent(currentProgramStartPosition);
		//Each unit acting image is 50f apart.
		temp.location.anchoredPosition = new Vector2(0, actingQueue.Count * -50f);
		temp.setUnit(currentlySelectedUnit);
		actingQueue.Enqueue(temp);
		if(actingQueue.Count == 1){
			temp.setCurrentlyActing();
		}else if(actingQueue.Count > maxVisibleItems){
			temp.setVisible(false);
		}
	}

	/// <summary>
	/// called by the unit when it is done acting
	/// </summary>
	/// <param name="currentUnit">Current unit.</param>
	public void currentUnitDoneActing(UnitScript currentUnit){
		UnitActingScript temp = actingQueue.Dequeue();
		temp.destroyThis();
		if(actingQueue.Count != 0){
			actingQueue.Peek().setCurrentlyActing();
			int deactivatedItem = 0;
			foreach (var item in actingQueue) { //shift all images up one
				item.location.anchoredPosition = new Vector2(0,item.location.anchoredPosition.y + 50f);
				if(deactivatedItem++ < maxVisibleItems){ // check to see if the item is now visible
					item.setVisible(true); 
				}
			}
		}
	}

	#endregion


}
