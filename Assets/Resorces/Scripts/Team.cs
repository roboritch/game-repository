using UnityEngine;
using System.Collections.Generic;

public class Team : MonoBehaviour
{
	private LinkedList<UnitScript> team;
	private int enemyKilled;
	private int spawned;
	private int score;
	private Color teamColor;
	private int index;

	public Team(Color tc,int teamIndex){
		enemyKilled = 0;
		spawned = 0;
		score = 0;
		teamColor = tc;
		index = teamIndex;
	}
	public void addAlly(UnitScript unit){
		team.AddLast (unit);

	}
	public int getIndex(){
		return index;
	}
	public void removeAlly(UnitScript unit){
		team.Remove (unit);
	}
	public bool defeated(){
		if (team == null) {
			return true;
		}
		return false;
	}
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

