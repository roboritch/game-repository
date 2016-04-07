﻿using UnityEngine;

using System.Collections;

/// <summary>
/// only one instance of this script can exist at a time.
/// it can be refrenced by any other script using Manager.Instance.(nameOfThing)
/// </summary>
public class Player : Singleton<Player>{
	public Team team;

	protected Player(){
		playerAlliance = -1;
	}

	void Start(){
		DontDestroyOnLoad(gameObject);
		numberOfClientsReadyToAct = 0;
	}

	public string playerName = "";

	public void setPlayerName(string name){
		playerName = name;
		gameObject.name = "player " + name;
	}

	public int playerAlliance;
	public NetworkIO thisPlayersNetworkHelper;
	public bool workingOnline = false;
	public int numberOfClientsReadyToAct;

}
