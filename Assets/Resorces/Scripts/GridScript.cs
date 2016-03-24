using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
// http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer

[XmlRoot("Gridblock")]
[Serializable]
public struct GridInfo {
	public bool isSpawnSpot; // Whether a block is a spawn spot
	public bool isWall; // Whether a block is an occupiable space or a wall
	public int gridSize; // The width and height (x, y) value of the gridblock
}

public class GridScript : MonoBehaviour {

  // Instance of struct.
	private GridInfo gridInfo;
	private string gridInfoFilepath;
	private GridInfo currentGridInfo;
	public GridInfo newGridInfo;

	// CreatePlayGrid script reference.
	public CreatePlayGrid createPlayGrid;

	// Grab initial gridblock info and load the grid
	void Initialize () {
		gridInfoFilepath = Application.dataPath + "/level.xml";
		if(!File.Exists(gridInfoFilepath)) {
			// Create file with dummy data just for creation.
			// Both bools isSpawnSpot and isWall default to false.
      // Dummy data creates an initial 10x10 grid.
			newGridInfo.gridSize = 10;
			saveGrid();
		}

		loadGrid();
    // New copy of gridInfo to pass into serializer for future saving.
		newGridInfo = currentGridInfo;
	}

	#region save and load grid
	public void saveGrid() {
		FileStream stream = null;
		try {
			XmlSerializer serializer = new XmlSerializer(typeof(GridInfo));
			stream = new FileStream(gridInfoFilepath, FileMode.Create);
      // New grid info to disk here.
			serializer.Serialize(stream, newGridInfo);
			stream.Close();
		} catch(Exception ex) {
			Debug.LogError(ex.ToString());
			if(stream != null)
				stream.Close();
		}
		currentGridInfo = newGridInfo;
	}

	public void loadGrid() {
		FileStream stream = null;
		try {
			XmlSerializer serializer = new XmlSerializer(typeof(GridInfo));
			stream = new FileStream(gridInfoFilepath, FileMode.Open);
			GridInfo container = (GridInfo)serializer.Deserialize(stream);
			currentGridInfo = container;
			stream.Close();
		}catch(Exception ex){
			Debug.LogError(ex.ToString());
			if(stream != null)
				stream.Close();
		}
	}
	#endregion
}

