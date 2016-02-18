using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Program script also known as a unit.
/// Parent script for all programs
/// </summary>
public class UnitScript : MonoBehaviour {
	#region programName

	private string programName;

	protected void setProgramamName(string name) {
		programName = name;
	}

	public string getProgramName() {
		return programName;
	}



	#endregion

	#region maxProgramLength

	private int maxProgramLength;

	public int MaxProgramLength {
		get {
			return maxProgramLength;
		}
	}

	public bool programLengthModifiable = true;

	/// <summary>
	/// Sets the length of the program.
	/// </summary>
	/// <returns><c>true</c>, if program length was set, <c>false</c> otherwise.</returns>
	/// <param name="value">Value.</param>
	public bool setMaxProgramLength(int value) {
		if(programLengthModifiable) {
			maxProgramLength = value;
			return true;
		} else {
			return false;
		}
	}

	#endregion

	#region currentLength

	/// <summary>
	/// The current length must be modifyed by the attack method
	/// </summary>
	private int length = 1;
	// length starts at one unless otherwise specified

	public virtual void receiveDamage(int damageAmount) {

	}

	#endregion

	#region sprite information
	public Color unitColor;
	/// <summary>
	/// Gets the color of the unit.
	/// must be overiden by new unit with that units color
	/// </summary>
	/// <returns>The unit color.</returns>
	public virtual Color getUnitColor() {
		return unitColor;
	}

	/// <summary>
	/// The head sprite.
	/// must be set from child unit
	/// </summary>
	public Sprite headSprite;

	public virtual Sprite getUnitHeadSprite() {
		return headSprite;
	}

	#endregion

	#region Actions
	private CreatePlayGrid grid;
	/// <summary>
	/// Sets the grid conection.
	/// this must be set when the unit is created
	/// </summary>
	/// <param name="playGrid">Play grid.</param>
	public void setGridConection(CreatePlayGrid playGrid){
		grid = playGrid;
	}

	/// <summary>
	/// Displaies the action as buttion.
	/// Each unit will have it's own button prefabs
	/// </summary>
	/// <param name="actionDiscription">The Action's discription.</param>
	/// <param name="button">button that will be displayed on the gui</param>
	public void displayActionAsButtion(string actionDiscription,GameObject button){
		
	}
	#endregion

	#region Action queue
	public Queue<ActionScript> action;
	//TODO add action queue 
	/*each action will be a child of the ActionScript */

	public void resetActionQueue(){
		/*TODO this function must remove every action from the queue
		while calling each action to remove there displayed images */ 
	}



	#endregion
	// Use this for initialization
	void Start() {
		action = new Queue<ActionScript>();
	}

	// Update is called once per frame
	void Update() {

	}
}
