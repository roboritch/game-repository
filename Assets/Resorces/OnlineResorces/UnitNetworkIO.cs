using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class UnitNetworkIO : NetworkBehaviour{

	void Start(){
		
	}

	[SyncVar]
	public string unitNameToLoad;

	[Command]
	public void CmdreciveUnitFromNetwork(UnitSaving unitSaved){
		if(!localPlayerAuthority){
				  
		}
	}

	public void sendUnitOverNetwork(UnitScript unit){
		
	}

	public void spawnUnitOverNetwork(string unitName){
		
	}


}
