using System;
using UnityEngine;

/// <summary>
/// An artificial intelligence attached to a unit.
/// </summary>
public class UnitAI {

	/// <summary>The unit attached to this AI.</summary>
	public UnitScript unit;

	//Attributes of this AI, for decision making.

	/// <summary>
	/// The offense/defense bias of this AI, in percentage.
	/// Compare enemy health to this unit's health to decide.
	/// 0 = defense behavior, 1 = offense behavior.
	/// </summary>
	public double moveDir;
	/// <summary>
	/// The idle movement bias of this AI, in percentage.
	/// Only greater than 0% if at max-length.
	/// 0 = non-idle behavior, 1 = idle behavior.
	/// </summary>
	public double moveIdle;
	/// <summary>
	/// The micro/macro bias of this AI, in percentage.
	/// Based on enemy health, this unit's health, and total number of enemies and team units.
	/// 0 = micro behavior, 1 = macro behavior.
	/// </summary>
	public double moveScope;
	/// <summary>
	/// The global bias of this AI, in percentage.
	/// Based on enemy health, this unit's health, and total number of enemies and team units.
	/// 0 = non-global behavior, 1 = global behavior.
	/// </summary>
	public double moveGlobalScope;
	/// <summary>
	/// The head/body target bias of this AI, in percentage.
	/// Based on enemy health and this unit's health.
	/// 0 = head target behavior, 1 = body target behavior.
	/// </summary>
	public double moveTarget;
	/// <summary>
	/// The micro/macro attack bias of this AI, in percentage.
	/// Based on enemy health.
	/// 0 = micro behavior, 1 = macro behavior.
	/// </summary>
	public double attack;

	//Current state attributes of this AI.

	/// <summary>
	/// The current move behavior of this unit.
	/// </summary>
	private MoveDirBehavior moveDirB;
	/// <summary>
	/// The current move scope behavior of this unit.
	/// </summary>
	private MoveScopeBehavior moveScopeB;
	/// <summary>
	/// The current target behavior of this unit.
	/// </summary>
	private MoveTargetBehavior moveTargetB;
	/// <summary>
	/// The current attack behavior of this unit.
	/// </summary>
	private AttackBehavior attackB;

	public UnitAI(UnitScript unit) {
		this.unit = unit;

		//TODO fix these default values.
		moveDir = 0.5;
		moveIdle = 0.5;
		moveScope = 0.5;
		moveGlobalScope = 0.5;
		moveTarget = 0.5;
		attack = 0.5;

		moveDirB = MoveDirBehavior.IDLE;
		moveScopeB = MoveScopeBehavior.MICRO;
		moveTargetB = MoveTargetBehavior.HEAD;
		attackB = AttackBehavior.MICRO;
	}

	public void aiAct() {

		//for(){//TODO for each team.
		//Check if new minimum is found and change closest distances and units.
		//Increment total distances.
		//}

		//Perform decision making based off of decision parameters decided by overarching AI.
		System.Random random = new System.Random();
		//Check if idle behavior instead of non-idle.
		if(random.NextDouble() > moveIdle) {
			moveDirB = MoveDirBehavior.IDLE;
		} else {
			//Check if offensive movement behavior instead of defensive.
			if(random.NextDouble() > moveDir)
				moveDirB = MoveDirBehavior.TOWARD;
			else
				moveDirB = MoveDirBehavior.AWAY;
		}
		//Check if global movement behavior instead of non-global.
		if(random.NextDouble() > moveGlobalScope) {
			moveScopeB = MoveScopeBehavior.GLOBAL;
		} else {
			//Check if macro movement behavior instead of micro.
			if(random.NextDouble() > moveScope)
				moveScopeB = MoveScopeBehavior.MACRO;
			else
				moveScopeB = MoveScopeBehavior.MICRO;
		}
		//Check if head target behavior instead of body.
		if(random.NextDouble() > moveTarget)
			moveTargetB = MoveTargetBehavior.HEAD;
		else
			moveTargetB = MoveTargetBehavior.BODY;
		//Check if macro behavior instead of micro.
		if(random.NextDouble() > attack)
			attackB = AttackBehavior.MACRO;
		else
			attackB = AttackBehavior.MICRO;


		GridLocation headLoc = unit.getCurrentBlockHeadLocation().gridlocation;
		int headX = headLoc.x;
		int headY = headLoc.y;

		int moves = unit.getUnitMaxMovment();

		//Possible locations to move to.
		GridBlock[,] possibleLocations = new GridBlock[moves * 2 + 1, moves * 2 + 1];
		//Center of matrix is the starting position, the unit head.
		possibleLocations[moves + 1, moves + 1] = headLoc;

		//Get other possible locations.
		//For each number of possible moves.
		for(int m = 0; m < moves; m++) {
			//Horizontal locations.
			for(int mx = -m; mx <= m; mx++) {
				//Vertical locations.
				for(int my = -m; my <= m; my++) {
					//Break if location requires too many moves.
					if(Math.Abs(mx) + Math.Abs(my) > moves)
						break;
				}
			}
		}


		//Apply movement direction behavior.
		switch(moveDirB) {
		case MoveDirBehavior.AWAY:
			break;
		case MoveDirBehavior.TOWARD:
			break;
		default: //case MoveDirBehavior.IDLE:
			break;
		}

		//Apply movement scope behavior.
		switch(moveScopeB) {
		case MoveScopeBehavior.MACRO:
			break;
		case MoveScopeBehavior.GLOBAL:
			break;
		default: //case MoveScopeBehavior.MICRO:
			break;
		}

		//Apply movement target behavior.
		switch(moveTargetB) {
		case MoveTargetBehavior.HEAD:
			break;
		default: //case MoveTargetBehavior.BODY:
			break;
		}

		//Apply attack behavior.
		switch(attackB) {
		case AttackBehavior.MACRO:
			break;
		default: //case AttackBehavior.MICRO:
			break;
		}
	}

	/// <summary>
	/// Descriptive code of this AI unit. Follows the format:
	/// "M:{moveDirectionBehavior},{moveScopeBehavior},{moveTargetBehavior},A:{attackBehavior}"
	/// </summary>
	/// <returns>The code string.</returns>
	public string toString() {
		return "M:" + moveDirB + "," + moveScopeB + "," + moveTargetB + ",A:" + attackB;
	}

}
