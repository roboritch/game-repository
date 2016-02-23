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

	#region block static properties

	public bool full = false;
	public bool programHead = false;
	public bool spawnSpot = false;
	public bool online = true;

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

	public UnitScript programInstalled;

	private Collider2D selectionBox;
	private CreatePlayGrid gridManager;

	public CreatePlayGrid GridManager {
		set {
			gridManager = value;
		}
	}


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
			gridManager.gui.setButtons(programInstalled.buttonPrefabs);
		}

		if(spawnSpot && programInstalled != null) {
			//gridManager.gui.
		}
	}

	public void createUnit(UnitScript unit) {
		programInstalled = unit;
		gridManager.gui.setButtons(unit.buttonPrefabs);
			
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
		//TODO THIS!	
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
