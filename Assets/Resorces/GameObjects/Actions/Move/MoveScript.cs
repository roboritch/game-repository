using UnityEngine;
using System.Collections;

//TODO Move Action
/// <summary> Move script.</summary>
public class MoveScript : ActionScript{

	/// <summary>
	/// The block the unit is on when this script is called to display user selection.
	/// </summary>
	private GridBlock unitBlock;
	// Up, right, down, left clockwise.
	private GridBlock[] adjBlocks = new GridBlock[4];
	/// <summary>
	/// This is the location the action will be preformed on
	/// </summary>
	private GridBlock locationToPreformAction;
	private SpriteControler[] moveDirectionArms;

	//u is passed to the base constructor.
	public MoveScript(UnitScript u) : base(u){
		actionTime = 1f; 
	}

	/// <summary> Perform this action when called by the unit's action list. </summary>
	public override void act(){
		bool itMoved = unit.addBlock(locationToPreformAction, false);
		removeActionRepresentationDisplay();
		if(itMoved == false){
			unit.resetActionQueue(true);
			Debug.Log("Unit crashed, remaning actions dumped");
		}
	}

	#region User Selection

	/// <summary>
	/// Calls the GUI to display this action on the game.
	/// </summary>
	/// <param name="gui">GUI.</param>
	public override void displayUserSelection(){
		if(unit.movmentRemaning() <= 0){
			Debug.Log("no movment actions remaning");
			return;
		}
		unitBlock = unit.getVirtualBlockHeadLocation();
		checkAndDisplayPossibleUserActions();
	}


	private bool possibleMoveLocation(GridBlock block){
		bool canMove = true;
		if(block == null)
			return false;
		// True if no unit installed.
		canMove = block.unitInstalled == null;
		return canMove;
	}


	private void checkAndDisplayPossibleUserActions(){
		for(int i = 0; i < adjBlocks.Length; i++){
			adjBlocks[i] = unitBlock.getAdj(i);
			// Only display if that location is valid.
			if(possibleMoveLocation(adjBlocks[i])){
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
	public override void userSelectedAction(GridBlock blockSelected){
		for(int i = 0; i < adjBlocks.Length; i++){
			if(adjBlocks[i] == blockSelected){
				locationToPreformAction = adjBlocks[i];
			}
		}
		removeUserSelectionDisplay();
		displayFinishedAction();
		unit.virtualBlockHead = locationToPreformAction;
		unit.addActionToQueue(this);
		unit.moveActionAdded();
	}

	#endregion

	#region Action Save and Load

	public override void loadAction(SerializedCompletedAction s){
		locationToPreformAction = unit.grid.gridLocationToGameGrid(s.locationToPerformAction);
	}

	public override SerializedCompletedAction serializeAction(){
		if(locationToPreformAction == null){
			Debug.LogError("Action is not set yet!");
		}
		SerializedCompletedAction temp = new SerializedCompletedAction();
		temp.locationToPerformAction = locationToPreformAction.gridLocation;
		temp.actionType = typeof(MoveScript);
		return temp;
	}

	#endregion

	//TODO Test past this section.
	public override void displayFinishedAction(){
		moveDirectionArms = locationToPreformAction.spriteDisplayScript.displayMoveOnQueue(unit.getVirtualBlockHeadLocation(), locationToPreformAction);
	}

	public override void removeUserSelectionDisplay(){
		for(int i = 0; i < adjBlocks.Length; i++){
			if(adjBlocks[i] != null){
				adjBlocks[i].actionWaitingForUserInput = null;
				adjBlocks[i].spriteDisplayScript.removeMoveSprite();
			}
		}
	}

	public override void removeActionRepresentationDisplay(){
		moveDirectionArms[0].setColor(Color.clear);
		moveDirectionArms[1].setColor(Color.clear);
		unit.moveActionRemoved();
	}
}
