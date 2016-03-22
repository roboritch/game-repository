using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Grid block belonging to the play grid.
/// Contains pointers to the four adjcent blocks.
/// Contains a sprite.
/// Can be online or offline (occupiable or not).
/// Can be a spawn spot.
/// </summary>
public class GridBlock : MonoBehaviour,IPointerDownHandler {


	#region adjacent blocks

  /// <summary>Upper adjacent block.</summary>
  private GridBlock up;
  /// <summary>Lower adjacent block.</summary>
  private GridBlock down;
  /// <summary>Left adjacent block.</summary>
  private GridBlock left;
  /// <summary>Right adjacent block.</summary>
  private GridBlock right;

  public Collider gridBlockCollider;

	public GridBlock getAdj(int dir){
		if(dir == (int)Direction.UP){
			return up;
		}else if(dir == (int)Direction.LEFT){
			return left;
		}else if(dir == (int)Direction.DOWN){
			return down;
		}else if(dir == (int)Direction.RIGHT){
			return right;
		}
		return null;
  }

  public GridBlock getAdj(Direction dir){
    switch (dir){
      case Direction.UP:
        return up;
      case Direction.DOWN:
        return down;
      case Direction.LEFT:
        return left;
      default:
        return right;
    }
  }

  public void setAdj(Direction dir, GridBlock block){
    switch (dir){
      case Direction.UP:
         up = block;
        break;
      case Direction.DOWN:
        down = block;
        break;
      case Direction.LEFT:
        left = block;
        break;
      default:
        right = block;
        break;
    }
  }

  #endregion

	#region block properties

  /// <summary>Whether this block is a spawn spot.</summary>
  [SerializeField] private bool spawnSpot = false;
  /// <summary>Whether this block is an occupiable space.</summary>
  [SerializeField] private bool available = true;

  #endregion

	#region sprites

  /// <summary>Sprite display for this block.</summary>
  public GridBlockSpriteDisplay spriteDisplayScript;

  /// <summary>Display the conections.</summary>
  /// <param name="unit">Unit.</param>
  void displayConections(UnitScript unit){
    /*TODO call this every time a unit changes size or moves to 
		 * correctily display the conections in a single program */
		 
  }

  #endregion

	#region simple block vars

	/// <summary>The unit currently occupying this space.</summary>
	public UnitScript unitInstalled;
	/// <summary> The action waiting for user input on this block. </summary>
	public ActionScript actionWaitingForUserInput;
	/// <summary>The game's grid manager.</summary>
	private CreatePlayGrid gridManager;
	/// <summary>Collision box of this space.</summary>
	private Collider2D selectionBox;

	/// <summary>Sets the grid manager.</summary>
	/// <value>The grid manager.</value>
	public CreatePlayGrid GridManager {
		set {
			gridManager = value;
		}
	}

  /// <summary>Location of this grid block on the play grid.</summary>
  public GridLocation gridlocation;

  #endregion

	#region mouse events

	/// <summary>Raises the mouse down event.</summary>
	void OnMouseDown(){ //TODO there could be a better way to handle these events
		
  }

  /// <summary>Raises the mouse over event.</summary>
  void OnMouseOver(){

  }

  #endregion

	#region IPointerDownHandler implementation (mouse events)

	// the gui will  now block clicks
	public void OnPointerDown (PointerEventData eventData)
	{
		if (gridManager.editModeOn && !gridManager.contextMenuUp){
			Debug.Log("mouse down on grid block");
			gridManager.contextMenuUp = true;
			displayEditRightClickMenu();
		}

		//set the buttons up in the GUI for the installed unit when this grid block is selected
		//all prev buttons are removed when this method is called
		if (unitInstalled != null && Input.GetMouseButton(0)){ // only on left click
			gridManager.gui.setSelectedUnit(unitInstalled);
		}

		//if the mouse button is pressed, and this block is a spawn spot and is not currently occupied by a unit
		if (spawnSpot && unitInstalled == null && actionWaitingForUserInput == null && Input.GetMouseButton(0)){
			gridManager.gui.unitSelectionScript.enableOnGridBlock(this);
		}

		if (actionWaitingForUserInput is MoveScript){
			actionWaitingForUserInput.userSelectedAction(this);
		}else if(actionWaitingForUserInput is AttackScript){
			actionWaitingForUserInput.userSelectedAction(this);
		}else if (unitInstalled == null && Input.GetMouseButton(0)){
			if(gridManager.gui.getCurUnit() != null)
				gridManager.gui.getCurUnit().removeUserSelectionDisplay();
			gridManager.gui.setSelectedUnit(null);	
		}

		//		if (Input.GetMouseButton (0) && gridManager.gui.getCurUnit() != null) {
		//			gridManager.gui.setUnitAsSelected (null);
		//		}
	}

	#endregion

	#region attack handling
	private int attackActionID = -1;
	public void attackActionWantsToAttackHere(AttackScript attack){
		if(unitInstalled != null){
			attackActionID = spriteDisplayScript.displayAction(gridManager.spritesAndColors.sprite_attack);
			actionWaitingForUserInput = attack;
		}
	}

	public void removeAttackDisplayForThis(){
		spriteDisplayScript.removeAction(attackActionID);
		attackActionID = -1;
		actionWaitingForUserInput = null;
	}


	#endregion

	#region edit mode

  /// <summary>Displays the edit right click menu.</summary>
  public void displayEditRightClickMenu(){ //UNDONE display menu on right click
    GameObject contextMenu = Instantiate(gridManager.gridEditMenu) as GameObject;
    contextMenu.GetComponent<ContextCanvas>().space = this;

    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    Debug.DrawRay(ray.origin, ray.direction, Color.red, 50f);
    if (Physics.Raycast(ray, out hit)){
      contextMenu.transform.position = hit.point + new Vector3(0, 3f, 0);
      //Debug.Log(hit);
    }
  }

  /// <summary>
  /// Removes the current context menu for this block and alows new context menus
  /// to be created for other blocks.
  /// </summary>
  internal void ExitContextMenu(){
    gridManager.contextMenuUp = false;
  }

  /// <summary>
  /// <para>Prints the state of the space for level saving.</para>
  /// <para>" " = offline</para>
  /// <para>"o" = spawn spot</para>
  /// <para>"x" = other</para>
  /// </summary>
  public string printSpaceState(){
    if (available == false)
      return " ";
    if (spawnSpot)
      return "o";
    return "x";
    //TODO make level saver
  }

  /// <summary>Toggle the online state of this block.</summary>
  public void toggleSpaceAvailable(){ 
    if (available == false){
      up.down = this;
      down.up = this;
      left.right = this;
      right.left = this;
      setSpriteDefault();
    } else{
      up.down = null;
      down.up = null;
      left.right = null;
      right.left = null;
      setSpriteNone();
    }
    available = !available;
  }


  /// <summary>Sets gridblock as a spawn spot.</summary>
  public void setSpawn(){
    //fail to set spawn if block is offline
    if (!available)
      return;
    spawnSpot = true;
    //set spawn sprite
    setSpriteSpawn();
  }

  /// <summary>Sets gridblock from a spawn spot to a default spot.</summary>
  public void removeSpawn(){
    //fail to remove spawn if block is offline
    if (!available)
      return;
    spawnSpot = false;
    //set default sprite
    setSpriteDefault();
  }


  #endregion

	#region unit control

 	/// <summary>Spawns a given unit.</summary>
	/// <param name="unit">Unit.</param>
	public void spawnUnit(UnitScript unit){
		unit.transform.position = new Vector3();
		unit.transform.SetParent(gridManager.unitObjectHolder);
		unitInstalled = unit;
		unit.spawnUnit(gridManager, this);
	}

	public void removeUnit(){
		unitInstalled = null;
	}

  #endregion

	// Use this for initialization.
	/// <summary>Start this instance.</summary>
	void Start(){
		gridBlockCollider = GetComponent<Collider>();
		spriteDisplayScript = GetComponent<GridBlockSpriteDisplay>();
	}

	// Update is called once per frame.
	/// <summary>Update this instance.</summary>
	void Update(){
		
	}

	#region sprite controles for this block
  /// <summary>Sets the sprite to default.</summary>
  private void setSpriteDefault(){
    transform.GetComponent<SpriteControler>().setSprite(gridManager.spritesAndColors.sprite_defaultSpace, gridManager.spritesAndColors.color_defaultSpaceColor);
  }

  /// <summary>Sets the sprite to spawn.</summary>
  private void setSpriteSpawn(){
    transform.GetComponent<SpriteControler>().setSprite(gridManager.spritesAndColors.sprite_spawnSpace, gridManager.spritesAndColors.color_spawnSpaceColor);
  }

  /// <summary>Removes the sprite.</summary>
  private void setSpriteNone(){
    transform.GetComponent<SpriteControler>().removeSprite();
  }
	#endregion
}

#region gridLocation
/// <summary>
/// Grid location.
/// implements ==, != and = operations
/// </summary>
#pragma warning disable
public struct GridLocation{
  /// <summary>X coordinate of grid location.</summary>
  public int x;
  /// <summary>Y coordinate of grid location.</summary>
  public int y;

	public GridLocation(int xl,int yl){
		x = xl;
		y = yl;
	}

	public static GridLocation operator +(GridLocation a, GridLocation b){
		return new GridLocation(a.x + b.x, a.y + b.y);
	}

	public static GridLocation operator -(GridLocation a, GridLocation b){
		return new GridLocation(a.x - b.x, a.y - b.y);
	}





  public static bool operator ==(GridLocation a, GridLocation b){
    return a.x == b.x && a.y == b.y;
  }

  public static bool operator !=(GridLocation a, GridLocation b){
    return !(a == b);
  }

}
#endregion
public enum Direction : int{
	UP = 0,
	DOWN = 2,
	LEFT = 3,
	RIGHT = 1
}