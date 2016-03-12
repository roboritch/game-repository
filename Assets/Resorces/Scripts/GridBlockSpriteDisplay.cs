﻿using UnityEngine;
using System.Collections;

/*TODO Add display for selection operations, currently this script is
setup to display all unit actions after they are created.
More will need to be done to display the options for the currently
selected unit.*/
using System;


/// <summary>Grid block sprite display.</summary>
public class GridBlockSpriteDisplay : MonoBehaviour {

	#region basic vars

	/// <summary>The info of this sprite.</summary>
	private CreatePlayGrid spriteInfo;
	/*using gameObject.getComponent has a larger overhead than a direct refrence */
	/// <summary>The grid block of this sprite.</summary>
	private GridBlock attachedGridBlock;
	/// <summary>The connection location of this sprite.</summary>
	public float conectionLocation = 0.5f;
	//TODO hard code this, this may nead to be changed
	private int MAX_ACTIONS_ON_THIS_BLOCK = 4;

	#endregion

	#region spriteControlers

	/// <summary>The sprite of this block's unit.</summary>
	private SpriteControler unitSprite;
	/// <summary>The head sprite of this block's unit.</summary>
	private SpriteControler headSprite;
	/// <summary>The action sprites of this block's unit.</summary>
	private SpriteControler[] actionSprites;
	/// <summary>Actions used on this block.</summary>
	private SpriteControler moveSprite;
	/// <summary>State of actions used on this block.</summary>
	private bool[] actionUsed;
	/// <summary>The above sprite controller connection.</summary>
	private SpriteControler aboveConection;
	/// <summary>The right sprite controller connection.</summary>
	private SpriteControler rightConection;


	#endregion

	#region script startup

	// All sprite layers are created on load to minimize the impact of changing sprites
	/// <summary>Start this instance.</summary>
	void Start() { 
		spriteInfo = GetComponentInParent<CreatePlayGrid>();
		attachedGridBlock = GetComponent<GridBlock>();

		//Initialize unit sprite.
		unitSprite = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
		unitSprite.transform.SetParent(transform);
		unitSprite.transform.localPosition = new Vector3(0, 0, 0.1f);
		unitSprite.name = "Unit Sprite";

		//Initialize unit head sprite.
		headSprite = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
		headSprite.transform.SetParent(transform);
		headSprite.transform.localPosition = new Vector3(0, 0, 0.2f);
		headSprite.name = "Head Sprite";

		//Initialize action sprites and use state.
		actionSprites = new SpriteControler[MAX_ACTIONS_ON_THIS_BLOCK];
		actionUsed = new bool[MAX_ACTIONS_ON_THIS_BLOCK];
		for(int x = 0; x < MAX_ACTIONS_ON_THIS_BLOCK; x++) {
			actionSprites[x] = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
			actionSprites[x].transform.SetParent(transform);
			actionSprites[x].transform.localPosition = new Vector3(0, 0, 0.3f + x * 0.05f);
			actionSprites[x].name = "Action Sprite " + x;
		}

		//Initialize movement directions.
		movmentDirections = new SpriteControler[4];
		string[] directionNames = { "up", "right", "down", "left" };
		for(int x = 0; x < 4; x++) {
			movmentDirections[x] = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
			movmentDirections[x].transform.SetParent(transform);
			movmentDirections[x].transform.localPosition = new Vector3(0, 0, 2.5f);
			movmentDirections[x].transform.Rotate(0.0f, 90.0f * x, 0.0f); //UNDONE check to see if rotation is correct
			movmentDirections[x].name = "Movment Direction " + directionNames[x];
			// Sprites for this controller are always the same.
			movmentDirections[x].setSprite(spriteInfo.spritesAndColors.sprite_moveLine);
			//The color for this sprite is Alpha only.
			movmentDirections[x].setColor(Color.clear);
		}

		//Initialize movement circle.
		movmentCirlcle = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
		movmentCirlcle.transform.SetParent(transform);
		movmentCirlcle.transform.localPosition = new Vector3(0, 0, 2.5f);
		movmentCirlcle.name = "Movement Circle";

		//Initialize right connection.
		rightConection = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
		rightConection.transform.SetParent(transform);
		rightConection.transform.localPosition = new Vector3(0, conectionLocation, 0.1f);
		rightConection.name = "Right Conection";

		//Initialize above connection.
		aboveConection = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
		aboveConection.transform.SetParent(transform);
		aboveConection.transform.localPosition = new Vector3(conectionLocation, 0, 0.1f);
		aboveConection.name = "Above Conection";

		//Initialize move sprite.
		moveSprite = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
		moveSprite.transform.SetParent(transform);
		moveSprite.transform.localPosition = new Vector3(0, 0, 0.2f);
		moveSprite.name = "Move Sprite";

	}

	#endregion

	#region check conections

	/// <summary>
	/// Checks the connections of the unit.
	/// This should be called by every segment of a unit where that unit was
	/// before a change and by any new unit segments.
	/// </summary>
	public void checkConection() {
		if(attachedGridBlock.unitInstalled == null) {
			Debug.LogWarning("This block has no program to check connections on.");
			return;
		}

		if(attachedGridBlock.right != null)
			checkRight();
		if(attachedGridBlock.up != null)
			checkAbove();
		if(attachedGridBlock.left != null)
			attachedGridBlock.left.spriteDisplayScript.checkRight(); // check left
		if(attachedGridBlock.down != null)
			attachedGridBlock.down.spriteDisplayScript.checkAbove(); // check down
	}


	/// <summary>
	/// Checks the right sprite.
	/// Only to be called by another GridBlockSpriteDisplay script.
	/// </summary>
	public void checkRight() {
		if(attachedGridBlock.unitInstalled == attachedGridBlock.right.unitInstalled) {
			rightConection.setSprite(spriteInfo.spritesAndColors.sprite_unitConecter, unitSprite.getColor());
		} else {
			rightConection.removeSprite();
		}
	}

	/// <summary>
	/// Checks the above sprite.
	/// Only to be called by another GridBlockSpriteDisplay script.
	/// </summary>
	public void checkAbove() {
		if(attachedGridBlock.unitInstalled == attachedGridBlock.up.unitInstalled) {
			aboveConection.setSprite(spriteInfo.spritesAndColors.sprite_unitConecter, unitSprite.getColor());
		} else {
			aboveConection.removeSprite();
		}
	}

	#endregion

	#region check unit sprite and head

	//All things that need to be diplayed will have their own function.

	/// <summary>
	/// Updates this sprite by setting the appropriate unit sprite and checking the
	/// head sprite, or removes both sprites if no unit is present.
	/// The unit information must be setup on the grid block before this is called.
	/// </summary>
	public void updateUnitSprite() {
		if(attachedGridBlock.unitInstalled == null) {
			unitSprite.removeSprite();
			headSprite.removeSprite();
		} else {
			unitSprite.setSprite(spriteInfo.spritesAndColors.sprite_unit, attachedGridBlock.unitInstalled.getUnitColor());
			checkHeadSprite();
		}
	}

	/// <summary>
	/// Updates this sprite by setting the appropriate unit head sprite,
	/// or removes both sprites if no unit is present.
	/// Displays head if this this the correct head location,
	/// removes head sprite if it is not.
	/// </summary>
	public void checkHeadSprite() {
		if(attachedGridBlock.unitInstalled.getVirtualBlockHeadLocation().gridlocation == attachedGridBlock.gridlocation) {
			headSprite.setSprite(attachedGridBlock.unitInstalled.getUnitHeadSprite());
		} else {
			headSprite.removeSprite();
		}
	}


	#endregion

	#region action display

	/// <summary>
	/// Displays the action.
	/// </summary>
	/// <returns>The actionID</returns>
	/// <param name="aS">action sprite.</param>
	public int displayAction(Sprite aS) {
		int actionID = 0;
		while(actionUsed[actionID]) {
			actionID++;
		}
		actionSprites[actionID].setSprite(aS);
		actionUsed[actionID] = true;
		return actionID;
	}

	/// <summary>
	/// Removes the action.
	/// </summary>
	/// <param name="actionID">ActionID.</param>
	public void removeAction(int actionID) {
		actionSprites[actionID].removeSprite();
		actionUsed[actionID] = false;
	}

	#endregion

	#region movment display

	//vars are complex read descriptions for more info
	public SpriteControler[] movmentDirections;
	/// <summary>
	/// Unit movment overlap.
	/// The number of units ready to move over this block.
	/// </summary>
	private int[] UMO = { 0, 0, 0, 0 };
	private SpriteControler movmentCirlcle;


	//TODO work on unit overlap
	/// <summary>
	/// Display unit's projected movement through this block.
	/// </summary>
	/// <param name="cameFrom">Block this unit is coming from.</param>
	/// <param name="isGoingTo">Block this unit is going to.</param>
	///<returns> A refrence to the spriteControlers for moveArmIn (0) and moveArmOut (1)
	/// when the action is preformed set there color to Color.Clear</returns>
	public SpriteControler[] displayMoveOnQueue(GridBlock cameFrom, GridBlock isGoingTo) {

		SpriteControler moveArmIn = null;
		SpriteControler moveArmOut = null;

		GridBlock tempAdj;
		for(int i = 0; i < 4; i++) { //
			tempAdj = cameFrom.getAdjTranslated(i);
			if(tempAdj == isGoingTo) {
				moveArmIn = movmentDirections[i];
				i+=2;
				if(i > 3){i -= 4;}// faster than mod 4
				moveArmOut = tempAdj.spriteDisplayScript.movmentDirections[i];
				break;
			}
		}
		//TODO make move arm in add out visible using color.
		if(moveArmIn ==null || moveArmOut ==null){
			Debug.LogError("cameFrom and isGoingTo were not adjacent!\n" +
				"null was returned to the action");
			return null;
		}
		moveArmIn.setColor(Color.white);
		moveArmOut.setColor(Color.white);

		SpriteControler[] sc = {moveArmIn,moveArmOut};

		return sc;
	}

	/// <summary>
	/// UMOs color converter.
	/// not currently used
	/// </summary>
	/// <returns>The o color converter.</returns>
	/// <param name="numberOverlap">Number overlap.</param>
	private Color UMO_ColorConverter(int numberOverlap) {
		switch(numberOverlap) {
		case 0:
			return Color.clear;
		case 1:
			return Color.white;
		case 2:
			return Color.yellow;
		case 3:
			return Color.magenta;
		default:
			return Color.blue;
		}
	}

	/// <summary>
	/// Display the move sprite over this gridblock.
	/// </summary>
	public void displayMoveSprite() {
		moveSprite.setSprite(spriteInfo.spritesAndColors.sprite_moveTo);
	}

	/// <summary>
	/// Removes the move sprite from this display.
	/// </summary>
	public void removeMoveSprite() {
		moveSprite.removeSprite();
	}

	#region old move code

	/// <summary>
	/// Draws all available unit move sprites.
	/// </summary>
	public bool moveDisplayOn = false;
	public Direction dir;
	public UnitScript unitMoving;
	//temp keeps the up,down,left,right blocks' spriteDisplay for removing the move sprite
	public GridBlockSpriteDisplay tempUp;
	public GridBlockSpriteDisplay tempDown;
	public GridBlockSpriteDisplay tempLeft;
	public GridBlockSpriteDisplay tempRight;

	[Obsolete("Move script will handle refrences", true)]
	public void moveAction(UnitScript unit) {
		GridBlock up = unit.getVirtualBlockHeadLocation().up;
		GridBlock down = unit.getVirtualBlockHeadLocation().down;
		GridBlock left = unit.getVirtualBlockHeadLocation().left;
		GridBlock right = unit.getVirtualBlockHeadLocation().right;
		if(right != null) {
			GridBlockSpriteDisplay tempRight = right.spriteDisplayScript;
			tempRight.moveSprite.setSprite(spriteInfo.spritesAndColors.sprite_moveTo);
			tempRight.moveDisplayOn = true;
			tempRight.unitMoving = unit;
			dir = Direction.RIGHT;
		}
		if(left != null) {
			GridBlockSpriteDisplay tempLeft = left.spriteDisplayScript;
			tempLeft.moveSprite.setSprite(spriteInfo.spritesAndColors.sprite_moveTo);
			tempLeft.moveDisplayOn = true;
			tempLeft.unitMoving = unit;
			dir = Direction.LEFT;
		}
		if(up != null) {
			GridBlockSpriteDisplay tempUp = up.spriteDisplayScript;
			tempUp.moveSprite.setSprite(spriteInfo.spritesAndColors.sprite_moveTo);
			tempUp.moveDisplayOn = true;
			tempUp.unitMoving = unit;
			dir = Direction.UP;
		}
		if(down != null) {
			GridBlockSpriteDisplay tempDown = down.spriteDisplayScript;
			tempDown.moveSprite.setSprite(spriteInfo.spritesAndColors.sprite_moveTo);
			tempDown.moveDisplayOn = true;
			tempDown.unitMoving = unit;
			dir = Direction.DOWN;
		}

	}

	#endregion

	#endregion

	// Update is called once per frame.
	/// <summary> Update this instance.</summary>
	void Update() {
	
	}
}
