using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkActionCheck : NetworkBehaviour{

	public void networkAct(){
		GUIScript gui = GetComponentInParent<GUIScript>();
		UnitScript currentUnit = gui.getCurUnit();
		if(currentUnit != null){
			if(currentUnit.getNumberOfActionsInQueue() != 0){
				if(currentUnit.IsActing){
					Debug.LogWarning("unit is already acting"); 
				} else{
					gui.getUnitActingQueue().addToUnitActing(gui.getCurUnit());
					//
				}
			} else{
				Debug.LogWarning("unit does not have any actions queued"); 
			}
		} else{
			//TODO Give user feedback of this
			Debug.LogWarning("no unit selected");
		}
	}

	[Command]
	private void CmdsendUnitActionsOverNetwork(){
		
	}

}
