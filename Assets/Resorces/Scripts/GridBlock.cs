using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Grid block belonging to the play grid.
/// Contains pointers to the four adjcent blocks.
/// Contains a sprite.
/// Can be online or offline (occupiable or not).
/// Can be a spawn spot.
/// </summary>
public class GridBlock : MonoBehaviour,IPointerDownHandler{
	
	//Adjacent Blocks.
	/// <summary>Upper adjacent block.</summary>
	private GridBlock up;
	/// <summary>Lower adjacent block.</summary>
	private GridBlock down;
	/// <summary>Left adjacent block.</summary>
	private GridBlock left;
	/// <summary>Right adjacent block.</summary>
	private GridBlock right;

	public Collider gridBlockCollider;

	//Block properties.
	/// <summary>Whether this block is a spawn spot.</summary>
	[SerializeField] private bool spawnSpot = false;

	/// <summary>Whether this block is an occupiable space.</summary>
	[SerializeField] private bool available = true;

	/// <summary>Sprite display for this block.</summary>
	public GridBlockSpriteDisplay spriteDisplayScript;

	//Block attributes.
	/// <summary>The unit currently occupying this space.</summary>
	public UnitScript unitInstalled;
	/// <summary> The action waiting for user input on this block. </summary>
	public ActionScript actionWaitingForUserInput;
	/// <summary>The game's grid manager.</summary>
	private CreatePlayGrid gridManager;
	/// <summary>Collision box of this space.</summary>
	private Collider2D selectionBox;

	/// <summary>Location of this grid block on the play grid.</summary>
	public GridLocation gridLocation;

	//Attack attributes
	private int attackActionID = -1;
	private Team teamSpawn;
	private bool aiSpawn = false;

	#region Adjacent Blocks

	public bool isAdj(GridBlock blk){
		if(blk == up || blk == down || blk == left || blk == right){
			return true;
		} else{
			return false;
		}
	}


	public GridBlock getAdj(int dir){
		if(dir == (int)Direction.UP){
			return up;
		} else if(dir == (int)Direction.LEFT){
			return left;
		} else if(dir == (int)Direction.DOWN){
			return down;
		} else if(dir == (int)Direction.RIGHT){
			return right;
		}
		return null;
	}

	public GridBlock getAdj(Direction dir){
		switch(dir){
		case Direction.UP:
			return up;
		case Direction.DOWN:
			return down;
		case Direction.LEFT:
			return left;
		default:
			return right;
		}
	}

	public Direction blockAdjDirection(GridBlock block){
		if(block == up){
			return Direction.UP;
		} else if(block == down){
			return Direction.DOWN;
		} else if(block == left){
			return Direction.LEFT;
		} else if(block == right){
			return Direction.RIGHT;
		} else{
			return Direction.UP;
		} 
	}

	public void setAdj(Direction dir, GridBlock block){
		switch(dir){
		case Direction.UP:
			up = block;
			break;
		case Direction.DOWN:
			down = block;
			break;
		case Direction.LEFT:
			left = block;
			break;
		default:
			right = block;
			break;
		}
	}

	#endregion

	/// <summary>Display the connections.</summary>
	/// <param name="unit">Unit.</param>
	void displayConnections(UnitScript unit){
		/*TODO Call this every time a unit changes size or moves to 
		 * correctly display the conections in a single program. */
	}

	/// <summary>Sets the grid manager.</summary>
	/// <value>The grid manager.</value>
	public CreatePlayGrid GridManager{
		set{
			gridManager = value;
		}
		get{
			return gridManager;
		}
	}

	#region Mouse Events

	/// <summary>Raises the mouse down event.</summary>
	void OnMouseDown(){ //TODO there could be a better way to handle these events
		
	}

	/// <summary>Raises the mouse over event.</summary>
	void OnMouseOver(){

	}

	#endregion

	#region IPointerDownHandler Implementation (Mouse Events)

	// The GUI will now block clicks.
	public void OnPointerDown(PointerEventData eventData){
		if(gridManager.editModeOn){ // only this if block will be called if edit mode is on
			if(!gridManager.contextMenuUp){
				Debug.Log("mouse down on grid block");
				gridManager.contextMenuUp = true;
				displayEditRightClickMenu();
			}
			return;
		}
		//Set the buttons up in the GUI for the installed unit when this grid block is selected.
		//All previous buttons are removed when this method is called.
		//If the mouse button is pressed, and this block is a spawn spot and is not currently occupied by a unit.
		if(teamSpawn != null)
		if((Player.Instance.playerAlliance == teamSpawn.getIndex() || Player.Instance.playerAlliance == -1) && spawnSpot && unitInstalled == null && actionWaitingForUserInput == null && Input.GetMouseButton(0)){
			gridManager.gui.unitSelectionScript.enableOnGridBlock(this);
		}
		if(actionWaitingForUserInput is MoveScript){
			actionWaitingForUserInput.userSelectedAction(this);
		} else if(actionWaitingForUserInput is AttackScript){
			actionWaitingForUserInput.userSelectedAction(this);
		} else if(!GridManager.editModeOn && unitInstalled == null && Input.GetMouseButton(0)){
			if(gridManager.gui.getCurUnit() != null)
				gridManager.gui.getCurUnit().removeUserSelectionDisplay();
			gridManager.gui.setSelectedUnit(null);
			//Only on left click.
		} else if(unitInstalled != null && Input.GetMouseButton(0)){
			if(unitInstalled.getTeam().getIndex() == Player.Instance.playerAlliance || Player.Instance.playerAlliance == -1) //only a player can select a unit
				gridManager.gui.setSelectedUnit(unitInstalled);
		}
	
	}


	#endregion

	#region Attack Handling

	public void attackActionWantsToAttackHere(AttackScript attack, UnitScript unitAttacking){
		// Unit can't attack nothing or itself.
		if(unitInstalled != null && unitInstalled != unitAttacking){
			if(attackActionID == -1){
				attackActionID = spriteDisplayScript.displayAction(gridManager.spritesAndColors.sprite_attack);
				actionWaitingForUserInput = attack;
			} else{
				Debug.Log("attack action already displayed");
			}
		}
	}

	public void removeAttackDisplayForThis(){
		spriteDisplayScript.removeAction(attackActionID);
		attackActionID = -1;
		actionWaitingForUserInput = null;
	}


	#endregion

	#region Edit Mode

	/// <summary>Displays the edit right click menu.</summary>
	//UNDONE Display menu on right click.
	public void displayEditRightClickMenu(){
		GameObject contextMenu = Instantiate(gridManager.gridEditMenu) as GameObject;
		contextMenu.GetComponent<ContextCanvas>().space = this;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		Debug.DrawRay(ray.origin, ray.direction, Color.red, 50f);
		if(Physics.Raycast(ray, out hit)){
			contextMenu.transform.position = hit.point + new Vector3(0, 3f, 0);
			//Debug.Log(hit);
		}
	}

	/// <summary>
	/// Removes the current context menu for this block and alows new context menus
	/// to be created for other blocks.
	/// </summary>
	internal void ExitContextMenu(){
		gridManager.contextMenuUp = false;
	}

	/// <summary>
	/// <para>Prints the state of the space for level saving.</para>
	/// <para>" " = offline</para>
	/// <para>"o" = spawn spot</para>
	/// <para>"x" = other</para>
	/// </summary>
	public string printSpaceState(){
		if(available == false)
			return " ";
		if(spawnSpot)
			return "o";
		return "x";
		//TODO Make level saver.
	}

	/// <summary>Toggle the online state of this block.</summary>
	public void toggleSpaceAvailable(){ 
		if(available == false){
			if(up != null)
				up.down = this;
			if(down != null)
				down.up = this;
			if(left != null)
				left.right = this;
			if(right != null)
				right.left = this;

			setSpriteDefault();
		} else{
			if(up != null)
				up.down = null;
			if(down != null)
				down.up = null;
			if(left != null)
				left.right = null;
			if(right != null)
				right.left = null;

			setSpriteNone();
			spawnSpot = false;
		}
		available = !available;
	}

	/// <summary>Sets gridblock as a spawn spot.</summary>
	public void setSpawn(Team ts, bool AI){
		//fail to set spawn if block is offline
		if(!available)
			return;
		spawnSpot = true;
		teamSpawn = ts;
		//set spawn sprite
		setSpriteSpawn();
		//Add to team spawn block list.
		ts.spawnBlocks.AddLast(this);
		setAIOnThisSpawnBlock(AI);
	}

	public void setTeamSpawnAlliance(Team team, bool AI){
		teamSpawn.removeSpawn();
		teamSpawn = team;
		setSpriteSpawn();
		team.spawnBlocks.AddLast(this);
		setAIOnThisSpawnBlock(AI);
	}

	public void setAIOnThisSpawnBlock(bool aiState){
		//TODO AI spawn block code
		aiSpawn = aiState;
	}


	public void setTeamSpawnAlliance(int teamNumb, bool aiState){
		setTeamSpawnAlliance(gridManager.team[teamNumb], aiState);
	}

	/// <summary>Sets gridblock from a spawn spot to a default spot.</summary>
	public void removeSpawn(){
		//Fail to remove spawn if block is offline.
		if(!available)
			return;
		spawnSpot = false;
		//Set default sprite.
		setSpriteDefault();
	}


	public Team getTeam(){
		return teamSpawn;
	}
		
	#endregion

	#region Unit Control

	/// <summary>Spawns a given unit
	/// the units alliance must be 
	/// set before this is called
	/// .</summary>
	/// <param name="unit">Unit.</param>
	public void spawnUnit(UnitScript unit){
		UnitAI ai = new UnitAI(unit);
		unit.ai = ai;
		unit.transform.position = new Vector3();
		unit.transform.SetParent(gridManager.unitObjectHolder);
		unitInstalled = unit;
		unit.spawnUnit(gridManager, this, teamSpawn);
	}

	public void removeUnit(){
		unitInstalled = null;
	}

	public void spawnAIUnit(UnitScript unit){
		UnitAI ai = new UnitAI(unit);
		unit.ai = ai;
		spawnUnit(unit);
	}

	public void spawUnitPlayerFromNetwork(string unitName){//change Alliance to team.getTeamIndex
		Player.Instance.thisPlayersNetworkHelper.Cmd_SendUnitSpawnEventToServer(unitName, (ushort)gridLocation.x, (ushort)gridLocation.y, (byte)Player.Instance.playerAlliance);
	}
	#endregion

	// Use this for initialization.
	/// <summary>Start this instance.</summary>
	void Start(){
		gridBlockCollider = GetComponent<Collider>();
		spriteDisplayScript = GetComponent<GridBlockSpriteDisplay>();
	}

	// Update is called once per frame.
	/// <summary>Update this instance.</summary>
	void Update(){
	}

	#region Sprite Controls

	/// <summary>Sets the sprite to default.</summary>
	private void setSpriteDefault(){
		transform.GetComponent<SpriteControler>().setSprite(gridManager.spritesAndColors.sprite_defaultSpace, gridManager.spritesAndColors.color_defaultSpaceColor);
	}

	/// <summary>Sets the sprite to spawn.</summary>
	private void setSpriteSpawn(){
		transform.GetComponent<SpriteControler>().setSprite(gridManager.spritesAndColors.sprite_spawnSpace, Team.colorBlend(teamSpawn.getColor(), Color.black, 0.8f));
	}

	/// <summary>Removes the sprite.</summary>
	private void setSpriteNone(){
		transform.GetComponent<SpriteControler>().removeSprite();
	}

	#endregion

	#region Block Properties Accessors

	// Return property of gridblock if it is a spawn spot or not
	public bool isSpawnSpot(){
		return spawnSpot;
	}

	/// <summary>
	/// Gets the whenther this spawn is AI.
	/// </summary>
	/// <returns>The AI spawn.</returns>
	public bool getAISpawn(){
		return aiSpawn;
	}

	// Return property of gridblock if it is occupiable space or a wall
	public bool getAvailable(){
		return available;
	}

	/// <summary>
	/// Gets the team number.
	/// </summary>
	/// <returns>The team number. -1 if no team</returns>
	public int getTeamNumber(){
		if(teamSpawn == null){
			return -1;
		} else{
			return teamSpawn.getIndex();
		}
	}


	#endregion
}