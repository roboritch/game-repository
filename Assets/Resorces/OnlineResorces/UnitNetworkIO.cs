﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class UnitNetworkIO : NetworkBehaviour{
	public CreatePlayGrid localGrid;
	[SyncVar(hook = "setAllianceNumber")]
	private int allianceNumber = 2;


	void Start(){
		localGrid = GameObject.FindGameObjectWithTag("MainGrid").GetComponent<CreatePlayGrid>();
		setAlliance();
	}


	#region player alliance Networking
	public void setAlliance(){
		if(isServer){
			Player.Instance.playerAlliance = 1;
		} else{
			setAllianceInPlayer(allianceNumber);
			CmdIncrmentAlliance(); //all new players are set to a different alliance
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
	private void CmdIncrmentAlliance(){
		allianceNumber++;
	}

	private void setAllianceNumber(int numb){
		allianceNumber = numb;
	}

	#endregion

	#region unit spawn Networking
	[ClientRpc]
	public void RpcReciveUnitSpawnEventFromNetwork(string unitName, int x, int y, ControlType controlType, int team){
		UnitScript unit = (Instantiate(UnitHolder.Instance.getUnitFromName(unitName)) as GameObject).GetComponent<UnitScript>();
		if(controlType == ControlType.AI){
			//TODO create this method unit.setAsAI();
		}
		unit.setTeam(team);
		localGrid.gameGrid[x, y].spawnUnit(unit);
	}

	[Command]
	public void Cmd_SendUnitSpawnEventToServer(string unitName, int x, int y, ControlType controlType, int team){
		RpcReciveUnitSpawnEventFromNetwork(unitName, x, y, controlType, team);
	}
	#endregion

}
