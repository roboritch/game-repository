using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

// http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer


[Serializable]
public struct GridInfo{
	[XmlArrayItemAttribute("enabled")]
	[XmlArrayAttribute("block_enabled_list")]
	public bool[] enabled;

	[XmlArrayItemAttribute("Spawn")]
	[XmlArrayAttribute("SpawnSpot_list")]
	public bool[] isSpawnSpot;

	[XmlArrayItemAttribute("AI")]
	[XmlArrayAttribute("AI_enabled_list")]
	public bool[] isAI;

	[XmlArrayItemAttribute("t_numb")]
	[XmlArrayAttribute("team_numbers")]
	public int[] team;

	[XmlElement("grid_size")]
	// The width and height (x, y) value of the gridblock
	public int gridSize;
}


public class GridScript : MonoBehaviour{

	// Instance of struct.
	[SerializeField] private CreatePlayGrid gridInfo;
	private string gridInfoFilepath;
	private GridInfo currentGridInfo;
	public GridInfo newGridInfo;

	// CreatePlayGrid script reference.
	public CreatePlayGrid createPlayGrid;

	// Grab initial gridblock info and load the grid
	void Initialize(){
		gridInfoFilepath = Application.dataPath + "/levels" + "/level.xml";
		if(!File.Exists(gridInfoFilepath)){
			// Create file with dummy data just for creation.
			// Both bools isSpawnSpot and isWall default to false.
			// Dummy data creates an initial 10x10 grid.
			newGridInfo.gridSize = 10;
			saveGrid();
		}

		loadGrid(gridInfoFilepath);
		// New copy of gridInfo to pass into serializer for future saving.
		currentGridInfo = newGridInfo;
	}

	// Set a level name
	public void setLevelName(string levelName){
		gridInfoFilepath = Application.dataPath + "/levels" + "/" + levelName + ".xml";
	}

	// Call grid and save current info into struct...
	public void getGridInfo(){
		currentGridInfo.gridSize = createPlayGrid.gridSize;

		currentGridInfo.isSpawnSpot = new bool[currentGridInfo.gridSize * currentGridInfo.gridSize];
		currentGridInfo.enabled = new bool[currentGridInfo.gridSize * currentGridInfo.gridSize];
		currentGridInfo.isAI = new bool[currentGridInfo.gridSize * currentGridInfo.gridSize];
		currentGridInfo.team = new int[currentGridInfo.gridSize * currentGridInfo.gridSize];

		for(int x = 0; x < currentGridInfo.gridSize; x++){
			for(int y = 0; y < currentGridInfo.gridSize; y++){
				currentGridInfo.isSpawnSpot[x + y * currentGridInfo.gridSize] = gridInfo.gameGrid[x, y].isSpawnSpot();
				currentGridInfo.enabled[x + y * currentGridInfo.gridSize] = gridInfo.gameGrid[x, y].getAvailable();
				currentGridInfo.isAI[x + y * currentGridInfo.gridSize] = gridInfo.gameGrid[x, y].getAISpawn();
				currentGridInfo.team[x + y * currentGridInfo.gridSize] = gridInfo.gameGrid[x, y].getTeamNumber();
			}
		}
	}

	#region save and load grid

	public void saveGrid(){
		FileStream stream = null;
		try{
			XmlSerializer serializer = new XmlSerializer(typeof(GridInfo));
			stream = new FileStream(gridInfoFilepath, FileMode.CreateNew);
			// New grid info to disk here.
			serializer.Serialize(stream, currentGridInfo);
			stream.Close();
		} catch(Exception ex){
			Debug.LogError(ex.ToString());
			if(stream != null)
				stream.Close();
		}
		currentGridInfo = newGridInfo;
	}

	public void loadGrid(string pathName){
		if(Directory.Exists(pathName)){
			Debug.Log("no level with that name exists");
			return;
		}
		//createPlayGrid.loadLevel(pathName);
	}

	#endregion
}

