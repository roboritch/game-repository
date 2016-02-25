using System;
using System.Collections;
using UnityEngine;

public class GridBlock : MonoBehaviour {
	#region adjacent blocks
	/// <summary>Upper adjacent block.</summary>
	public GridBlock up;
	/// <summary>Lower adjacent block.</summary>
	public GridBlock down;
	/// <summary>Left adjacent block.</summary>
	public GridBlock left;
	/// <summary>Right adjacent block.</summary>
	public GridBlock right;

	#endregion

	#region block static properties
	/// <summary>Spawn spot flag.</summary>
	public bool spawnSpot = false;
	/// <summary>Whether this grid block 'exists'.</summary>
	public bool online = true;

	#endregion


	#region sprites

	public GridBlockSpriteDisplay spriteDisplayScript;

	/// <summary>Displays the conections.</summary>
	/// <param name="unit">Unit.</param>
	void displayConections(UnitScript unit) {
		/*TODO call this every time a unit changes size or moves to 
		 * correctly display the conections in a single program */
		 
	}


	#endregion

	public UnitScript programInstalled;
	private GridBlock programHeadLocation;

	private Collider2D selectionBox;
	private CreatePlayGrid gridManager;

	public CreatePlayGrid GridManager {
		set {
			gridManager = value;
		}
	}


	#region mouse events

	/// <summary>Raises the mouse down event.</summary>
	void OnMouseDown() { // UNDONE need to add collider to gridblock prefab
		if(gridManager.editModeOn && !gridManager.contextMenuUp) {
			Debug.Log("mouse down on grid block");
			gridManager.contextMenuUp = true;
			displayEditRightClickMenu();
		}
	}

	/// <summary>Raises the mouse over event.</summary>
	void OnMouseOver() {

	}

	#endregion

	#region edit mode

	/// <summary>Display the right click menu.</summary>
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
	/// Removes the current context menu for this block and allows new context menus
	/// to be created for other blocks.
	/// </summary>
	internal void ExitContextMenu() {
		gridManager.contextMenuUp = false;
	}

	/// <summary>Prints the state of the space for level saving.</summary>
	public string printSpaceState() {
		if(online == false)
			return " ";
		if(spawnSpot) 
			return "o";
		return "x";
		//TODO make level saver
	}

	/// <summary>Toggles the online state.</summary>
	public void toggleSpaceOnline() { 
		if(online == false) {
			//TODO tell spaces around this one that it is active
			setSpriteDefault();
		} else {
			//TODO tell spaces around this one that it is not active
			transform.GetComponent<SpriteControler>().removeSprite();
		}
		//toggle state
		online = !online;
	}


	/// <summary>Sets the spawn flag and updates sprite.</summary>
	public boolean setSpawn() {
		//fail to add spawn flag if not active
		if(!online)
			return false;
		spawnSpot = true;
		setSpriteSpawn();
		return true;
	}

	/// <summary>Removes the spawn flag and updates sprite.</summary>
	/// <returns>The spawn.</returns>
	public boolean removeSpawn() {
		//fail to remove spawn flag if not active
		if(!online)
			return false;
		spawnSpot = false;
		setSpriteDefault();
		return true;
	}

	/// <summary>Sets the sprite to default.</summary>
	private void setSpriteDefault(){
		transform.GetComponent<SpriteControler>().setSprite(gridManager.sprite_defaultSpace, gridManager.color_defaultSpaceColor);
	}

	/// <summary>Sets the sprite to spawn.</summary>
	private void setSpriteSpawn(){
		transform.GetComponent<SpriteControler>().setSprite(gridManager.sprite_spawnSpace, gridManager.color_spawnSpaceColor);
	}


	#endregion

	// Use this for initialization
	/// <summary>Start this instance.</summary>
	void Start() {
		spriteDisplayScript = GetComponent<GridBlockSpriteDisplay>();
	}

	// Update is called once per frame
	/// <summary>Update this instance.</summary>
	void Update() {

	}
}
