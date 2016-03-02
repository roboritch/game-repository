using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

/// <summary>
/// Action script.
/// Each unit has a set of actions, these actions can be placed into a list 
/// and exicuted in sequece with other actions
/// </summary>
public class ActionScript{
	protected UnitScript unit;
	/// <summary>
	/// Some actions may require more than one action Id
	/// </summary>
	private int actionID;
	//set for each desplay of the action
	//each move, attack or action must be a seprate instance of this script

	public int ActionID {
		get {
			return actionID;
		}
		set {
			actionID = value;
		}
	}
		
	/// <summary>
	/// preform this action when called by the units action list.
	/// </summary>
	public virtual void act() {
		
	}

	/// <summary>
	/// The amount of time it takes to animate the unit preforming this action
	/// </summary>
	/// <returns>The time.</returns>
	public virtual float actionTime() {
		return 1.0f;
	}

	/// <summary>
	/// calls the gui to display this action on the game.
	/// </summary>
	public virtual void display(GUIScript gui) {
		
	}

	/// <summary>
	/// calls this to get the gui to remove all displayed images of this action
	/// </summary>
	public virtual void removeDisplay(GUIScript gui) {
		
	}
}
