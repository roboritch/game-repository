using UnityEngine;
using System.Collections.Generic;

public class Team
{
	private LinkedList<UnitScript> units;
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
		units.AddLast (unit);

	}
	public int getIndex(){
		return index;
	}
	public void removeAlly(UnitScript unit){
		units.Remove(unit);
	}
	public bool defeated(){
		if (units == null) {
			return true;
		}
		return false;
	}
	public Color getColor(){
		return teamColor;
	}
	public static Color colorBlend(Color c,Color c2,float s){
		float a = c.a;
		float r = c.r;
		float g = c.g;
		float b = c.b;
		float r2 = c2.r;
		float g2= c2.g;
		float b2 = c2.b;
		r = (r*(1-s) + r2*s);
		g = (g*(1-s) + g2*s);
		b = (b*(1-s) + b2*s);
		return new Color (r, g, b, a);

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

