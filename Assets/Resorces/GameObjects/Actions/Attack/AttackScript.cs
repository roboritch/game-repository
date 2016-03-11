using UnityEngine;
using System.Collections;

//TODO Move Action
/// <summary> Attack script.</summary>
public class AttackScript : ActionScript{

	public AttackScript (UnitScript u) : base (u){
	
	}



  /// <summary>
  /// The attack grid block location.
  /// </summary>
  private GridBlock attackLocation;

  /// <summary>
  /// The attack strength; the number of blocks removed by this attack.
  /// </summary>
  private int attackStrength;

  /// <summary>
  /// Sets the attack location.
  /// </summary>
  /// <param name="attackLocation">Attack location.</param>
  public void setAttackLocation(GridBlock attackLocation){
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
		
  /// <summary>
  /// Perform this action when called by the unit's action list.
  /// </summary>
  public override void act(){
    if (attackLocation.unitInstalled != null)
      attackLocation.unitInstalled.removeBlock(attackStrength);
  }


	public override void removeUserSelectionDisplay () {
		
	}

	public override void displayFinishedAction () {

	}

	public override void userHasSelectedTheirActionLocation (GridBlock blockSelected) {
		throw new System.NotImplementedException ();
	}

  /// <summary>
  /// Calls the GUI to display this action on the game.
  /// </summary>
  /// <param name="gui">GUI.</param>
	public override void displayUserSelection(){
	
	}

  /// <summary>
  /// Calls this to get the GUI to remove all displayed images of this action.
  /// </summary>
  /// <param name="gui">GUI.</param>
	public override void removeDisplayFinishedDisplay(){
    
  }

	public override void loadAction (SerializedCompeatedAction s)
	{
		throw new System.NotImplementedException ();
	}

	public override SerializedCompeatedAction serializeAction ()
	{
		throw new System.NotImplementedException ();
	}



}
