using UnityEngine;

/// <summary>
/// Program script also known as a unit.
/// Parent script for all programs
/// </summary>
public class UnitScript : MonoBehaviour {
	#region programName

	private string programName;

	protected void setProgramName(string name) {
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

	private int currentLength=1;

	public int CurrentLength {
		get{ return currentLength; }
	}
	public void setCurrentLength(int value){
		currentLength = value;
	}
	// length starts at one unless otherwise specified

	public virtual void receiveDamage(int damageAmount) {
		if(CurrentLength>damageAmount){
			setCurrentLength(CurrentLength - damageAmount);}
		else {
			setCurrentLength(0);
			GridBlockSpriteDisplay.DestroyObject;}//removes sprite
			
	}

	#endregion

	#region programAttack
	private int attackDamage;
	public int AttackDamage {
		get{ return attackDamage; }
	}
	public void setAttackDamage(int value){
		attackDamage = value;
	}

	public virtual void attack(UnitScript target) { //TODO  program attack
		target.receiveDamage(AttackDamage);
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
