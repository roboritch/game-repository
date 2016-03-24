using UnityEngine;
using System.Collections;

/// <summary> Attack script.</summary>
public class AttackScript : ActionScript{

  //Attack attributes.
  /// <summary>
  /// The attack grid block location.
  /// </summary>
  private GridBlock attackLocation;
  /// <summary>
  /// The attack strength; the number of blocks removed by this attack.
  /// </summary>
  private int attackStrength = 0;
  private GridBlock[] posibleAttackLocations;
  private int actionDisplayID = -1;

  public AttackScript(UnitScript u)
    : base(u){
    attackStrength = unit.getAttackPower();
  }

  #region Attack Strength and Location

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
    if (attackLocation != null)
    if (attackLocation.unitInstalled != null){
      if (attackLocation.isAdj(unit.getCurrentBlockHeadLocation())){
        displayCloseRangeAttackAnimation(attackLocation);
        // Must be called after the display.
        attackLocation.unitInstalled.queueBlockRemoval(attackStrength, actionTime);
      } else{
        //displayLongRangeAttackAnimation();
        attackLocation.unitInstalled.removeBlock(attackStrength);
      }
    } else{
      Debug.Log("unit's block was removed before it could be attacked");
    }
  }

  private void displayCloseRangeAttackAnimation(GridBlock animationLocation){
    GameObject closeAttack = unit.instantiationHelper(unit.grid.getAnimation("close attack"));
    actionTime = closeAttack.GetComponent<IGetAnimationTimeToFin>().getAnimationTime();
    closeAttack.transform.SetParent(animationLocation.transform, false);
    closeAttack.transform.localPosition = new Vector3();
    closeAttack.transform.position = closeAttack.transform.position + new Vector3(0f, 1f, 0f);
  }


  //TODO Long range attack animation not done.
  private void displayLongRangeAttackAnimation(GridBlock animationLocation){
    // Diplayed on the units block.
    GameObject farAttackOut = unit.grid.getAnimation("far attack out");
    // Diplayed on the animationLocation.
    GameObject farAttackIn = unit.grid.getAnimation("far attack in");
    actionTime = farAttackOut.GetComponent<IGetAnimationTimeToFin>().getAnimationTime();
    actionTime += farAttackIn.GetComponent<IGetAnimationTimeToFin>().getAnimationTime();
  }


  #region User Display

  /// <summary>
  /// Calls the GUI to display this action on the game.
  /// </summary>
  /// <param name="gui">GUI.</param>
  public override void displayUserSelection(){
    if (unit.attacksRemaning() <= 0){
      Debug.Log("no attack actions remaning");
      return;
    }
    setPosibleAttackLocations();
    checkAndDisplayPossibleUserActions();
  }

  #region Set Possible Attack Locations

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
    for (int i = 0; i < posibleAttackLocations.Length; i++){
      posibleAttackLocations[i].attackActionWantsToAttackHere(this, unit);
    }
  }


  public override void removeUserSelectionDisplay(){
    if (posibleAttackLocations != null)
      for (int i = 0; i < posibleAttackLocations.Length; i++){
        posibleAttackLocations[i].removeAttackDisplayForThis();
      }
  }

  #endregion

  #region Finished Action Display
  //For when enamy actions are loaded.
  public override void displayFinishedAction(){
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


  public override void userSelectedAction(GridBlock blockSelected){
    //Remove the selection display.
    removeUserSelectionDisplay();
    //Set attack location.
    attackLocation = blockSelected;
    //Display the finished action.
    displayFinishedAction();
    //Add the action to the queue.
    unit.addActionToQueue(this);
    //The unit can no longer attack.
    unit.useAttackAction();
  }

  #region Action Save And Load

  public override void loadAction(SerializedCompletedAction s){
    attackStrength = s.actionAmountInt;
    attackLocation = unit.grid.gridLocationToGameGrid(s.locationToPerformAction);
  }

  public override SerializedCompletedAction serializeAction(){
    SerializedCompletedAction compleatedActionSave = new SerializedCompletedAction();
    compleatedActionSave.actionAmountInt = attackStrength;
    compleatedActionSave.locationToPerformAction = attackLocation.gridlocation;
    compleatedActionSave.actionType = typeof(AttackScript);
    return compleatedActionSave;
  }

  #endregion
>>>>>>> refs/remotes/origin/master


}
