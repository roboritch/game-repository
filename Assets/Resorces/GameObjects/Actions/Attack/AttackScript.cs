using UnityEngine;
using System.Collections;

/// <summary> Attack script.</summary>
public class AttackScript : ActionScript{

	public AttackScript (UnitScript u) : base (u){
	
	}

	#region attack strength and location
	/// <summary>
  /// The attack grid block location.
  /// </summary>
	private GridBlock attackLocation;

	/// <summary>
 	/// The attack strength; the number of blocks removed by this attack.
 	/// </summary>
	private int attackStrength = 0;

	/// <summary>
	 /// Sets the attack location.
 	/// </summary>
 	/// <param name="attackLocation">Attack location.</param>
	private void setAttackLocation(GridBlock attackLocation){
		this.attackLocation = attackLocation;
	}

	/// <summary>
  /// Gets the attack location.
  /// </summary>
  /// <returns>The attack location.</returns>
	public GridBlock getAttackLocation(){
		return attackLocation;
	}

	/// <summary>
  /// Sets the attack strength.
  /// </summary>
  /// <param name="attackStrength">Attack strength.</param>
	public void setAttackStrength(int attackStrength){
		this.attackStrength = attackStrength;
	}

	/// <summary>
  /// Gets the attack strength.
  /// </summary>
  /// <returns>The attack strength.</returns>
	public int getAttackStrength(){
		return attackStrength;
	}
	#endregion

	/// <summary>
  /// Perform this action when called by the unit's action list.
  /// </summary>
	public override void act(){
		if(attackLocation != null)
		if (attackLocation.unitInstalled != null){
			attackLocation.unitInstalled.removeBlock(attackStrength);
		}
		
	}

	#region user display
	/// <summary>
	/// Calls the GUI to display this action on the game.
	/// </summary>
	/// <param name="gui">GUI.</param>
	public override void displayUserSelection(){
		if(unit.attacksRemaning() <= 0){
			Debug.Log("no attack actions remaning");
			return;
		}
		setPosibleAttackLocations();
		checkAndDisplayPossibleUserActions();
	}

	#region set posible attack locations
	private GridBlock[] posibleAttackLocations;
	/// <summary>
	/// Sets the posible attack locations.
	/// Valid locations will be flound within this script
	/// </summary>
	/// <param name="attackLocations">Attack locations.</param>
	public void setPosibleAttackLocations(GridBlock[] attackLocations){
		posibleAttackLocations = attackLocations;
	}

	public void setPosibleAttackLocations(){
		unit.getAttackLocations();
	}

	#endregion

	private void checkAndDisplayPossibleUserActions(){
		for (int i = 0; i < posibleAttackLocations.Length; i++) {
			posibleAttackLocations[i].attackActionWantsToAttackHere(this);
		}
	}
		

	public override void removeUserSelectionDisplay () {
		for (int i = 0; i < posibleAttackLocations.Length; i++) {
			posibleAttackLocations[i].removeAttackDisplayForThis();
		}
	}

	#endregion

	#region finished action display
	private int actionDisplayID = -1; //for when enamy actions are loaded
	public override void displayFinishedAction () {
		actionDisplayID = attackLocation.spriteDisplayScript.displayAction(unit.grid.spritesAndColors.sprite_attack);
		unit.useAttackAction();
	}

	/// <summary>
	/// Calls this to get the GUI to remove all displayed images of this action.
	/// </summary>
	/// <param name="gui">GUI.</param>
	public override void removeActionRepresentationDisplay(){
		attackLocation.spriteDisplayScript.removeAction(actionDisplayID);
		unit.addAttackAction();
	}
	#endregion 


	public override void userSelectedAction (GridBlock blockSelected) {
		removeUserSelectionDisplay();
		attackLocation = blockSelected;
		displayFinishedAction();
	}

	#region action Save And Load
	public override void loadAction (SerializedCompeatedAction s) {
		attackStrength = s.actionAmountInt;
		attackLocation = unit.grid.gridLocationToGameGrid(s.locationToPreformAction);
	}

	public override SerializedCompeatedAction serializeAction () {
		SerializedCompeatedAction compleatedActionSave = new SerializedCompeatedAction();
		compleatedActionSave.actionAmountInt = attackStrength;
		compleatedActionSave.locationToPreformAction = attackLocation.gridlocation;
		compleatedActionSave.actionType = typeof(AttackScript);
		return compleatedActionSave;
	}
	#endregion


}
