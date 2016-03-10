using UnityEngine;
using System.Collections;

//TODO Move Action
/// <summary> Attack script.</summary>
public class AttackScript : ActionScript{

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

  // Use this for initialization
  /// <summary>Start this instance.</summary>
  void Start(){

  }

  // Update is called once per frame
  /// <summary> Update this instance. </summary>
  void Update(){

  }

  /// <summary>
  /// Perform this action when called by the unit's action list.
  /// </summary>
  public override void act(){
    if (attackLocation.unitInstalled != null)
      attackLocation.unitInstalled.removeBlock(attackStrength);
  }

  /// <summary>
  /// Calls the GUI to display this action on the game.
  /// </summary>
  /// <param name="gui">GUI.</param>
  public override void display(){
    
  }

  /// <summary>
  /// Calls this to get the GUI to remove all displayed images of this action.
  /// </summary>
  /// <param name="gui">GUI.</param>
  public override void removeDisplay(){
    
  }
	public override void setUnit(UnitScript newUnit){
		unit = newUnit;
	}

}
