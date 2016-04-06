using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

// http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer

[XmlRoot("Gridblock")]
[Serializable]
public struct GridInfo{
	[XmlIgnore]
	// Whether a block is a spawn spot
	public bool[,] isSpawnSpot;

	[XmlElement("spawn_spot")]
	public bool[][] xmlSpawnSpot{
		get{
			bool[][] jaggedSpawn = new bool[isSpawnSpot.Length][];
			for(int i = 0; i < isSpawnSpot.Length; i++){
				jaggedSpawn[i] = new bool[isSpawnSpot.Length];
				for(int j = 0; j < isSpawnSpot.Length; j++){
					jaggedSpawn[i][j] = (bool)isSpawnSpot[i, j];
				}
			}
			return jaggedSpawn;
		}

	}

	[XmlIgnore]
	// Whether a block is an occupiable space or a wall
	public bool[,] isWall;

	[XmlElement("wall")]
	public bool[][] xmlWall{
		get{
			bool[][] jaggedWall = new bool[isWall.Length][];
			for(int i = 0; i < isWall.Length; i++){
				jaggedWall[i] = new bool[isWall.Length];
				for(int j = 0; j < isWall.Length; j++){
					jaggedWall[i][j] = (bool)isWall[i, j];
				}
			}
			return jaggedWall;
		}
	}

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
		gridInfoFilepath = Application.dataPath + "/level.xml";
		if(!File.Exists(gridInfoFilepath)){
			// Create file with dummy data just for creation.
			// Both bools isSpawnSpot and isWall default to false.
			// Dummy data creates an initial 10x10 grid.
			newGridInfo.gridSize = 10;
			saveGrid();
		}

		loadGrid();
		// New copy of gridInfo to pass into serializer for future saving.
		currentGridInfo = newGridInfo;
	}

	// Set a level name
	public void setLevelName(string levelName){
		gridInfoFilepath = Application.dataPath + "/" + levelName + ".xml";
	}

	// Call grid and save current info into struct...
	public void getGridInfo(){
		for(int x = 0; x < currentGridInfo.gridSize; x++){
			for(int y = 0; y < currentGridInfo.gridSize; y++){
				currentGridInfo.isSpawnSpot[x, y] = gridInfo.gameGrid[x, y].getSpawnSpot();
				//currentGridInfo.xmlSpawnSpot[x][y] = currentGridInfo.isSpawnSpot[x, y];

				currentGridInfo.isWall[x, y] = gridInfo.gameGrid[x, y].getAvailable();
				//currentGridInfo.xmlWall[x][y] = currentGridInfo.isWall[x, y];
			}
		}
		currentGridInfo.gridSize = gridInfo.gridSize;
	}

	#region save and load grid

	public void saveGrid(){
		FileStream stream = null;
		try{
			XmlSerializer serializer = new XmlSerializer(typeof(GridInfo));
			stream = new FileStream(gridInfoFilepath, FileMode.OpenOrCreate);
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

	public void loadGrid(){
		FileStream stream = null;
		try{
			XmlSerializer serializer = new XmlSerializer(typeof(GridInfo));
			stream = new FileStream(gridInfoFilepath, FileMode.Open);
			GridInfo container = (GridInfo)serializer.Deserialize(stream);
			currentGridInfo = container;
			stream.Close();
		} catch(Exception ex){
			Debug.LogError(ex.ToString());
			if(stream != null)
				stream.Close();
		}
	}

	#endregion
}

