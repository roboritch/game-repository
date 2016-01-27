using UnityEngine;

/// <summary>
/// Program script also known as a unit.
/// Parent script for all programs
/// </summary>
public class ProgramScript : MonoBehaviour {
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

	#region programAttack


	public virtual void attack() { //TODO  program attack

	}

	#endregion

	#region programMovment

	//TODO enable program movent

	#endregion


	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}
}
