﻿using System;
using UnityEngine;
using System.Collections.Generic;

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
	/// The ally/enemy target bias of this AI, in percentage.
	/// Based on enemy health and this unit's health.
	/// 0 = ally target behavior, 1 = enemy target behavior.
	/// </summary>
	public double moveTeam;
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
	/// The current target team behavior of this unit.
	/// </summary>
	private MoveTeamBehavior moveTeamB;
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
		moveTeam = 0.5;
		attack = 0.5;

		moveDirB = MoveDirBehavior.IDLE;
		moveScopeB = MoveScopeBehavior.MICRO;
		moveTargetB = MoveTargetBehavior.HEAD;
		moveTeamB = MoveTeamBehavior.ENEMY;
		attackB = AttackBehavior.MICRO;
	}

	public void aiAct() {

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
		//Check if ally target behavior instead of enemy.
		if(random.NextDouble() > moveTeam)
			moveTeamB = MoveTeamBehavior.ALLY;
		else
			moveTeamB = MoveTeamBehavior.ENEMY;
		//Check if macro behavior instead of micro.
		if(random.NextDouble() > attack)
			attackB = AttackBehavior.MACRO;
		else
			attackB = AttackBehavior.MICRO;

		//Get the list of blocks to move to.
		LinkedList<GridBlock> moveBlockList = getMoveBlockList();

		foreach(GridBlock b in moveBlockList) 
			MoveScript ms = new MoveScript(unit, b);

		//Apply attack behavior. TODO
		switch(attackB) {
		case AttackBehavior.MACRO:
			break;
		default: //case AttackBehavior.MICRO:
			break;
		}
	}

	/// <summary>
	/// Gets a list of GridBlocks to move to in sequence, depending on scope behavior.
	/// </summary>
	/// <returns>List of move locations.</returns>
	private LinkedList<GridBlock> getMoveBlockList() {
		//The list of moves to perform.
		LinkedList<GridBlock> gridBlockMoves = new LinkedList<GridBlock>();
		//Get the proper distance grid to movement target.
		int[,] targetGrid = getTargetGrid();
		//Get the unit head location and max possible moves to determine possible intermediate and final locations.
		GridBlock headLoc = unit.getCurrentBlockHeadLocation();
		int moves = unit.getUnitMaxMovment();
		//Create a move tree to assemble a list of possible finishing move locations.
		MoveTree moveTree = new MoveTree(headLoc, moves);
		//Get the final position to move to.
		Position finalPos = getPositionByDirection(moveTree.finalPositions, targetGrid);
		//Return empty list if no final position determined.
		if(finalPos == null)
			return gridBlockMoves;
		//Add all gridblocks from path position data.
		Position currPos = finalPos;
		do {
			gridBlockMoves.AddLast(currPos.getGridBlock());
			currPos = currPos.prevPos;
		} while(currPos != null);
		return gridBlockMoves;
	}

	/// <summary>
	/// Gets the appropriate position from a list, depending on the current move direction behavior.
	/// </summary>
	/// <returns>The position to move to.</returns>
	/// <param name="positions">Possible positions to move to.</param>
	private Position getPositionByDirection(LinkedList<Position> positions, int[,] targetGrid) {
		//Apply movement direction behavior.
		switch(moveDirB) {
		case MoveDirBehavior.AWAY:
			int minDist = 0; //Assign to remove warning.
			Position minPos = null;
			foreach(Position pos in positions) {
				GridLocation posLoc = pos.getGridBlock().gridLocation;
				int dist = targetGrid[posLoc.x, posLoc.y];
				if(dist < minDist || minPos == null) {
					minDist = dist;
					minPos = pos;
				}
			}
			return minPos;
		case MoveDirBehavior.TOWARD:
			int maxDist = 0;
			Position maxPos = null;
			foreach(Position pos in positions) {
				GridLocation posLoc = pos.getGridBlock().gridLocation;
				int dist = targetGrid[posLoc.x, posLoc.y];
				if(dist < maxDist || maxPos == null) {
					maxDist = dist;
					maxPos = pos;
				}
			}
			return maxPos;
		default: //case MoveDirBehavior.IDLE:
			//Do nothing, use null position for idle.
			return null;
		}
	}

	/// <summary>
	/// Gets the target distance grid from the AIGrid based off of scope and team behavior.
	/// </summary>
	/// <returns>The target grid.</returns>
	private int[,] getTargetGrid() {
		//The target distance grid.
		int[,] targetGrid;// = new int[unit.grid.gridSize,unit.grid.gridSize];
		//Whether to target ally or enemy, depending on movement team behavior.
		bool ally = moveTeamB == MoveTeamBehavior.ALLY;
		//Whether to target head or body, depending on movement target behavior.
		bool head = moveTargetB == MoveTargetBehavior.HEAD;
		//Get this unit's team index.
		int teamIndex = unit.getTeam().getIndex();

		//Apply movement scope behavior.
		switch(moveScopeB) {
		case MoveScopeBehavior.MACRO:
			targetGrid = unit.grid.aiGrid.getDist(teamIndex, ally, head);
			break;
		case MoveScopeBehavior.GLOBAL:
			targetGrid = unit.grid.aiGrid.getDist(teamIndex, ally, head);
			break;
		default: //case MoveScopeBehavior.MICRO:
			targetGrid = unit.grid.aiGrid.getClosestDist(teamIndex, ally, head);
			break;
		}
		return targetGrid;
	}

	/// <summary>
	/// Descriptive code of this AI unit. Follows the format:
	/// "M:{moveDirectionBehavior},{moveScopeBehavior},{moveTargetBehavior},A:{attackBehavior}"
	/// </summary>
	/// <returns>The code string.</returns>
	public string toString() {
		return "M:" + moveDirB + "," + moveScopeB + "," + moveTargetB + ",A:" + attackB;
	}

	public class MoveTree {
		private Position start;
		public LinkedList<Position> finalPositions;

		public MoveTree(GridBlock startGB, int depth) {
			finalPositions = new LinkedList<Position>();
			this.start = new Position(startGB, depth, null, finalPositions);
		}
	}

	/// <summary>
	/// Current position of a move sequence path.
	/// </summary>
	public class Position {
		//Gridblock at current position.
		private GridBlock gridBlock;
		//Adjacent positions to this position.
		private Position[] adjPos;
		//Previous positions to this position.
		public Position prevPos;

		public GridBlock getGridBlock() {
			return gridBlock;
		}

		/// <summary>
		/// Initializes a new position.
		/// </summary>
		/// <param name="gridBlock">Grid block of position.</param>
		/// <param name="depth">Remaining depth to traverse.</param>
		/// <param name="prevPos">Previous position in traversal.</param>
		/// <param name="finalPositions">The final positions list of the move tree.</param>
		public Position(GridBlock gridBlock, int depth, Position prevPos, LinkedList<Position> finalPositions) {
			this.gridBlock = gridBlock;
			//Decrease the number of remaining moves.
			depth--;
			adjPos = new Position[4];
			//Only get adjacent positions if more moves remain.
			if(depth > 0) {
				//For each adjacent gridblock.
				for(int i = 0; i < 4; i++) {
					//Get the adjacent gridblock, if it exists.
					GridBlock adjGridBlock = gridBlock.getAdj(i);
					//Check if the adjacent gridblock exists, and if it can be occupied.
					if(adjGridBlock != null && adjGridBlock.unitInstalled == false)
						//Set the adjacent gridblock as a space that can be moved to.
						adjPos[i] = new Position(adjGridBlock, depth, this, finalPositions);
				}
			} else {
				//If no more moves remain, add to final positions.
				finalPositions.AddLast(this);
			}
		}
	}

}
