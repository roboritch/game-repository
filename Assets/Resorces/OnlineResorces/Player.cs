using UnityEngine;

using System.Collections;

/// <summary>
/// only one instance of this script can exist at a time.
/// it can be refrenced by any other script using Manager.Instance.(nameOfThing)
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

	public NetworkIO thisPlayersNetworkHelper;
	public bool workingOnline = false;
	public int numberOfClientsReadyToAct;
	public string dataPathOfLevelTheUserWantsToLoad = "";
}
