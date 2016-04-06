using UnityEngine;
using System.Collections.Generic;

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


	//Animation attributes.
	#pragma warning disable
	[SerializeField] private Animations[] animationLibrarySetter;
	private Dictionary<string,GameObject> animationLibrary = new Dictionary<string, GameObject>(10);

	public GridBlock gridLocationToGameGrid(GridLocation gl){
		int x=gl.x;
		int y=gl.y;
		if (x < 0 || y < 0 || x > gridSize || y > gridSize) {
			return null;
		}
		return gameGrid[gl.x, gl.y];
	}

	private void animationLibrarySetup(){
		for(int i = 0; i < animationLibrarySetter.Length; i++){
			animationLibrary.Add(animationLibrarySetter[i].animationName, animationLibrarySetter[i].animation);
		}
	}

	/// <summary>
	/// Act AI units if idle.
	/// </summary>
	public void actAI() {
		LinkedList<UnitScript> readyUnits = new LinkedList<UnitScript>();
		foreach(Team t in team)
			foreach(UnitScript u in t.units)
				if(u.ai != null)
				if(u.readyToAct)
					readyUnits.AddLast(u);
		//Recalculate AI grid if needed.
		if(readyUnits.Count > 0)
			aiGrid.calc();
		//Act ready units.
		foreach(UnitScript u in readyUnits)
			u.nowReadyToAct();
	}

	/// <summary>
	/// Animations the library.
	/// </summary>
	/// <returns>An uninitalized prefab for the animation. Null if no animation with that name exists.</returns>
	public GameObject getAnimation(string animationName){
		GameObject animation;
		bool getAnimationSuccess = animationLibrary.TryGetValue(animationName, out animation);
		if(getAnimationSuccess){
			return animation;
		} else{
			return null;
		}
	}

	// Use this for initialization.
	void Start(){
		team = new Team[4];
		team [0] = new Team (Color.red,0);
		team [1] = new Team (Color.blue,1);
		team [2] = new Team (Color.yellow,2);
		team [3] = new Team (Color.green,3);

		animationLibrarySetup();
		//All grid spaces are represented by a game object setup the game grid array.
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
		gameGrid[2, 2].setSpawn(team[0]);
		gameGrid[5, 2].setSpawn(team[1]);
		gameGrid [0, 0].setSpawn (team [2]);
		gameGrid [1, 1].setSpawn (team [3]);

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