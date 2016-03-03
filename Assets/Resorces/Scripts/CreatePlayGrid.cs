using UnityEngine;

/// <summary>
/// Create play grid.
/// This script also holds all prefabs and assent refrences since there is only one instance of this class
/// </summary>
public class CreatePlayGrid : MonoBehaviour {
	public GameObject gridBlock;
	public Transform gridStartPoint;
	public int gridSize;
	public GridBlock[,] gameGrid;

	public GUIScript gui;

	public Transform unitObjectHolder;

	//all the default sprites and colors
	//each unit's script holds there own sprite and color

	#region sprites

	//all this must be setup in the inspector
	public GameObject spritePrefab;

	public SpritesAndColors spritesAndColors;

	#endregion

	#region units

	public UnitInformationStruct[] units;

	#endregion

	//use regions to improve readability of large code sections

	#region edit

	public bool editModeOn = true;
	public GameObject gridEditMenu;

	#endregion

	#region start

	// Use this for initialization
	void Start() {

		/*
		 * all grid spaces are represented by a game object
		 * setup the game grid array */
		gameGrid = new GridBlock[gridSize, gridSize]; 
		GameObject tempObject; 		
		for(int x = 0; x < gridSize; x++) { //
			for(int y = 0; y < gridSize; y++) {
				tempObject = Instantiate(gridBlock) as GameObject; // game objects must be 
				tempObject.name = "gridBlock " + x + "," + y; // set name for debuging
				tempObject.transform.position = gridStartPoint.position; // start the block relative to the master object
				tempObject.transform.position = tempObject.transform.position + new Vector3((float)x * 2, 0, (float)y * 2); // move the space to the correct spot 
				tempObject.transform.SetParent(transform); // parent the grid space to this object
				gameGrid[x, y] = tempObject.GetComponent<GridBlock>(); //a pointer the grid block script from the tempObject is stored in the array for easy access
				gameGrid[x, y].GridManager = this; // each space has a refrence to this script for easy access
				gameGrid[x, y].gridlocation.x = x;
				gameGrid[x, y].gridlocation.y = y;
			}
		}
		//setup refrences from one grid block to another to improve unit interaction
		for(int x = 0; x < gridSize; x++) {
			for(int y = 0; y < gridSize; y++) {
				if(y + 1 < gridSize) {
					gameGrid[x, y].up = gameGrid[x, y + 1];
				}
				if(y - 1 >= 0) {
					gameGrid[x, y].down = gameGrid[x, y - 1];
				}
				if(x + 1 < gridSize) {
					gameGrid[x, y].left = gameGrid[x + 1, y];
				}
				if(x - 1 >= 0) {
					gameGrid[x, y].right = gameGrid[x - 1, y];
				}
			}
		}
	}

	#endregion

	#region levelSaving

	public string filepath;
	public bool contextMenuUp;

	/// <summary>
	/// saves the level with a text file representation
	/// </summary>
	public void saveLevel() { //UNDONE almost done, some file path errors
		string levelFile = "";
		levelFile += gridSize + "\n";
		for(int x = 0; x < gridSize; x++) {
			for(int y = 0; y < gridSize; y++) {
				levelFile += gameGrid[x, y].printSpaceState();
			}
			levelFile += "\n";
		}

	}

	#endregion


	// Update is called once per frame
	void Update() {

	}
}

#region sprites and colors
/// <summary>
/// struct filled with all the sprites and colors
/// </summary>
[System.Serializable]
public struct SpritesAndColors {
	public Sprite sprite_unit;
	public Sprite sprite_unitConecter;

	public Sprite sprite_defaultSpace;
	public Color color_defaultSpaceColor;

	public Sprite sprite_spawnSpace;
	public Color color_spawnSpaceColor;

	public Sprite sprite_moveTo;
	public Color color_move;

	public Sprite sprite_moveLine;

	public Sprite sptite_moveCircle;

	public Sprite sprite_attack;
	public Color color_attack;
}
#endregion