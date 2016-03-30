using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Program script also known as a unit.
/// Parent script for all programs.
/// </summary>
public class UnitScript : MonoBehaviour{
	#region basic vars
	/// <summary>A list of this unit's blocks.</summary>
	private LinkedList<GridBlock> blockList;
	/// <summary>Block head location after a move action is queued.</summary>
	public GridBlock virtualBlockHead;
	/// <summary>Maximum amount of unit blocks.</summary>
	private int maxProgramLength;
	#pragma warning disable
	/// <summary>The unit info.</summary>
	[SerializeField] private UnitInformationStruct unitInfo;
	/// <summary>The grid the unit is on (the level).</summary>
	public CreatePlayGrid grid;
	// Use this to make private fields visible in the inspector.

	#pragma warning disable
	[SerializeField] private GameObject[] buttonPrefabs;
	/// <summary>A list of all the actions the user has selected for this unit.</summary>
	private LinkedList<ActionScript> actionList;

	public ControlType controlType;

	private int team;
	private int enemyCount;
	#endregion

	#region programName

	private string programName;

	public string ProgramName{
		get{
			return programName;
		}
		set{
			programName = value;
		}
	}

	#endregion

	#region Unit Size Management

	public LinkedList<GridBlock> getBlockList(){
		return  blockList;
	}

	public GridBlock getCurrentBlockHeadLocation(){
		if(blockList.First == null){
			return null;
		}
		return blockList.First.Value;
	
	}

	public GridBlock getVirtualBlockHeadLocation(){
		if(virtualBlockHead == null)
			virtualBlockHead = blockList.First.Value;
		return virtualBlockHead;
	}

	/// <summary>
	/// Gets the length of this unit.
	/// </summary>
	/// <returns>The length of this unit.</returns>
	public int getLength(){
		return blockList.Count;
	}

	/// <summary>
	/// Gets the percentage of this unit's health.
	/// </summary>
	/// <returns>The health percentage of this unit.</returns>
	public double getHealthPercentage(){
		return (double)blockList.Count/maxProgramLength;
	}

	/// <summary>
	/// Move the unit's head by adding a unit block to the given new location. Will remove the last block if the unit is already at its max length.
	/// </summary>
	/// <returns>Success of unit's move action.</returns>
	/// <param name="newLocation">The block to move unit head to.</param>
	public bool addBlock(GridBlock newLocation, bool animate){
		bool movedSuccess;
		//check if grid space is already occupied
		if(newLocation.unitInstalled == null){
			//check if unit is already at its max length
			if(getLength() >= MaxProgramLength){
				//remove a block from the end
				removeBlock();
			}
			//add a new unit block to given location
			blockList.AddFirst(newLocation);
			newLocation.unitInstalled = this;
			movedSuccess = true;
		} else{
			movedSuccess = false;
		}
		if(animate){
			//TODO unitMoving animation
			float animationTime = 0f;
			Invoke("checkAllDisplay", animationTime);
		} else{
			checkAllDisplay();
		}

		return movedSuccess;
	}

	#region block removal
	/// <summary>
	/// Remove one block from this unit, destroying it if all blocks are removed.
	/// </summary>
	/// <returns>Whether the unit was destroyed.</returns>
	public bool removeBlock(){
		return removeBlock(1);
	}

	/// <summary>
	/// Removes a number of last blocks from this unit, destroying it if all blocks are removed.
	/// </summary>
	/// <returns>Whether the unit was destroyed.</returns>
	/// <param name="amount">The amount of blocks to remove.</param>
	public virtual bool removeBlock(int amount){
		//remove the given amount of blocks
		for(int i = 0; i < amount; i++){
			if(blockList.Last == null){
				enemyCount--;
				destroyUnit();
				checkWin ();
				return true;
			}
			GridBlock tempBlock = blockList.Last.Value;
			tempBlock.unitInstalled = null;
			checkAllDisplay();
			blockList.RemoveLast();
		}
		return false;
	}
	private void checkWin(){
		if (enemyCount == 0) {
			print("team "+team+" wins");
		}
		}
	///<summary> used by animations that want to show blocks being removed one at a time till the end of the animation</summary>
	public void queueBlockRemoval(int numberOfBlocksToRemove, float timeInterval_s, float delay){
		float removalSection = timeInterval_s / (float)numberOfBlocksToRemove;
		for(int i = 1; i < numberOfBlocksToRemove + 1; i++){
			Invoke("removeBlock", (float)(i) * removalSection + delay);
		}
	}
	#endregion


	/// <summary>
	/// Called when the grid block creates the unit.
	/// </summary>
	/// <param name="startLocation">Start location.</param>
	public virtual void spawnUnit(CreatePlayGrid gm, GridBlock startLocation){
		grid = gm;
		blockList = new LinkedList<GridBlock>();
		//set base unit stats so they can be adjusted at runtime
		maxProgramLength = unitInfo.maxLength;
		maximumMovment = unitInfo.maxMove;
		movmentActionsRemaning = maximumMovment;
		currentMaxPosibleAttackActions = unitInfo.maxAttackActions;
		currentAttacksRemaning = currentMaxPosibleAttackActions;
		currentAttackPower = unitInfo.attackPow;
		team = unitInfo.team;
		//setUnitColor (team);
		//transform.GetComponent<SpriteControler>().setColor (getUnitColor ());


		blockList.AddLast(startLocation);
		float spawnTime = spawnAnimation();
		Invoke("checkAllDisplay", spawnTime);
	}
		
	//each unit spawn must have it's colour set by this script
	private float spawnAnimation(){
		GameObject animationObj = Instantiate(grid.getAnimation("unit spawn")) as GameObject; // be carfull changing Names!
		animationObj.transform.SetParent(getCurrentBlockHeadLocation().transform, true);
		animationObj.transform.localPosition = new Vector3();
		SquareParticleFill anim = animationObj.GetComponent<SquareParticleFill>();
		anim.setParticalColor(getUnitColor());
		return anim.getAnimationTime();
	}

	public void checkAllDisplay(){
		foreach( GridBlock loc in  blockList ){
			loc.spriteDisplayScript.updateUnitSprite();
			loc.spriteDisplayScript.checkConection();
		}
	}

	public int MaxProgramLength{
		get{
			return maxProgramLength;
		}
	}

	/// <summary>
	/// Modifies the max length of the program.
	/// Does not shrink the current size of the unit.
	/// </summary>
	/// <param name="value">Value.</param>
	public void setMaxProgramLength(int value){
		maxProgramLength = value;
	}

	#endregion

	#region Basic Unit Information

	/// <summary>
	/// Gets the color of the unit.
	/// Must be overiden by new unit with that unit's color.
	/// </summary>
	/// <returns>The unit color.</returns>
	public virtual Color getUnitColor(){
		return unitInfo.unitColor;
	}
	private void setUnitColor(int i){
		if(i==1){
			unitInfo.unitColor=Color.blue;
		}
		else{
			unitInfo.unitColor=Color.red;
		}
	}

	/// <summary>
	/// The head sprite.
	/// Must be set from child unit.
	/// </summary>
	public virtual Sprite getUnitHeadSprite(){
		return unitInfo.unitHeadSprite;
	}

	#endregion

	#region Display Possible Actions

	/// <summary>
	/// Used by the GUI to display this unit's possible actions.
	/// </summary>
	public GameObject[] getButtonPrefabs(){
		return buttonPrefabs;
	}

	/// <summary>
	/// Sets the grid connection.
	/// This must be set when the unit is created.
	/// </summary>
	/// <param name="playGrid">Play grid.</param>
	public void setGridConection(CreatePlayGrid playGrid){
		grid = playGrid;
	}

	/// <summary>
	/// Displays the action as a button.
	/// Each unit will have its own button prefabs.
	/// </summary>
	/// <param name="actionDiscription">The action's description.</param>
	/// <param name="button">Button which will be displayed on the GUI</param>
	public void displayActionsAsButton(string actionDiscription, GameObject button){
		
	}

	#endregion

	#region Action List
	private bool isActing = false;

	public bool IsActing{
		get{
			return isActing;
		}
	}

	private ActionScript tempAction;

	public void setTempAction(ActionScript action, bool displayUserSelection){
		if(tempAction != null){
			tempAction.removeUserSelectionDisplay();
		}	
		tempAction = action;
		if(displayUserSelection){
			tempAction.displayUserSelection();
		}

	}

	public void removeUserSelectionDisplay(){
		if(tempAction != null)
			tempAction.removeUserSelectionDisplay();
	}

	/*each action will be a child of the ActionScript */
	public ActionScript getLastAction(){
		return actionList.Last.Value;
	}

	public virtual void startActing(){
		if(!readyToAct || actionList.Count == 0){
			Debug.LogWarning("Unit is not ready to act!");
		} else{
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
	private void invokeNextAction(){
		ActionScript action = actionList.First.Value;
		action.act();
		actionList.RemoveFirst();
		if(actionList.First != null){ // only preform another action if there is one
			getReadyToPreformAnotherAction(action.getActionTime()); //what for the current actions animation to finish
		} else{
			//TODO send info that this unit is done acting
			isActing = false;
			startTimerTick();
			resetActionQueue(false); 
			grid.gui.unitIsDoneActing(this);
		}
	}

	/// <summary>
	/// The next action will be preformed after the animation has 
	/// been completed on the last action.
	/// </summary>
	/// <param name="timeTillNextAction">Time till next action.</param>
	private void getReadyToPreformAnotherAction(float timeTillNextAction){
		Invoke("invokeNextAction", timeTillNextAction);
	}

	/// <summary>
	/// removes the last action added to the actionList
	/// </summary>
	public void undoAction(){
		actionList.Last.Value.removeActionRepresentationDisplay();
		actionList.RemoveLast();
	}

	public void addActionToQueue(ActionScript action){
		actionList.AddLast(action);
	}

	/// <summary>
	/// Resets the action queue and the maximum alowed move and attack actions.
	/// </summary>
	public void resetActionQueue(bool overRideIsActing){
		if(!overRideIsActing && isActing)
			return;
		foreach( ActionScript actions in actionList ){
			actions.removeActionRepresentationDisplay();
		}
		actionList.Clear();
		virtualBlockHead = null;
		resetAttackActionsToCurrentMax();
		resetMovmentActionsToCurrentMax();
	}

	public int getNumberOfActionsInQueue(){
		return actionList.Count;
	}

	#endregion

	#region movment stats
	private int maximumMovment;
	private int movmentActionsRemaning;

	private void resetMovmentActionsToCurrentMax(){
		movmentActionsRemaning = maximumMovment;
	}

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

	//TODO unit movment animation
	public void displayUnitMovementAnimation(GridBlock location){
		GameObject moveAnimation = Instantiate(grid.getAnimation("unit move")); 
		//TODO setDirection and color of unit movment
	}

	#endregion

	#region attack stats
	/// <summary> The attack locations, 0,0 is the unit 1,0 is the block to the right ext. </summary>
	[SerializeField] private GridLocation[] attackLocations;

	public GridBlock[] getAttackLocations(){
		GridBlock[] x = new GridBlock[attackLocations.Length];

		for(int i = 0; i < attackLocations.Length; i++){
			x[i] = grid.gridLocationToGameGrid(getVirtualBlockHeadLocation().gridlocation + attackLocations[i]);
		}

		return x;
	}

	/// <summary>
	/// The current attacks remaning for the current action queue.
	/// </summary>
	private int currentAttacksRemaning;
	private int currentMaxPosibleAttackActions;

	private void resetAttackActionsToCurrentMax(){
		currentAttacksRemaning = currentMaxPosibleAttackActions;
	}

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

	private int currentAttackPower;

	public int getAttackPower(){
		return currentAttackPower;
	}

	#endregion

	#region unit timeing

	private UnitTimer UT;

	public UnitTimer unitTimer{
		get{
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
		} else{
			readyToAct = true;
			stopTimerTick();
		}
	}

	private void resetTimer(){
		UT.time = 0f;
	}

	private void startTimerTick(){
		InvokeRepeating("timerTick", 0f, 0.01f);
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

	#region unit serialization
	private string unitName;

	private void setUnitNameToLoadOffof(){
		unitName = unitInfo.unitNameForLoad;
	}

	/// <summary>
	/// Serializes the unit.
	/// Primaraly used to insure network instances are synced
	/// </summary>
	/// <returns>The unit.</returns>
	public UnitSaving serializeUnit(){
		UnitSaving serl = new UnitSaving();
		serl.controlType = controlType;
		serl.currentAttackPow = currentAttackPower;
		serl.currentMaxAttackActions = currentMaxPosibleAttackActions;
		serl.currentMaxLength = maxProgramLength;
		serl.currentMaxMove = maximumMovment;
		serl.unitNameToLoad = name;
		serl.currentUnitTimer = unitTimer;
		GridLocation[] BL = new GridLocation[blockList.Count];
		int count = 0;
		foreach( var item in blockList ){
			BL[count++] = item.gridlocation;
		}
		return serl;
	}

	public void loadUnit(UnitSaving unitSave){
		if(unitSave.controlType != null)
			controlType = unitSave.controlType;
		if(unitSave.currentAttackPow != null)
			currentAttackPower = unitSave.currentAttackPow;
		if(unitSave.currentMaxAttackActions != null)
			currentMaxPosibleAttackActions = unitSave.currentMaxAttackActions;
		if(unitSave.currentMaxLength != null)
			maxProgramLength = unitSave.currentMaxLength;
		if(unitSave.currentMaxMove != null)
			maximumMovment = unitSave.currentMaxMove;
		if(unitSave.unitNameToLoad != null)
			name = unitSave.unitNameToLoad;
		if(unitSave.unitNameToLoad != null)
			name = unitSave.unitNameToLoad;
		if(unitSave.currentUnitTimer != null)
			unitTimer = unitSave.currentUnitTimer;
		if(unitSave.currentBlockLocations != null){
			for(int i = 0; i < unitSave.currentBlockLocations.Length; i++){
				addBlock(grid.gridLocationToGameGrid(unitSave.currentBlockLocations[i]), false);
			}
		}
	}

	#endregion
	#region team
	public int getTeam(){
		return team;
	}
	public void setTeam(int num){
		team = num;
	}
	#endregion
	void Start(){
		grid.units.Add(this);
		actionList = new LinkedList<ActionScript>();
		timerStartup();
		startTimerTick();
	}

	// Update is called once per frame
	/// <summary> Update this instance. </summary>
	void Update(){

	}

	/// <summary>
	/// actions can't Instantiate so this allows them to do animations
	/// themselves without adding a buch of extra code to UnitScript.
	/// </summary>
	/// <returns>a new instance of the prefab.</returns>
	/// <param name="prefab">Prefab.</param>
	public GameObject instantiationHelper(GameObject prefab){
		return Instantiate(prefab) as GameObject;
	}

	#region unit destruction
	/// <summary> Destroys the unit. </summary>
	protected void destroyUnit(){
		//TODO make sure there are no refrences to this unit before it is destroyed
		if(tempAction != null)
			tempAction.removeUserSelectionDisplay();
		resetActionQueue(true);
		if(getCurrentBlockHeadLocation() != null){
			getCurrentBlockHeadLocation().removeUnit();	
		}
		if(grid.gui.getCurUnit() == this){
			grid.gui.setSelectedUnit(null);
		}
		Destroy(gameObject);
	}
	#endregion

	/// <summary>
	/// Descriptive code of this unit. Follows the format:
	/// "{name},H:{length}/{macLength},M:{moveCount},A:{attackPow},{attackCount}"
	/// </summary>
	/// <returns>The code string.</returns>
	public virtual string toString() {
		return unitInfo.unitNameForLoad + ",H:" + getLength() + "/" + maxProgramLength + ",M:" + unitInfo.maxMove + ",A:" + unitInfo.attackPow + "," + unitInfo.maxAttackActions;
	}
}