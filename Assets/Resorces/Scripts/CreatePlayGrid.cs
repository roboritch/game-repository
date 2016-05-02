using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System;

/// <summary>
/// Create play grid.
/// This script also holds all prefabs and assent refrences since there is only one instance of this class.
/// </summary>
public class CreatePlayGrid : MonoBehaviour{
	//Grid attributes.
	public GameObject gridBlock;
	public Transform gridStartPoint;
	public int gridSize;
	public GridBlock[,] gameGrid;
	public Team[] team;
	public GUIScript gui;
	public Transform unitObjectHolder;

	//Attached AIGrid
	public AIGrid aiGrid;

	//Sprite attributes.
	public GameObject spritePrefab;
	public SpritesAndColors spritesAndColors;

	//Unit attributes.
	[SerializeField] private GameObject UnitTimerDisplay;
	public List<UnitScript> units;

	//Editing attributes.
	public bool editModeOn = true;
	public GameObject gridEditMenu;

	//Level saving attributes.
	public string filepath;
	public bool contextMenuUp;

	public GridBlock gridLocationToGameGrid(GridLocation gl){
		if(gl.x < 0 || gl.y < 0 || gl.x >= gridSize || gl.y >= gridSize){
			return null;
		}
		return gameGrid[gl.x, gl.y];
	}


	/// <summary>
	/// Act AI units if idle.
	/// </summary>
	public void actAI(){ //this section breaks the unitQueue, it also apears to be unnecessary
		LinkedList<UnitScript> readyUnits = new LinkedList<UnitScript>();
		foreach( Team t in team ){
			//Continue to next team if not controlled by an AI.
			if(t.ai == null)
				continue;
			t.ai.calc();
			foreach( UnitScript u in t.units )
				if(u.ai != null)
				if(u.readyToAct)
					readyUnits.AddLast(u);
		}
		//Recalculate AI grid if needed.
		if(readyUnits.Count > 0)
			aiGrid.calc();
		//Act ready units. This breakes the unit queue
/*		foreach( UnitScript u in readyUnits ) 
			u.nowReadyToAct();*/
	}



	// Use this for initialization.
	void Start(){
		team = new Team[4];
		team[0] = new Team(Color.red, 0, false); //TODO add AI team boolean here to enable AI team
		team[1] = new Team(Color.blue, 1, false);
		team[2] = new Team(Color.yellow, 2, false);
		team[3] = new Team(Color.green, 3, false);

		//All grid spaces are represented by a game object setup the game grid array.
		if(Player.Instance.dataPathOfLevelTheUserWantsToLoad.EndsWith(".xml"))
			loadLevel(Player.Instance.dataPathOfLevelTheUserWantsToLoad);
		else
			debugLevelLoad();
	}

	private void debugLevelLoad(){
		gridSize = 10;
		gameGrid = new GridBlock[gridSize, gridSize]; 
		GameObject tempObject;      
		for(int x = 0; x < gridSize; x++){
			for(int y = 0; y < gridSize; y++){
				tempObject = Instantiate(gridBlock) as GameObject;
				// Set name for debuging.
				tempObject.name = "gridBlock " + x + "," + y;
				// Start the block relative to the master object.
				tempObject.transform.position = gridStartPoint.position;
				// Move the space to the correct spot.
				tempObject.transform.position = tempObject.transform.position + new Vector3((float)x * 2, 0, (float)y * 2);
				// Parent the grid space to this object.
				tempObject.transform.SetParent(transform); 
				//A pointer to the grid block script from the tempObject is stored in the array for easy access.
				gameGrid[x, y] = tempObject.GetComponent<GridBlock>();
				// Each space has a refrence to this script for easy access.
				gameGrid[x, y].GridManager = this; 
				gameGrid[x, y].gridLocation.x = x;
				gameGrid[x, y].gridLocation.y = y;
			}
		}

		//Setup refrences from one grid block to another to improve unit interaction.
		for(int x = 0; x < gridSize; x++){
			for(int y = 0; y < gridSize; y++){
				if(y + 1 < gridSize){
					gameGrid[x, y].setAdj(Direction.UP, gameGrid[x, y + 1]);
				}
				if(y - 1 >= 0){
					gameGrid[x, y].setAdj(Direction.DOWN, gameGrid[x, y - 1]);
				}
				if(x - 1 >= 0){
					gameGrid[x, y].setAdj(Direction.LEFT, gameGrid[x - 1, y]);
				}
				if(x + 1 < gridSize){
					gameGrid[x, y].setAdj(Direction.RIGHT, gameGrid[x + 1, y]);
				}
			}
		}

		// Setup default spawn blocks for testing purposes.
		gameGrid[2, 2].setSpawn(team[0], false);
		gameGrid[5, 2].setSpawn(team[1], false);
		gameGrid[0, 0].setSpawn(team[2], false);
		gameGrid[1, 1].setSpawn(team[3], false);

		//Initialize AI grid.
		aiGrid = new AIGrid(this);
	}

	public void newLevel(int size){
		gridSize = size;
		// destroy all current grid blocks
		if(gameGrid != null)
			foreach( var item in gameGrid ){
				foreach( Transform child in item.transform ){
					Destroy(child.gameObject);
				}
				Destroy(item.gameObject);
			}

		gameGrid = new GridBlock[size, size]; 
		GameObject tempObject;      
		for(int x = 0; x < size; x++){
			for(int y = 0; y < size; y++){
				tempObject = Instantiate(gridBlock) as GameObject;
				// Set name for debuging.
				tempObject.name = "gridBlock " + x + "," + y;
				// Start the block relative to the master object.
				tempObject.transform.position = gridStartPoint.position;
				// Move the space to the correct spot.
				tempObject.transform.position = tempObject.transform.position + new Vector3((float)x * 2, 0, (float)y * 2);
				// Parent the grid space to this object.
				tempObject.transform.SetParent(transform); 
				//A pointer to the grid block script from the tempObject is stored in the array for easy access.
				gameGrid[x, y] = tempObject.GetComponent<GridBlock>();
				// Each space has a refrence to this script for easy access.
				gameGrid[x, y].GridManager = this; 
				gameGrid[x, y].gridLocation.x = x;
				gameGrid[x, y].gridLocation.y = y;
			}
		}

		

		//Setup refrences from one grid block to another to improve unit interaction.
		for(int x = 0; x < size; x++){
			for(int y = 0; y < size; y++){
				if(y + 1 < size){
					gameGrid[x, y].setAdj(Direction.UP, gameGrid[x, y + 1]);
				}
				if(y - 1 >= 0){
					gameGrid[x, y].setAdj(Direction.DOWN, gameGrid[x, y - 1]);
				}
				if(x - 1 >= 0){
					gameGrid[x, y].setAdj(Direction.LEFT, gameGrid[x - 1, y]);
				}
				if(x + 1 < size){
					gameGrid[x, y].setAdj(Direction.RIGHT, gameGrid[x + 1, y]);
				}
			}
		}
		//Initialize AI grid.
		aiGrid = new AIGrid(this);
	}

	public void loadLevel(string levelXmlPath){
		// destroy all current grid blocks
		if(gameGrid != null)
			foreach( var item in gameGrid ){
				foreach( Transform child in item.transform ){
					Destroy(child.gameObject);
				}
				Destroy(item.gameObject);
			}

		FileStream stream = null;
		GridInfo container;

		try{
			XmlSerializer serializer = new XmlSerializer(typeof(GridInfo));
			stream = new FileStream(levelXmlPath, FileMode.Open);
			container = (GridInfo)serializer.Deserialize(stream);
			stream.Close();
		} catch(Exception ex){
			if(stream != null)
				stream.Close();
			Debug.LogError("level load failed, error:/n" + ex);
			return;
		}

		gridSize = container.gridSize;
		gameGrid = new GridBlock[gridSize, gridSize]; 
		GameObject tempObject;  
		for(int x = 0; x < gridSize; x++){
			for(int y = 0; y < gridSize; y++){
				tempObject = Instantiate(gridBlock) as GameObject;
				// Set name for debuging.
				tempObject.name = "gridBlock " + x + "," + y;
				// Start the block relative to the master object.
				tempObject.transform.position = gridStartPoint.position;
				// Move the space to the correct spot.
				tempObject.transform.position = tempObject.transform.position + new Vector3((float)x * 2, 0, (float)y * 2);
				// Parent the grid space to this object.
				tempObject.transform.SetParent(transform); 
				//A pointer to the grid block script from the tempObject is stored in the array for easy access.
				gameGrid[x, y] = tempObject.GetComponent<GridBlock>();
				// Each space has a refrence to this script for easy access.
				gameGrid[x, y].GridManager = this; 
				gameGrid[x, y].gridLocation.x = x;
				gameGrid[x, y].gridLocation.y = y;	

				// setup level from file
				if(container.enabled[x + y * gridSize] == false)
					gameGrid[x, y].toggleSpaceAvailable();
				if(container.isSpawnSpot[x + y * gridSize] && container.team[x + y * gridSize] != -1 && container.isAI[x + y * gridSize] == false){
					gameGrid[x, y].setSpawn(team[container.team[x + y * gridSize]], false);
				} else if(container.isSpawnSpot[x + y * gridSize] && container.team[x + y * gridSize] != -1 && container.isAI[x + y * gridSize]){
					gameGrid[x, y].setSpawn(team[container.team[x + y * gridSize]], true);
				}
			}
		}

		for(int x = 0; x < gridSize; x++){
			for(int y = 0; y < gridSize; y++){
				if(y + 1 < gridSize){
					gameGrid[x, y].setAdj(Direction.UP, gameGrid[x, y + 1]);
				}
				if(y - 1 >= 0){
					gameGrid[x, y].setAdj(Direction.DOWN, gameGrid[x, y - 1]);
				}
				if(x - 1 >= 0){
					gameGrid[x, y].setAdj(Direction.LEFT, gameGrid[x - 1, y]);
				}
				if(x + 1 < gridSize){
					gameGrid[x, y].setAdj(Direction.RIGHT, gameGrid[x + 1, y]);
				}
			}
		}

		//Initialize AI grid.
		aiGrid = new AIGrid(this);
	}

	/// <summary>
	/// Saves the level with a text file representation.
	/// </summary>
	//UNDONE Almost done, some file path errors.
	public void saveLevel(){
		string levelFile = "";
		levelFile += gridSize + "\n";
		for(int x = 0; x < gridSize; x++){
			for(int y = 0; y < gridSize; y++){
				levelFile += gameGrid[x, y].printSpaceState();
			}
			levelFile += "\n";
		}

	}

	public void setContextMenuFalse(){
		contextMenuUp = false;
	}

	// Update is called once per frame.
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update(){
	}
}