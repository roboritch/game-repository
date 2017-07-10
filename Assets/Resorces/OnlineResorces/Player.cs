using UnityEngine;

using System.Collections;

/// <summary>
/// only one instance of this script can exist at a time.
/// it can be referenced by any other script using Manager.Instance.(nameOfThing)
/// 
/// This class describes a player 
/// </summary>
public class Player : Singleton<Player>{
	protected Player(){
	}
	#region team setting
	private int mainMenuAlliance;

	public void setMainMenuAlliance(int alliance){
		mainMenuAlliance = alliance;
	}

	void OnLevelWasLoaded(int levelNumb){
		Invoke("setTeam", 0.2f);
	}

	private void setTeam(){
		GameObject maingrid = GameObject.FindWithTag("MainGrid");
		if(maingrid != null){
			team = maingrid.GetComponent<CreatePlayGrid>().team[mainMenuAlliance];
		}
	}

	private Team team;

	public Team Team{
		get{
			return team;
		}
		set{
			team = value;
		}
	}

	private void setTeamIfNotSet(){
		GameObject go = GameObject.FindWithTag("MainGrid");
		if(go != null)
		if(team == null){
			team = go.GetComponent<CreatePlayGrid>().team[0];
		}
	}
	#endregion

	void Start(){
		DontDestroyOnLoad(gameObject);
		numberOfClientsReadyToAct = 0;
		Invoke("setTeamIfNotSet", 0.5f); // used for debuging if main menu is not used
	}


	public string playerName = "";

	public void setPlayerName(string name){
		playerName = name;
		gameObject.name = "player " + name;
	}

	/// <summary>
	/// the network object that helps this player send commands across the network
	/// </summary>
	public NetworkIO thisPlayersNetworkHelper;
	/// <summary>
	/// indicates whether or not this player is connected to an online game
	/// </summary>
	public bool workingOnline = false;
	/// <summary>
	/// used to synchronize all 
	/// </summary>
	public int numberOfClientsReadyToAct;
	/// <summary>
	/// level that all players are loading
	/// </summary>
	public string dataPathOfLevelTheUserWantsToLoad = "";
	/// <summary>
	/// this player can spawn and control all units
	/// </summary>
	public bool debugUnitControl = false;
}
