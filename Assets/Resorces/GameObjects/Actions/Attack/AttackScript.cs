using UnityEngine;
using System.Collections;

//TODO Move Action
/// <summary> Attack script.</summary>
public class AttackScript : ActionScript{

  /// <summary>
  /// The attack grid block location.
  /// </summary>
  GridBlock attackLocation;

  /// <summary>
  /// The attack strength; the number of blocks removed by this attack.
  /// </summary>
  int attackStrength;

  // Use this for initialization
  /// <summary>Start this instance.</summary>
  void Start(){

  }

  // Update is called once per frame
  /// <summary> Update this instance. </summary>
  void Update(){

  }

  public override void act(){
    if (unit != null){
      unit.removeBlock(attackStrength);
    }
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

}
