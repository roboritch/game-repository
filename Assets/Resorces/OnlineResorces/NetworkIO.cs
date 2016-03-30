using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class NetworkIO : NetworkBehaviour{
	public CreatePlayGrid localGrid;
	[SyncVar(hook = "setAllianceNumber")]
	private int allianceNumber;

	public override void OnStartServer(){
		base.OnStartServer();
		if(allianceNumber == -1){
			allianceNumber = 1;	
		}
	}



	void Start(){
		localGrid = GameObject.FindGameObjectWithTag("MainGrid").GetComponent<CreatePlayGrid>();
		setAlliance();
		if(isLocalPlayer)
			Cmd_SetObjNameServer(Player.Instance.playerName);


	}

	#region set playerObj representaion name
	[Command]
	private void Cmd_SetObjNameServer(string n){
		name = n;
		Rpc_SetObjNameClients(n);
	}

	[ClientRpc]
	private void Rpc_SetObjNameClients(string n){
		name = n;
	}
	#endregion

	#region player alliance Networking
	public void setAlliance(){
		if(isLocalPlayer){
			setAllianceInPlayer(allianceNumber);
			Cmd_IncrmentAlliance(); //all new players are set to a different alliance
		}
	}

	/// <summary>
	/// Sets the alliance in player.
	/// this does not update the alliance of 
	/// the units under the players control
	/// </summary>
	/// <param name="numb">allianceNumber.</param>
	private void setAllianceInPlayer(int numb){
		Player.Instance.playerAlliance = allianceNumber;
	}

	[Command]
	private void Cmd_IncrmentAlliance(){
		allianceNumber++;
	}

	private void setAllianceNumber(int numb){
		allianceNumber = numb;
	}

	#endregion

	#region unit spawn Networking
	[ClientRpc]
	public void Rpc_ReciveUnitSpawnEventFromNetwork(string unitName, int x, int y, ControlType controlType, int team){
		UnitScript unit = (Instantiate(UnitHolder.Instance.getUnitFromName(unitName)) as GameObject).GetComponent<UnitScript>();
		if(controlType == ControlType.AI){
			//TODO create this method unit.setAsAI();
		}
		unit.setTeam(team);
		localGrid.gameGrid[x, y].spawnUnit(unit);
	}

	[Command]
	public void Cmd_SendUnitSpawnEventToServer(string unitName, int x, int y, ControlType controlType, int team){
		Rpc_ReciveUnitSpawnEventFromNetwork(unitName, x, y, controlType, team);
	}
	#endregion

}
