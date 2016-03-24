using UnityEngine;
using System.Collections;

/// <summary> Attack script.</summary>
public class AttackScript : ActionScript{

	public AttackScript (UnitScript u) : base (u){
		attackStrength = unit.getAttackPower();
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
		removeActionRepresentationDisplay();
		if(attackLocation != null)
		if (attackLocation.unitInstalled != null){
			if(attackLocation.isAdj(unit.getCurrentBlockHeadLocation())){
				displayCloseRangeAttackAnimation(attackLocation);
				attackLocation.unitInstalled.queueBlockRemoval(attackStrength,actionTime,0f); // must be called after the display
			}else{
				float delay = displayLongRangeAttackAnimation(attackLocation);
				attackLocation.unitInstalled.queueBlockRemoval(attackStrength,actionTime,delay);
			}
		}else{
			Debug.Log("unit's block was removed before it could be attacked");
		}
	}

	private void displayCloseRangeAttackAnimation(GridBlock animationLocation){
		GameObject closeAttack = unit.instantiationHelper(unit.grid.getAnimation("close attack"));
		actionTime = closeAttack.GetComponent<IGetAnimationTimeToFin>().getAnimationTime();
		closeAttack.transform.SetParent(animationLocation.transform,false);
		closeAttack.transform.localPosition = new Vector3();
		closeAttack.transform.position = closeAttack.transform.position + new Vector3(0f,1f,0f);
	}


	//TODO long range attack animation not done
	private float displayLongRangeAttackAnimation(GridBlock animationLocation){
		GameObject farAttackOut = unit.instantiationHelper(unit.grid.getAnimation("far attack out")); // diplayed on the units block
		GameObject farAttackIn = unit.instantiationHelper(unit.grid.getAnimation("far attack in")); // diplayed on the animationLocation
		float delay = farAttackOut.GetComponent<IGetAnimationTimeToFin>().getAnimationTime();
		actionTime =farAttackIn.GetComponent<IGetAnimationTimeToFin>().getAnimationTime();
		farAttackOut.transform.SetParent(unit.getCurrentBlockHeadLocation().transform,false);
		farAttackOut.transform.localPosition = new Vector3();
		farAttackOut.transform.position = farAttackOut.transform.position + new Vector3(0f,1f,0f);

		farAttackIn.transform.SetParent(animationLocation.transform,false);
		farAttackIn.transform.localPosition = new Vector3();
		farAttackIn.transform.position = farAttackOut.transform.position + new Vector3(0f,1f,0f);

		return delay;
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
		posibleAttackLocations = unit.getAttackLocations();
	}

	#endregion

	private void checkAndDisplayPossibleUserActions(){
		for (int i = 0; i < posibleAttackLocations.Length; i++) {
			posibleAttackLocations[i].attackActionWantsToAttackHere(this,unit);
		}
	}
		

	public override void removeUserSelectionDisplay () {
		if(posibleAttackLocations != null)
		for (int i = 0; i < posibleAttackLocations.Length; i++) {
			posibleAttackLocations[i].removeAttackDisplayForThis();
		}
	}

	#endregion

	#region finished action display
	private int actionDisplayID = -1; //for when enamy actions are loaded
	public override void displayFinishedAction () {
		actionDisplayID = attackLocation.spriteDisplayScript.displayAction(unit.grid.spritesAndColors.sprite_attack);
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
		removeUserSelectionDisplay(); // remove the selection display
		attackLocation = blockSelected; // set attack location
		displayFinishedAction(); // display the finished action
		unit.addActionToQueue(this); // add the action to the queue
		unit.useAttackAction(); //the unit can no longer attack
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
