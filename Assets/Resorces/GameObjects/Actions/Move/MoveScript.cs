using UnityEngine;
using System.Collections;

//TODO Move Action
/// <summary> Move script.</summary>
public class MoveScript : ActionScript {

	/// <summary>
	/// The block the unit is on when this script is called to display user selection.
	/// </summary>
	private GridBlock unitBlock; 
	// up,right,down,left clockwise
	private GridBlock[] adjBlocks = new GridBlock[4];
	/// <summary>
	/// This is the location the action will be preformed on
	/// </summary>
	private GridBlock locationToPreformAction;

	//u is passed to the base constructor
	public MoveScript (UnitScript u) : base(u) {
		
	}

	/// <summary>
	/// The direction to move.
	/// </summary>
	private Direction moveDir;

	/// <summary>
	/// Sets the move direction.
	/// </summary>
	/// <param name="moveDir">Move dir.</param>
	public void setMoveDir (Direction moveDir){
		this.moveDir = moveDir;
	}

	/// <summary>
	/// Gets the move direction.
	/// </summary>
	/// <returns>The move dir.</returns>
	public Direction getMoveDir () {
		return moveDir;
	}

	/// <summary> Perform this action when called by the unit's action list. </summary>
	public override void act () {
		unit.addBlock (unit.getBlockHeadLocation ().getAdj (moveDir));
	}

	#region user selection
	/// <summary>
	/// Calls the GUI to display this action on the game.
	/// </summary>
	/// <param name="gui">GUI.</param>
	public override void displayUserSelection () {
		unitBlock = unit.getBlockHeadLocation();
		checkAndDisplayPossibleUserActions();
	}

	private void checkAndDisplayPossibleUserActions(){
		for (int i = 0; i < adjBlocks.Length; i++) {
			if(adjBlocks[i] != null){ // only display if that location is valid
				if(adjBlocks[i].unitInstalled == null){
					adjBlocks[i].spriteDisplayScript.displayMoveSprite();
					adjBlocks[i].actionWaitingForUserInput = this;
				}
			}
		}
	}

	public override void userHasSelectedTheirActionLocation (GridBlock blockSelected) {
		int selectedBlock;
		for (int i = 0; i < adjBlocks.Length; i++) {
			if(adjBlocks[i] == blockSelected){
				selectedBlock = i;
				break;
			}
		}
		displayFinishedAction();
	}
	#endregion

	#region action save and load
	public override void loadAction (SerializedCompeatedAction s) {
		locationToPreformAction = unit.grid.gameGrid[s.locationToPreformAction.x,s.locationToPreformAction.y];
	}

	public override SerializedCompeatedAction serializeAction () {
		if(locationToPreformAction == null){
			Debug.LogError("Action is not set yet!");
		}
		SerializedCompeatedAction temp = new SerializedCompeatedAction();
		temp.locationToPreformAction = locationToPreformAction.gridlocation;
		return temp;
	}
	#endregion


	public override void displayFinishedAction() {
		
	}

	public override void removeUserSelectionDisplay() {
		unit.getBlockHeadLocation ().spriteDisplayScript.removeMoveSprite ();

	}

	public override void removeDisplayFinishedDisplay() {
		
	}
}
