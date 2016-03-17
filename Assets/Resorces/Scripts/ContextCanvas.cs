using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// used by the "CreatePlayGrid" script to help with level editing.
/// </summary>
public class ContextCanvas : MonoBehaviour {
	//these must be set in the prefab
	public Button removeSpace;
	public Button makeSpawnPoint;
	//the context object
	public GridBlock space;

	// Use this for initialization
	void Start() {


	}

	/// <summary>
	/// alow programes to be spawned from this block
	/// </summary>
	public void MakeSpawnPoint() {
		space.setSpawn();
	}

	/// <summary>
	/// removes the space from the game grid
	/// </summary>
	public void RemoveSpace() {
		space.toggleSpaceAvailable();
	}

	/// <summary>
	/// destroy this context menu
	/// </summary>
	public void ExitContextMenu() { //TODO setup close button in prefab
		space.ExitContextMenu();
		Destroy(gameObject);
	}

}

