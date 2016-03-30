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

	//initialize each distance array for each team

	//array of grid blocks: grid.gameGrid
	//for each team

	public AIGrid(CreatePlayGrid grid) {
		this.grid = grid;

		int teams = 2;//TODO proper team indices

		closestDist = new int[teams, grid.gridSize, grid.gridSize]; 
		closestDistHead = new int[teams, grid.gridSize, grid.gridSize];
		dist = new int[teams, grid.gridSize, grid.gridSize];
		distHead = new int[teams, grid.gridSize, grid.gridSize];
	}

	/// <summary>
	/// Calculate all values of this AIGrid.
	/// </summary>
	public void calc() {
		//TODO for each enemy
		foreach(UnitScript unit in grid.units) {
			//Get team number of unit.
			int team = unit.getTeam();
			//Get unit head location.
			int unitHeadX = unit.getCurrentBlockHeadLocation().gridlocation.x;
			int unitHeadY = unit.getCurrentBlockHeadLocation().gridlocation.y;
			foreach(GridBlock block in unit.getBlockList()) {
				//Get the location of each unit block.
				int blockX = block.gridlocation.x;
				int blockY = block.gridlocation.y;
				for(int x = 0; x < grid.gridSize; x++) {
					for(int y = 0; y < grid.gridSize; y++) {
						//Distance from this block to unit.
						int distValue = Math.Abs(x - unitHeadX) + Math.Abs(y - unitHeadY);
						//Distance from this block to unit head.
						int distHeadValue = Math.Abs(x - blockX) + Math.Abs(y - blockY);
						//Only update closest distance if closer or first.
						if(distValue < closestDist[team, x, y] || (x == 0 && y == 0)){
							closestDist[team, x, y] = distValue;
							closestUnit[team, x, y] = unit;
						}
						//Only update closest head distance if closer or first.
						if(distHeadValue < closestDistHead[team, x, y] || (x == 0 && y == 0)){
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

