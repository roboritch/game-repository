using UnityEngine;
using System.Collections;

/// <summary>
/// Action script.
/// Each unit has a set of actions, these actions can be placed into a queue 
/// and exicuted in sequece with other actions
/// </summary>
public class ActionScript : MonoBehaviour {
	private UnitScript unit; // this should be the instance of the unit that  
	private int actionID;  //set for each desplay of the action 
						   //each move, attack or action must be a seprate instance of this script 
	public int ActionID {
		get {
			return actionID;
		}
		set {
			actionID = value;
		}
	}


	#region button infromation
	public string actionName;
	public string actionDescription;
	public GameObject buttonPrefab;
	#endregion

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
