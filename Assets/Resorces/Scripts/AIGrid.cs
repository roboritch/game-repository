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
	int[,,] closestDist;
	/// <summary>
	///The distance of each gridblock to the closest unit head of a team.
	/// </summary>
	int[,,] closestDistHead;

	//Distances are used for macro AI behavior.
	/// <summary>
	///The summed distance of each gridblock to every unit of a team.
	/// </summary>
	int[,,] dist;
	/// <summary>
	///The summed distance of each gridblock to every unit head of a team.
	/// </summary>
	int[,,] distHead;

	//initialize each distance array for each team

	//array of grid blocks: grid.gameGrid
	//for each team

	public AIGrid(CreatePlayGrid grid) {
		this.grid = grid;

		int teams = 2;//TODO proper team indices

		closestDist = new int[teams, grid.gridSize, grid.gridSize]; 
		closestDist = new int[teams, grid.gridSize, grid.gridSize];
		closestDistHead = new int[teams, grid.gridSize, grid.gridSize];
		dist = new int[teams, grid.gridSize, grid.gridSize];
		distHead = new int[teams, grid.gridSize, grid.gridSize];
	}

	/// <summary>
	/// Calculate all values of this AIGrid.
	/// </summary>
	public void calc(){
		//TODO for each enemy
		foreach( UnitScript in grid.units){
			
		}
		int team = 0; //TODO get team number from unit

	}
}

