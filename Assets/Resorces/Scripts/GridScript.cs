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
	// Whether a block is a spawn spot
	public bool[] isSpawnSpot;

	// Whether a block is an occupiable space or a wall
	public bool[] isWall;

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
		currentGridInfo.gridSize = gridInfo.gridSize;

		currentGridInfo.isSpawnSpot = new bool[(int)Math.Pow(currentGridInfo.gridSize, 2)];
		currentGridInfo.isWall = new bool[(int)Math.Pow(currentGridInfo.gridSize, 2)];

		for(int x = 1; x < currentGridInfo.gridSize; x++){
			for(int y = 1; y < currentGridInfo.gridSize; y++){
				currentGridInfo.isSpawnSpot[x * y] = gridInfo.gameGrid[x, y].getSpawnSpot();
				currentGridInfo.isWall[x * y] = gridInfo.gameGrid[x, y].getAvailable();
			}
		}
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

