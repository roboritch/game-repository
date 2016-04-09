using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Used by the "CreatePlayGrid" script to help with level editing.
/// </summary>
public class ContextCanvas : MonoBehaviour{
	/// <summary>
	/// The button to remove a space.
	/// </summary>
	public Button removeSpace;
	/// <summary>
	/// The button to make a spawn point.
	/// </summary>
	public Button makeSpawnPoint;
	/// <summary>
	/// The context object.
	/// </summary>
	public GridBlock space;
	private int allianceNumberForSpawnSpot;
	private bool aiOnThisSpace = false;

	public void setAISpown(bool value){
		aiOnThisSpace = value;
		space.setAIOnThisSpawnBlock(aiOnThisSpace);
	}


	public void setAllianceNumberForSpawnSpot(int allianceNumber){
		allianceNumberForSpawnSpot = allianceNumber;
		if(space.isSpawnSpot()){
			space.setTeamSpawnAlliance(allianceNumber, aiOnThisSpace);
		}
	}


	// Use this for initialization.
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start(){
	}

	/// <summary>
	/// Allow programs to be spawned from this block.
	/// </summary>
	public void toggelSpawnPoint(){
		if(!space.isSpawnSpot())
			space.setSpawn(space.GridManager.team[allianceNumberForSpawnSpot], aiOnThisSpace);
		else
			space.removeSpawn();
	}

	/// <summary>
	/// Removes the space from the game grid.
	/// </summary>
	public void RemoveSpace(){
		space.toggleSpaceAvailable();
	}

	/// <summary>
	/// Destroy this context menu.
	/// </summary>
	public void ExitContextMenu(){ //TODO setup close button in prefab
		space.ExitContextMenu();
		Destroy(gameObject);
	}

	void OnDestroy(){
		space.GridManager.setContextMenuFalse();
	}
}

