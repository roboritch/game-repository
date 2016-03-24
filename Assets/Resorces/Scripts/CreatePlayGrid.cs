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
  public GUIScript gui;
  public Transform unitObjectHolder;

  //Sprite attributes.
  public GameObject spritePrefab;
  public SpritesAndColors spritesAndColors;

  //Unit attributes.
  public UnitInformationStruct[] units;
  [SerializeField] private GameObject UnitTimerDisplay;

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
    return gameGrid[gl.x, gl.y];
  }

  #region animation library

  private void animationLibrarySetup(){
    for (int i = 0; i < animationLibrarySetter.Length; i++){
      animationLibrary.Add(animationLibrarySetter[i].animationName, animationLibrarySetter[i].animation);
    }
  }

  /// <summary>
  /// Animations the library.
  /// </summary>
  /// <returns>an uninitalized prefab for the animation. Null if no animation with that name exists</returns>
  public GameObject getAnimation(string animationName){
    GameObject animation;
    bool getAnimationSuccess = animationLibrary.TryGetValue(animationName, out animation);
    if (getAnimationSuccess){
      return animation;
    } else{
      return null;
    }
  }

  #endregion

  #region start

  // Use this for initialization
  void Start(){
    animationLibrarySetup();
    /*
         * all grid spaces are represented by a game object
         * setup the game grid array */
    gameGrid = new GridBlock[gridSize, gridSize]; 
    GameObject tempObject;      
    for (int x = 0; x < gridSize; x++){ //
      for (int y = 0; y < gridSize; y++){
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
    for (int x = 0; x < gridSize; x++){
      for (int y = 0; y < gridSize; y++){
        if (y + 1 < gridSize){
          gameGrid[x, y].setAdj(Direction.UP, gameGrid[x, y + 1]);
        }
        if (y - 1 >= 0){
          gameGrid[x, y].setAdj(Direction.DOWN, gameGrid[x, y - 1]);
        }
        if (x - 1 >= 0){
          gameGrid[x, y].setAdj(Direction.LEFT, gameGrid[x - 1, y]);
        }
        if (x + 1 < gridSize){
          gameGrid[x, y].setAdj(Direction.RIGHT, gameGrid[x + 1, y]);
        }
      }
    }
    gameGrid[2, 2].setSpawn(); // 2,2 is alwayes spawn for Debug
    gameGrid[5, 2].setSpawn();

  }

  #endregion

  #region levelSaving

  /// <summary>
  /// saves the level with a text file representation
  /// </summary>
  public void saveLevel(){ //UNDONE almost done, some file path errors
    string levelFile = "";
    levelFile += gridSize + "\n";
    for (int x = 0; x < gridSize; x++){
      for (int y = 0; y < gridSize; y++){
        levelFile += gameGrid[x, y].printSpaceState();
      }
      levelFile += "\n";
    }

  }

  #endregion


  // Update is called once per frame
  void Update(){

  }
}

#region sprites and colors
/// <summary>
/// struct filled with all the sprites and colors
/// </summary>
[System.Serializable]
public struct SpritesAndColors{
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

#region animations
[System.Serializable]
public struct Animations{
  public string animationName;
  public GameObject animation;
}
#endregion