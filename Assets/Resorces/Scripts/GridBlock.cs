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

	#region mesh types

	public Mesh basicMesh, spawnMesh, emptyMesh;

	#endregion

	public ProgramScript programInstalled;
	private GridBlock programHeadLocation;

	private Collider2D selectionBox;
	private CreatePlayGrid gridManager;

	public CreatePlayGrid GridManager {
		set {
			gridManager = value;
		}
	}

	#region mouseDown and mouseOver

	void OnMouseDown() { // UNDONE need to add colider to gridblock prefab
		if(gridManager.editModeOn && !gridManager.contextMenuUp) {
			Debug.Log("mouse down on grid block");
			gridManager.contextMenuUp = true;
			displayEditRightClickMenu();
		}
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
		if(!online){
			//TODO tell spaces around this one that it is active
			transform.GetComponent<MeshFilter>().mesh = basicMesh;
			online = !online;
			return;
		}
		//TODO tell spaces around this one that it is not active
		transform.GetComponent<MeshFilter>().mesh = emptyMesh;
		online = !online;
	}


	/// <summary>
	/// Sets gridblock as a spawn.
	/// </summary>
	public void setSpawn() {
		spawnSpot = true;
		transform.GetComponent<MeshFilter>().mesh = spawnMesh;
	}

	public void removeSpawn() {
		spawnSpot = false;
		transform.GetComponent<MeshFilter>().mesh = basicMesh;
	}


	#endregion

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}
}
