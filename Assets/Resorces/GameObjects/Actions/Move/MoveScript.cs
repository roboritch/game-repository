using UnityEngine;
using System.Collections;

//TODO Move Action
/// <summary> Move script.</summary>
public class MoveScript : ActionScript{

  /// <summary>
  /// The direction to move.
  /// </summary>
  private Direction moveDir;

  /// <summary>
  /// Sets the move direction.
  /// </summary>
  /// <param name="moveDir">Move dir.</param>
  public void setMoveDir(Direction moveDir){
    this.moveDir = moveDir;
  }

  /// <summary>
  /// Gets the move direction.
  /// </summary>
  /// <returns>The move dir.</returns>
  public Direction getMoveDir(){
    return moveDir;
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
	display ();
    unit.addBlock(unit.getBlockHeadLocation().getAdj(moveDir));
  }

  /// <summary>
  /// Calls the GUI to display this action on the game.
  /// </summary>
  /// <param name="gui">GUI.</param>
  public override void display(){
	unit.getBlockHeadLocation ().spriteDisplayScript.moveAction (unit);
  }

  /// <summary>
  /// Calls this to get the GUI to remove all displayed images of this action.
  /// </summary>
  /// <param name="gui">GUI.</param>
  public override void removeDisplay(){
		unit.getBlockHeadLocation ().spriteDisplayScript.removeMoveSprite (unit);
  }

}
