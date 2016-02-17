using UnityEngine;
using System.Collections;

/// <summary>
/// Action script.
/// Each unit has a set of actions, these actions can be placed into a queue 
/// and exicuted in sequece with other actions
/// </summary>
public class ActionScript : MonoBehaviour {
	public UnitScript unit;
	public int actionID;

	/// <summary>
	/// preform this action when called by the units action queue.
	/// </summary>
	public virtual void act(){
		
	}

	/// <summary>
	/// calls the gui to display this action on the game.
	/// </summary>
	public virtual void display(){
		
	}

	/// <summary>
	/// calls this to get the gui to remove all displayed images of this action
	/// </summary>
	public virtual void removeDisplay(){
		
	}

}
