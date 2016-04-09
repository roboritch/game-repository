using System;

/// <summary>
/// Values of a play grid used by AI units.
/// </summary>
public class AIGrid {

	//The grid to calculate values from.
	CreatePlayGrid grid;

	//Closest distances are used for micro AI behavior.
	/// <summary>
	/// The distance of each gridblock to the closest unit of a team.
	/// </summary>
	private int[,,] closestDist;
	/// <summary>
	///The distance of each gridblock to the closest unit head of a team.
	/// </summary>
	private int[,,] closestDistHead;
	/// <summary>
	///The closest unit to each gridblock.
	/// </summary>
	private UnitScript[,,] closestUnit;
	/// <summary>
	///The closest unit head to each gridblock.
	/// </summary>
	private UnitScript[,,] closestUnitHead;

	//Distances are used for macro AI behavior.
	/// <summary>
	///The summed distance of each gridblock to every unit of a team.
	/// </summary>
	private int[,,] dist;
	/// <summary>
	///The summed distance of each gridblock to every unit head of a team.
	/// </summary>
	private int[,,] distHead;

	int teamCount = 4;
	//TODO proper team indices

	/// <summary>
	/// Gets the grid of closest distances.
	/// </summary>
	/// <returns>The closest dist.</returns>
	/// <param name="team">Team.</param>
	/// <param name="ally">If set to <c>true</c> ally.</param>
	/// <param name="head">If set to <c>true</c> head.</param>
	public int[,] getClosestDist(int team, bool ally, bool head) {
		if(head)
			return getGridMin(closestDistHead, team, ally);
		else
			return getGridMin(closestDist, team, ally);
	}

	/// <summary>
	/// Gets the grid of closest units.
	/// </summary>
	/// <returns>The closest dist.</returns>
	/// <param name="team">Team.</param>
	/// <param name="ally">If set to <c>true</c> ally.</param>
	/// <param name="head">If set to <c>true</c> head.</param>
	public UnitScript[,] getClosestUnit(int team, bool ally, bool head) {
		if(head)
			return getGridMinUnit(closestDistHead, closestUnitHead, team, ally);
		else
			return getGridMinUnit(closestDist, closestUnit, team, ally);
	}

	/// <summary>
	/// Gets the grid of summed distances.
	/// </summary>
	/// <returns>The dist.</returns>
	/// <param name="team">Team.</param>
	/// <param name="ally">If set to <c>true</c> ally.</param>
	/// <param name="head">If set to <c>true</c> head.</param>
	public int[,] getDist(int team, bool ally, bool head) {
		if(head)
			return getGridSum(distHead, team, ally);
		else
			return getGridSum(dist, team, ally);
	}

	/// <summary>
	/// Gets the grid sum given an input grid.
	/// </summary>
	/// <returns>The grid sum.</returns>
	/// <param name="inputGrid">Input grid.</param>
	/// <param name="team">Team.</param>
	/// <param name="ally">If set to <c>true</c> ally.</param>
	private int[,] getGridSum(int[,,] inputGrid, int team, bool ally) {
		//The summed distance grid.
		int[,] gridSum = new int[grid.gridSize, grid.gridSize];

		//Check if no sum is needed (only ally distances).
		if(ally) {
			for(int x = 0; x < grid.gridSize; x++) {
				for(int y = 0; y < grid.gridSize; y++) {
					gridSum[x, y] = inputGrid[team, x, y];
				}
			}
		} else {
			//For each team.
			for(int t = 0; t < teamCount; t++) {
				//Don't sum the ally team.
				if(t == team)
					continue;
				for(int x = 0; x < grid.gridSize; x++) {
					for(int y = 0; y < grid.gridSize; y++) {
						gridSum[x, y] += inputGrid[t, x, y];
					}
				}
			}
		}
		return gridSum;
	}

	/// <summary>
	/// Gets the minimum grid given an input grid.
	/// </summary>
	/// <returns>The grid minimum.</returns>
	/// <param name="inputGrid">Input grid.</param>
	/// <param name="team">Team.</param>
	/// <param name="ally">If set to <c>true</c> ally.</param>
	private int[,] getGridMin(int[,,] inputGrid, int team, bool ally) {
		//The minimum distance grid.
		int[,] gridMin = new int[grid.gridSize, grid.gridSize];

		//Check if no min is needed (only ally distances).
		if(ally) {
			for(int x = 0; x < grid.gridSize; x++) {
				for(int y = 0; y < grid.gridSize; y++) {
					gridMin[x, y] = inputGrid[team, x, y];
				}
			}
		} else {
			//For each team.
			for(int t = 0; t < teamCount; t++) {
				//Don't min the ally team.
				if(t == team)
					continue;
				for(int x = 0; x < grid.gridSize; x++) {
					for(int y = 0; y < grid.gridSize; y++) {
						int amount = inputGrid[t, x, y];
						//If amount is less or uninitialized, set minimum.
						if(amount < gridMin[x, y] || ((team == 0 && t == 1) || t == 0)){
							gridMin[x, y] = inputGrid[t, x, y];
						}
					}
				}
			}
		}
		return gridMin;
	}

	/// <summary>
	/// Gets the closest unit grid given input grids.
	/// </summary>
	/// <returns>The grid minimum.</returns>
	/// <param name="inputGrid">Input grid.</param>
	/// <param name="team">Team.</param>
	/// <param name="ally">If set to <c>true</c> ally.</param>
	private UnitScript[,] getGridMinUnit(int[,,] inputGrid, UnitScript[,,] unitGrid, int team, bool ally) {
		//The minimum distance grid.
		int[,] gridMin = new int[grid.gridSize, grid.gridSize];
		//The minimum distance unit grid.
		UnitScript[,] gridMinUnit = new UnitScript[grid.gridSize, grid.gridSize];

		//Check if no min is needed (only ally distances).
		if(ally) {
			for(int x = 0; x < grid.gridSize; x++) {
				for(int y = 0; y < grid.gridSize; y++) {
					gridMinUnit[x, y] = unitGrid[team, x, y];
				}
			}
		} else {
			//For each team.
			for(int t = 0; t < teamCount; t++) {
				//Don't min the ally team.
				if(t == team)
					continue;
				for(int x = 0; x < grid.gridSize; x++) {
					for(int y = 0; y < grid.gridSize; y++) {
						int amount = inputGrid[t, x, y];
						//If amount is less or uninitialized, set minimum.
						if(amount < gridMin[x, y] || ((team == 0 && t == 1) || t == 0)){
							gridMin[x, y] = inputGrid[t, x, y];
							gridMinUnit[x, y] = unitGrid[t, x, y];
						}
					}
				}
			}
		}
		return gridMinUnit;
	}

	public AIGrid(CreatePlayGrid grid) {
		this.grid = grid;

		closestDist = new int[teamCount, grid.gridSize, grid.gridSize]; 
		closestDistHead = new int[teamCount, grid.gridSize, grid.gridSize];
		closestUnit = new UnitScript[teamCount, grid.gridSize, grid.gridSize]; 
		closestUnitHead = new UnitScript[teamCount, grid.gridSize, grid.gridSize];
		dist = new int[teamCount, grid.gridSize, grid.gridSize];
		distHead = new int[teamCount, grid.gridSize, grid.gridSize];
	}

	/// <summary>
	/// Calculate all values of this AIGrid.
	/// </summary>
	public void calc() {
		//TODO for each enemy
		foreach(UnitScript unit in grid.units) {
			//Continue to next unit if current is null.
			if(unit == null)
				continue;
			//Get team number of unit.
			int team = unit.getTeam().getIndex();
			//Continue to next unit if current head is null.
			if(unit.getCurrentBlockHeadLocation() == null)
				continue;
			//Get unit head location.
			int unitHeadX = unit.getCurrentBlockHeadLocation().gridLocation.x;
			int unitHeadY = unit.getCurrentBlockHeadLocation().gridLocation.y;
			foreach(GridBlock block in unit.getBlockList()) {
				//Get the location of each unit block.
				int blockX = block.gridLocation.x;
				int blockY = block.gridLocation.y;
				for(int x = 0; x < grid.gridSize; x++) {
					for(int y = 0; y < grid.gridSize; y++) {
						//Distance from this block to unit.
						int distValue = Math.Abs(x - unitHeadX) + Math.Abs(y - unitHeadY);
						//Distance from this block to unit head.
						int distHeadValue = Math.Abs(x - blockX) + Math.Abs(y - blockY);
						//Only update closest distance if closer or first.
						if(distValue < closestDist[team, x, y] || (x == 0 && y == 0)) {
							closestDist[team, x, y] = distValue;
							closestUnit[team, x, y] = unit;
						}
						//Only update closest head distance if closer or first.
						if(distHeadValue < closestDistHead[team, x, y] || (x == 0 && y == 0)) {
							closestDistHead[team, x, y] = distHeadValue;
							closestUnitHead[team, x, y] = unit;
						}
						//Increment global distances.
						dist[team, x, y] += distValue;
						distHead[team, x, y] += distHeadValue;
					}
				}
			}
		}
	}
}

