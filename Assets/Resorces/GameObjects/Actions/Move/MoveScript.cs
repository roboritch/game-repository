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
		unit.addBlock (unit.getCurrentBlockHeadLocation ().getAdj (moveDir));
	}

	#region user selection
	/// <summary>
	/// Calls the GUI to display this action on the game.
	/// </summary>
	/// <param name="gui">GUI.</param>
	public override void displayUserSelection () {
		unitBlock = unit.getVirtualBlockHeadLocation();

		checkAndDisplayPossibleUserActions();
	}


	private bool possibleMoveLocation(GridBlock block){
		bool canMove = true;
		if(block == null)
			return false;
		canMove = block.unitInstalled == null; // true if no unit installed
		return canMove;
	}


	private void checkAndDisplayPossibleUserActions(){

		for (int i = 0; i < adjBlocks.Length; i++) {
			adjBlocks[i] = unitBlock.getAdj(i);
			if(possibleMoveLocation(adjBlocks[i])){ // only display if that location is valid
				if(adjBlocks[i].unitInstalled == null){
					adjBlocks[i].spriteDisplayScript.displayMoveSprite();
					adjBlocks[i].actionWaitingForUserInput = this;
				}
			}
		}
	}
	/// <summary>
	/// This is called when a user has selected their actions location.
	/// This tells other blocks to stop waiting for input
	/// </summary>
	/// <param name="blockSelected">Block selected.</param>
	public override void userSelectedAction(GridBlock blockSelected) {
		for (int i = 0; i < adjBlocks.Length; i++) {
			if(adjBlocks[i] == blockSelected){
				locationToPreformAction = adjBlocks[i];

			}
		}
		removeUserSelectionDisplay();
		displayFinishedAction();
		unit.virtualBlockHead = locationToPreformAction;

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

	private SpriteControler[] moveDirectionArms;
	public override void displayFinishedAction() { //TODO test past this section
		moveDirectionArms = locationToPreformAction.spriteDisplayScript.displayMoveOnQueue(unit.getVirtualBlockHeadLocation(),locationToPreformAction);
	}

	public override void removeUserSelectionDisplay() {
		for (int i = 0; i < adjBlocks.Length; i++) {
			if(adjBlocks[i] != null){
				adjBlocks[i].actionWaitingForUserInput = null;
				adjBlocks[i].spriteDisplayScript.removeMoveSprite();
			}
		}
	}

	public override void removeActionRepresentationDisplay() {
		moveDirectionArms[0].setColor(Color.clear);
		moveDirectionArms[1].setColor(Color.clear);
	}
}
