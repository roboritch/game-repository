using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Program script also known as a unit.
/// Parent script for all programs.
/// </summary>
public class UnitScript : MonoBehaviour {
	/// <summary>A list of this unit's blocks.</summary>
	private LinkedList<GridBlock> blockList;
	/// <summary>Block head location after a move action is queued.</summary>
	public GridBlock virtualBlockHead;
	/// <summary>Maximum amount of unit blocks.</summary>
	private int maxProgramLength;
	/// <summary>The unit info.</summary>
	public UnitInformationStruct unitInfo;
	/// <summary>The grid the unit is on (the level).</summary>
	public CreatePlayGrid grid;
	// Use this to make private fields visible in the inspector.
	[SerializeField]
	#pragma warning disable
  private GameObject[] buttonPrefabs;
	/// <summary>A list of all the actions the user has selected for this unit.</summary>
	private LinkedList<ActionScript> actionList;
	/// <summary>The temp action.</summary>
	public ActionScript tempAction;

	#region programName

	private string programName;

	public string ProgramName {
		get {
			return programName;
		}
		set {
			programName = value;
		}
	}

	#endregion

	#region Unit Block Management

	public GridBlock getCurrentBlockHeadLocation() {
		return blockList.First.Value;
	}

	public GridBlock getVirtualBlockHeadLocation() {
		if(virtualBlockHead == null)
			virtualBlockHead = blockList.First.Value;
		return virtualBlockHead;
	}

	/// <summary>
	/// Gets the length of this unit.
	/// </summary>
	/// <returns>The length of this unit.</returns>
	public int getLength() {
		return blockList.Count;
	}

	/// <summary>
	/// Move the unit's head by adding a unit block to the given new location. Will remove the last block if the unit is already at its max length.
	/// </summary>
	/// <returns>Success of unit's move action.</returns>
	/// <param name="newLocation">The block to move unit head to.</param>
	public bool addBlock(GridBlock newLocation) {
		//check if grid space is already occupied
		if(newLocation.unitInstalled == null) {
			//check if unit is already at its max length
			if(getLength() > MaxProgramLength) {
				//remove a block from the end
				removeBlock();
			}
			//add a new unit block to given location
			blockList.AddFirst(newLocation);
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// Remove one block from this unit, destroying it if all blocks are removed.
	/// </summary>
	/// <returns>Whether the unit was destroyed.</returns>
	public bool removeBlock() {
		return removeBlock(1);
	}

	/// <summary>
	/// Removes a number of last blocks from this unit, destroying it if all blocks are removed.
	/// </summary>
	/// <returns>Whether the unit was destroyed.</returns>
	/// <param name="amount">The amount of blocks to remove.</param>
	public virtual bool removeBlock(int amount) {
		//check if unit will be destroyed by amount of blocks removed
		if(amount >= getLength()) {
			//destroy the unit
			destroyUnit();
			return true;
		} else {
			//remove the given amount of blocks
			for(int i = 0; i < amount; i++)
				blockList.RemoveLast();
		}
		return false;
	}

	/// <summary>
	/// Called when the grid block creates the unit.
	/// </summary>
	/// <param name="startLocation">Start location.</param>
	public virtual void spawnUnit(CreatePlayGrid gm, GridBlock startLocation) {
		grid = gm;
		blockList = new LinkedList<GridBlock>();
		maxProgramLength = unitInfo.maxLength;
		blockList.AddLast(startLocation);
		float spawnTime = spawnAnimation();
		Invoke("checkAllDisplay",spawnTime);
	}

	private float spawnAnimation(){
		GameObject animationObj = Instantiate(grid.animations.unitSpawn) as GameObject;
		animationObj.transform.SetParent(getCurrentBlockHeadLocation().transform,true);
		animationObj.transform.localPosition = new Vector3();
		SquareParticleFill anim = animationObj.GetComponent<SquareParticleFill>();
		anim.setParticalColor(getUnitColor());
		return anim.getTimeToCompleat();
	}

	private void checkAllDisplay() {
		foreach(GridBlock loc in  blockList) {
			loc.spriteDisplayScript.updateUnitSprite();
			loc.spriteDisplayScript.checkConection();
		}
	}

	public int MaxProgramLength {
		get {
			return maxProgramLength;
		}
	}

	/// <summary>
	/// Modifies the max length of the program.
	/// Does not shrink the current size of the unit.
	/// </summary>
	/// <param name="value">Value.</param>
	public void setMaxProgramLength(int value) {
		maxProgramLength = value;
	}

	#endregion

	#region Basic Unit Information

	/// <summary>
	/// Gets the color of the unit.
	/// Must be overiden by new unit with that unit's color.
	/// </summary>
	/// <returns>The unit color.</returns>
	public virtual Color getUnitColor() {
		return unitInfo.unitColor;
	}

	/// <summary>
	/// The head sprite.
	/// Must be set from child unit.
	/// </summary>
	public virtual Sprite getUnitHeadSprite() {
		return unitInfo.unitHeadSprite;
	}

	#endregion

	#region Display Possible Actions

	/// <summary>
	/// Used by the GUI to display this unit's possible actions.
	/// </summary>
	public GameObject[] getButtonPrefabs() {
		return buttonPrefabs;
	}


	/// <summary>
	/// Sets the grid connection.
	/// This must be set when the unit is created.
	/// </summary>
	/// <param name="playGrid">Play grid.</param>
	public void setGridConection(CreatePlayGrid playGrid) {
		grid = playGrid;
	}

	/// <summary>
	/// Displays the action as a button.
	/// Each unit will have its own button prefabs.
	/// </summary>
	/// <param name="actionDiscription">The action's description.</param>
	/// <param name="button">Button which will be displayed on the GUI</param>
	public void displayActionsAsButton(string actionDiscription, GameObject button) {
		
	}

	#endregion

	#region Action List

	/*each action will be a child of the ActionScript */
	public ActionScript getLastAction() {
		return actionList.Last.Value;
	}

	public void startActing() {
		
		invokeNextAction();
	}

	/// <summary>
	/// Invokes the next action.
	/// All actions will run until the actionList is empty.
	/// </summary>
	private void invokeNextAction() {
		ActionScript action = actionList.First.Value;
		action.act();
		actionList.RemoveFirst();
		if(actionList.First != null){ // only preform another action if there is one
			getReadyToPreformAnotherAction(action.getActionTime());
		}else{
			//TODO send info that this unit is done acting
			grid.gui.unitIsDoneActing(this);
		}
	}

	/// <summary>
	/// The next action will be preformed after the animation has 
	/// been completed on the last action.
	/// </summary>
	/// <param name="timeTillNextAction">Time till next action.</param>
	private void getReadyToPreformAnotherAction(float timeTillNextAction) {
		Invoke("invokeNextAction", timeTillNextAction);
	}

	/// <summary>
	/// removes the last action added to the actionList
	/// </summary>
	public void undoAction() {
		actionList.Last.Value.removeActionRepresentationDisplay();
		actionList.RemoveLast();
	}

	public void addActionToQueue(ActionScript action) {
		actionList.AddLast(action);
	}


	public void resetActionQueue(GUIScript gui) {
		foreach(ActionScript actions in actionList) {
			actions.removeActionRepresentationDisplay();
		}
	}


	#endregion



	// Use this for initialization
	/// <summary> Start this instance. </summary>
	void Start() {

	}

	// Update is called once per frame
	/// <summary> Update this instance. </summary>
	void Update() {

	}

	/// <summary> Destroys the unit. </summary>
	public void destroyUnit() {
		//TODO make sure there are no refrences to this unit before it is destroyed
		Destroy(gameObject);
	}
}
