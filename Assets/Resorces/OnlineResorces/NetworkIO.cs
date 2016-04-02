﻿using System;
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
		if(isLocalPlayer){
			Cmd_SetObjNameServer(Player.Instance.playerName);
			Player.Instance.thisPlayersNetworkHelper = this;
			Player.Instance.workingOnline = true;
		}
		InvokeRepeating("checkAllIfAllClientsAreReadyToAct", 2f, 0.1f);
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
	private void Rpc_ReciveUnitSpawnEventFromNetwork(string unitName, ushort  x, ushort  y, byte team){
		UnitScript unit = (Instantiate(UnitHolder.Instance.getUnitFromName(unitName)) as GameObject).GetComponent<UnitScript>();
		localGrid.gameGrid[x, y].spawnUnit(unit);
	}

	[Command]
	public void Cmd_SendUnitSpawnEventToServer(string unitName, ushort x, ushort y, byte team){
		Rpc_ReciveUnitSpawnEventFromNetwork(unitName, x, y, team);
	}
	#endregion

	#region send unit actions over network
	/// <summary>
	/// Converts the aciont queue to server command format for minimal network load.
	/// </summary>
	/// <param name="loc">Location of the unit that is acting.</param>
	/// <param name="actionQueue">Action queue</param>
	public void convertAciontQueueToServerCommand_ACT(GridLocation loc, LinkedList<ActionScript> actionQueue){
		int numberOfActions = actionQueue.Count;
		byte[] actionType = new byte[numberOfActions];
		byte[] actionAmount = new byte[numberOfActions];
		ushort[] locX = new ushort[numberOfActions];
		ushort[] locY = new ushort[numberOfActions];
		int count = 0;
		SerializedCompletedAction sca;
		foreach( ActionScript item in actionQueue ){
			sca = item.serializeAction();
			actionType[count] = getActionByte(sca.actionType);
			actionAmount[count] = (byte)sca.actionAmountInt;
			locX[count] = (ushort)sca.locationToPerformAction.x;
			locY[count++] = (ushort)sca.locationToPerformAction.y;
		}
		Cmd_sendCompleatedActionQueueOverNetwork((ushort)loc.x, (ushort)loc.y, actionType, actionAmount, locX, locY);
	}

	[ClientRpc]
	private void Rpc_receaveUnitActionsOverNetwork(ushort x, ushort  y, byte[] actionType, byte[] actionAmount, ushort[] locX, ushort[] locY){
		Type type;
		ActionScript tmp; 
		SerializedCompletedAction sca = new SerializedCompletedAction();

		for(int i = 0; i < actionType.Length; i++){
			type = getActionType(actionType[i]);
			tmp = (ActionScript)Activator.CreateInstance(type);
			sca.actionAmountInt = actionAmount[i];
			sca.locationToPerformAction = new GridLocation((int)locX[i], (int)locY[i]);
			tmp.loadAction(sca);
			localGrid.gameGrid[x, y].unitInstalled.addActionToQueue(tmp);
		}
	}

	[Command]
	private void Cmd_sendCompleatedActionQueueOverNetwork(ushort x, ushort  y, byte[] actionType, byte[] actionAmount, ushort[] locX, ushort[] locY){
		Rpc_receaveUnitActionsOverNetwork((ushort)x, (ushort)y, actionType, actionAmount, locX, locY);
	}

	private Type getActionType(byte aNumb){
		if(aNumb == 0){
			return typeof(MoveScript);
		} else if(aNumb == 1){
			return typeof(AttackScript);
		}
		return null;
	}

	private byte getActionByte(Type type){
		if(type == typeof(MoveScript)){
			return 0;
		} else if(type == typeof(AttackScript)){
			return 1;
		}
		return byte.MaxValue;
	}

	#endregion

	#region syncronized unit acting

	private int numberOfClientsThatAreReadyToAct = 0;

	[Command] //Called by each action queue when a unit is ready to act
	public void Cmd_incNumberOfReadyClients(){
		numberOfClientsThatAreReadyToAct++;
	}

	[Server] //invoked repeatedly by the server 
	private void checkAllIfAllClientsAreReadyToAct(){
		if(numberOfClientsThatAreReadyToAct == Network.connections.Length){
			tellTheActionQueueToRunNextActionOnAllClients();
			numberOfClientsThatAreReadyToAct = 0;
		}
	}

	/// <summary>
	/// get unit to start acting.
	/// Called by server when number of clients ready to act is = to the totalClients
	/// </summary>
	/// <param name="x">The x coordinate of the unit.</param>
	/// <param name="y">The y coordinate of the unit.</param>
	[Server]
	public void tellTheActionQueueToRunNextActionOnAllClients(){
		Rpc_CallOnlineActionQueue();
	}

	[ClientRpc]
	private void Rpc_CallOnlineActionQueue(){
		localGrid.gui.getUnitActingQueue().online_AllClientsReportReady();
	}


	#endregion
}
