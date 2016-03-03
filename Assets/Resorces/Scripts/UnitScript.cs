	using UnityEngine;
	using System.Collections.Generic;

	/// <summary>
	/// Program script also known as a unit.
	/// Parent script for all programs
	/// </summary>
	public class UnitScript : MonoBehaviour {
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

	#region unit length

	public LinkedList<GridBlock> blockList;
	public GridBlock virtualBlockHead; //blockhead location after a move action is queued

	public GridBlock getBlockHeadLocation() {
		if(virtualBlockHead==null){
			virtualBlockHead = blockList.First.Value;}
		if (virtualBlockHead != blockList.First.Value) {
			return virtualBlockHead;
		} else {
			return blockList.First.Value;
		}
	}

	/// <summary>
	/// called when the grid block creats the unit
	/// </summary>
	/// <param name="startLocation">Start location.</param>
	public virtual void spawnUnit(CreatePlayGrid gm, GridBlock startLocation) {
		grid = gm;
		blockList = new LinkedList<GridBlock>();
		maxProgramLength = unitInfo.maxLength;
		blockList.AddLast(startLocation);
		checkAllDisplay();
	}

	private void checkAllDisplay() {
		foreach(GridBlock loc in  blockList) {
			loc.spriteDisplayScript.updateUnitSprite();
			loc.spriteDisplayScript.checkConection();
		}

	}


	private int maxProgramLength;

	public int MaxProgramLength {
		get {
			return maxProgramLength;
		}
	}

	/// <summary>
	/// modifyes the max length of the program
	/// does not shrink the current size of the unit
	/// </summary>
	/// <param name="value">Value.</param>
	public void setMaxProgramLength(int value) {
		maxProgramLength = value;
	}

	public virtual void receiveDamage(int damageAmount) {
		
	}

	#endregion

	#region basic unit information

	public UnitInformationStruct unitInfo;

	/// <summary>
	/// Gets the color of the unit.
	/// must be overiden by new unit with that units color
	/// </summary>
	/// <returns>The unit color.</returns>
	public virtual Color getUnitColor() {
		return unitInfo.unitColor;
	}

	/// <summary>
	/// The head sprite.
	/// must be set from child unit
	/// </summary>
	public virtual Sprite getUnitHeadSprite() {
		return unitInfo.unitHeadSprite;
	}

	#endregion

	#region display posible actions

	/// <summary>
	/// The grid the unit is on (the level).
	/// </summary>
	private CreatePlayGrid grid;


	[SerializeField] // use this to make private fields visible in the inspector
	#pragma warning disable
	private GameObject[] buttonPrefabs;

	/// <summary>
	/// Used by the gui to display this units posible actions
	/// </summary>
	public GameObject[] getButtonPrefabs() {
		return buttonPrefabs;
	}


	/// <summary>
	/// Sets the grid conection.
	/// this must be set when the unit is created
	/// </summary>
	/// <param name="playGrid">Play grid.</param>
	public void setGridConection(CreatePlayGrid playGrid) {
		grid = playGrid;
	}

	/// <summary>
	/// Displays the action as button.
	/// Each unit will have it's own button prefabs
	/// </summary>
	/// <param name="actionDiscription">The Action's discription.</param>
	/// <param name="button">button that will be displayed on the gui</param>
	public void displayActionsAsButton(string actionDiscription, GameObject button) {
		
	}

	#endregion

	#region Action List

	/// <summary>
	/// A list of all the actions the user has selected for this unit
	/// </summary>
	private LinkedList<ActionScript> actionList;
	/*each action will be a child of the ActionScript */


	public void startActing() {
		invokeNextAction();
	}

	/// <summary>
	/// Invokes the next action.
	/// All actions will run till the actionList is empty
	/// </summary>
	private void invokeNextAction() {
		ActionScript action = actionList.First.Value;
		action.act();
		actionList.RemoveFirst();
		if(actionList.First != null) // only preform another action if there is one
			getReadyToPreformAnotherAction(action.actionTime());
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
		actionList.Last.Value.removeDisplay();
		actionList.RemoveLast();
	}

	public void addActionToQueue(ActionScript action) {
		action.display();
		actionList.AddLast(action);
	}


	public void resetActionQueue(GUIScript gui) {
		foreach(ActionScript actions in actionList) {
			actions.removeDisplay();
		}
	}


	#endregion



	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public void destroyUnit() {
		//TODO make sure there are no refrences to this unit before it is destroyed

		Destroy(gameObject);
	}
}
