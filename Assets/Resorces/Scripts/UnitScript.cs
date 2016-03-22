using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Program script also known as a unit.
/// Parent script for all programs.
/// </summary>
public class UnitScript : MonoBehaviour {
	#region basic vars
	/// <summary>A list of this unit's blocks.</summary>
	private LinkedList<GridBlock> blockList;
	/// <summary>Block head location after a move action is queued.</summary>
	public GridBlock virtualBlockHead;
	/// <summary>Maximum amount of unit blocks.</summary>
	private int maxProgramLength;
	/// <summary>The unit info.</summary>
	[SerializeField] private UnitInformationStruct unitInfo;
	/// <summary>The grid the unit is on (the level).</summary>
	public CreatePlayGrid grid;
	// Use this to make private fields visible in the inspector.

	#pragma warning disable
	[SerializeField] private GameObject[] buttonPrefabs;
	/// <summary>A list of all the actions the user has selected for this unit.</summary>
	private LinkedList<ActionScript> actionList;
	/// <summary>The temp action.</summary>
	#endregion

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

	#region Unit Size Management

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
	public bool addBlock(GridBlock newLocation,bool animate) {
		bool movedSuccess;
		//check if grid space is already occupied
		if(newLocation.unitInstalled == null) {
			//check if unit is already at its max length
			if(getLength() >= MaxProgramLength) {
				//remove a block from the end
				removeBlock();
			}
			//add a new unit block to given location
			blockList.AddFirst(newLocation);
			newLocation.unitInstalled = this;
			movedSuccess = true;
		} else {
			movedSuccess = false;
		}
		if(animate){
			//TODO unitMoving animation
			float animationTime = 0f;
			Invoke("checkAllDisplay",animationTime);
		}else{
			checkAllDisplay();
		}

		return movedSuccess;
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
			for(int i = 0; i < amount; i++){
				blockList.Last.Value.unitInstalled = null;
				blockList.Last.Value.spriteDisplayScript.updateUnitSprite();
				blockList.RemoveLast();
			}
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
		//set base unit stats so they can be adjusted at runtime
		maxProgramLength = unitInfo.maxLength;
		maximumMovment = unitInfo.maxMove;
		movmentActionsRemaning = maximumMovment;

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
		return anim.getAnimationTime();
	}

	public void checkAllDisplay() {
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
	private bool isActing = false;
	public bool IsActing {
		get {
			return isActing;
		}
	}

	public ActionScript tempAction;
	public void removeUserSelectionDisplay(){
		if(tempAction != null)
			tempAction.removeUserSelectionDisplay();
	}

	/*each action will be a child of the ActionScript */
	public ActionScript getLastAction() {
		return actionList.Last.Value;
	}

	public void startActing() {
		if(!readyToAct || actionList.Count == 0){
			Debug.LogWarning("Unit is not ready to act!");
		}else{
			isActing = true;
			stopTimerTick();
			resetTimer();
			readyToAct = false;
			invokeNextAction(); // this must be last
		}
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
			isActing = false;
			startTimerTick();
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

	public void resetActionQueue() {
		foreach(ActionScript actions in actionList) {
			actions.removeActionRepresentationDisplay();
		}
		actionList.Clear();
		virtualBlockHead = null;
	}

	public int getNumberOfActionsInQueue(){
		return actionList.Count;
	}

	#endregion

	#region movment stats
	private int maximumMovment;
	private int movmentActionsRemaning;
	public int getMaximumMovment(){
		return maximumMovment;
	}

	public void moveActionAdded(){
		movmentActionsRemaning--;
	}

	public void moveActionRemoved(){
		movmentActionsRemaning++;
	}

	public int movmentRemaning(){
		return movmentActionsRemaning;
	}

	#endregion

	#region attack stats
	/// <summary> The attack locations, 0,0 is the unit 1,0 is the block to the right ext. </summary>
	[SerializeField] private GridLocation[] attackLocations;
	public GridBlock[] getAttackLocations(){
		GridBlock[] x = new GridBlock[attackLocations.Length];
		foreach(var item in attackLocations) {
			x = grid.gridLocationToGameGrid(item);
		}
	}

	/// <summary>
	/// The current attacks remaning for the current action queue.
	/// </summary>
	private int currentAttacksRemaning;
	public int attacksRemaning(){
		return currentAttacksRemaning;
	}

	public void useAttackAction(){
		currentAttacksRemaning--;
	}

	public void useAttackAction(int amount){
		currentAttacksRemaning -= amount;
	}

	public void addAttackAction(){
		currentAttacksRemaning++;
	}

	public void addAttackAction(int amount){
		currentAttacksRemaning += amount;
	}

	#endregion

	#region unit timeing

	private UnitTimer UT;
	public UnitTimer unitTimer {
		get {
			return UT;
		}
		set{
			UT = value;
		}
	}

	[SerializeField] private bool readyToAct = false;
	private void timerTick(){
		if(UT.time < UT.maxTime){
			UT.time += UT.ticAmount;
		}else{
			readyToAct = true;
			stopTimerTick();
		}
	}

	private void resetTimer(){
		UT.time = 0f;
	}

	private void startTimerTick(){
		InvokeRepeating("timerTick",0f,0.01f);
	}

	private void stopTimerTick(){
		CancelInvoke("timerTick");
	}

	private void timerStartup(){
		UT = unitInfo.unitTimer;
	}

	#endregion

	#region onSelection
	public int getUnitMaxSize(){
		return maxProgramLength;
	}

	public int getUnitMaxMovment(){
		return unitInfo.maxMove;
	}

	#endregion

	void Start() {
		actionList = new LinkedList<ActionScript>();
		timerStartup();
		startTimerTick();
	}

	// Update is called once per frame
	/// <summary> Update this instance. </summary>
	void Update() {

	}

	#region unit destruction
	/// <summary> Destroys the unit. </summary>
	public void destroyUnit() {
		//TODO make sure there are no refrences to this unit before it is destroyed
		getCurrentBlockHeadLocation().removeUnit();
		if(grid.gui.getCurUnit() == this){
			grid.gui.setSelectedUnit(null);
		}
		Destroy(gameObject);
	}
	#endregion 
}

[System.Serializable]
public struct UnitTimer{
	public float time;
	public float maxTime;
	public float ticAmount;
}