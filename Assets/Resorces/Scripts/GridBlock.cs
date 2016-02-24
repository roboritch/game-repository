using System;
using System.Collections;
using UnityEngine;

public class GridBlock : MonoBehaviour {
	#region adjasent blocks

	public GridBlock up;
	public GridBlock down;
	public GridBlock left;
	public GridBlock right;

	#endregion

	#region block properties

	[SerializeField]
	private bool spawnSpot = false;
	[SerializeField]
	private bool online = true;

	#endregion

	#region sprites

	public GridBlockSpriteDisplay spriteDisplayScript;

	/// <summary>
	/// Displaies the conections.
	/// </summary>
	/// <param name="unit">Unit.</param>
	void displayConections(UnitScript unit) {
		/*TODO call this every time a unit changes size or moves to 
		 * correctily display the conections in a single program */
		 
	}


	#endregion

	#region simple block vars

	public UnitScript programInstalled;
	private CreatePlayGrid gridManager;

	private Collider2D selectionBox;


	public CreatePlayGrid GridManager {
		set {
			gridManager = value;
		}
	}

	/// <summary>
	/// Location of this grid block on the play grid
	/// </summary>
	public GridLocation gridlocation;

	#endregion

	#region mouseDown and mouseOver

	void OnMouseDown() { 
		if(gridManager.editModeOn && !gridManager.contextMenuUp) {
			Debug.Log("mouse down on grid block");
			gridManager.contextMenuUp = true;
			displayEditRightClickMenu();
		}

		//Set the buttons up in the gui for the installed unit when this grid block is selected
		//all prev buttons are removed when this method is called
		if(programInstalled != null) {
			gridManager.gui.setButtons(programInstalled.getButtonPrefabs());
		}

		if(spawnSpot && programInstalled != null) {
			//TODO bring up unit selection for this block
		}
	}

	public void createUnit(UnitScript unit) {
		programInstalled = unit;
		programInstalled.spawnUnit(gridlocation);
	}

	void OnMouseOver() {

	}

	#endregion

	#region edit mode

	public void displayEditRightClickMenu() { //UNDONE display menu on right click
		GameObject contextMenu = Instantiate(gridManager.gridEditMenu) as GameObject;
		contextMenu.GetComponent<ContextCanvas>().space = this;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		Debug.DrawRay(ray.origin, ray.direction, Color.red, 50f);
		if(Physics.Raycast(ray, out hit)) {
			contextMenu.transform.position = hit.point + new Vector3(0, 3f, 0);

			//Debug.Log(hit);
		}
	}

	/// <summary>
	/// removes the current context menu for this block and alows new context menus
	/// to be created for other blocks
	/// </summary>
	internal void ExitContextMenu() {
		gridManager.contextMenuUp = false;
	}

	/// <summary>
	/// Prints the state of the space for level saving.
	/// </summary>
	public string printSpaceState() {
		if(online == false) {
			return " ";
		}
		if(spawnSpot) {
			return "o";
		}

		return "x";
		//TODO make level saver
	}

	public void toggleSpaceOnline() { 
		if(online == false) {
			up.down = this;
			down.up = this;
			left.right = this;
			right.left = this;
			transform.GetComponent<SpriteControler>().setSprite(gridManager.spritesAndColors.sprite_defaultSpace, gridManager.spritesAndColors.color_defaultSpaceColor);
			online = !online;
		} else {
			up.down = null;
			down.up = null;
			left.right = null;
			right.left = null;
			transform.GetComponent<SpriteControler>().removeSprite();
			online = !online;
		}
	}


	/// <summary>
	/// Sets gridblock as a spawn.
	/// </summary>
	public void setSpawn() {
		if(!online) {
			return;
		}
		spawnSpot = true;
		transform.GetComponent<SpriteControler>().setSprite(gridManager.spritesAndColors.sprite_spawnSpace, gridManager.spritesAndColors.color_spawnSpaceColor);
	}

	public void removeSpawn() {
		if(!online) {
			return;
		}
		spawnSpot = false;
		transform.GetComponent<SpriteControler>().setSprite(gridManager.spritesAndColors.sprite_defaultSpace, gridManager.spritesAndColors.color_defaultSpaceColor);
	}


	#endregion

	#region create unit

	public void spawnUnit(UnitScript unit) {
		unit.transform.position = new Vector3();
		unit.transform.SetParent(gridManager.unitObjectHolder);
		programInstalled = unit;
		unit.spawnUnit(gridlocation);
	}

	#endregion

	// Use this for initialization
	void Start() {
		spriteDisplayScript = GetComponent<GridBlockSpriteDisplay>();
	}

	// Update is called once per frame
	void Update() {

	}
}

#region gridLocation
/// <summary>
/// Grid location.
/// implements ==, != and = operations
/// </summary>
#pragma warning disable
public struct GridLocation {
	public int x;
	public int y;

	/// <summary>
	/// Copy this instance.
	/// Not the same as = (refrence copy).
	/// </summary>
	public GridLocation copy() {
		GridLocation a;
		a.x = x;
		a.y = y;
		return a;
	}

	public static bool operator ==(GridLocation a, GridLocation b) {
		return a.x == b.x && a.y == b.y;
	}

	public static bool operator !=(GridLocation a, GridLocation b) {
		return !(a == b);
	}

}
#endregion