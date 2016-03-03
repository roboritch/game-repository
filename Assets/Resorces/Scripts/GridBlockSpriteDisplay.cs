using UnityEngine;
using System.Collections;

/*TODO Add display for selection operations, currently this script is
setup to display all unit actions after they are created.
More will need to be done to display the options for the currently
selected unit.*/
/// <summary>Grid block sprite display.</summary>
public class GridBlockSpriteDisplay : MonoBehaviour
{

	#region basic vars
	/// <summary>The info of this sprite.</summary>
	private CreatePlayGrid spriteInfo;
	/*using gameObject.getComponent has a larger overhead than a direct refrence */
	/// <summary>The grid block of this sprite.</summary>
	private GridBlock attachedGridBlock;
	/// <summary>The conection location of this sprite.</summary>
	public float conectionLocation = 0.5f;
	//TODO hard code this
	private int MAX_ACTIONS_ON_THIS_BLOCK = 4;
	// this may nead to be changed

	#endregion

	#region spriteControlers
	/// <summary>The sprite of this block's unit.</summary>
	private SpriteControler unitSprite;
	/// <summary>The head sprite of this block's unit.</summary>
	private SpriteControler headSprite;
	/// <summary>The action sprites of this block's unit.</summary>
	private SpriteControler[] actionSprites;
	/// <summary>actions used on this block.</summary>
	private SpriteControler moveSprite;
	private bool[] actionUsed;
	/// <summary>The above sprite controller conection.</summary>
	private SpriteControler aboveConection;
	/// <summary>The right sprite controller conection.</summary>
	private SpriteControler rightConection;

	#endregion

	#region script startup

	// all sprite layers are created on load to minimize the impact of changing sprites
	/// <summary>Start this instance.</summary>
	void Start ()
	{ 
		spriteInfo = GetComponentInParent<CreatePlayGrid> ();
		attachedGridBlock = GetComponent<GridBlock> ();

		//initialize unit sprite
		unitSprite = Instantiate (spriteInfo.spritePrefab).GetComponent<SpriteControler> ();
		unitSprite.transform.SetParent (transform);
		unitSprite.transform.localPosition = new Vector3 (0, 0, 0.1f);
		unitSprite.name = "Unit Sprite";

		//initialize unit head sprite
		headSprite = Instantiate (spriteInfo.spritePrefab).GetComponent<SpriteControler> ();
		headSprite.transform.SetParent (transform);
		headSprite.transform.localPosition = new Vector3 (0, 0, 0.2f);
		headSprite.name = "Head Sprite";

		//initialize action sprites and use state
		actionSprites = new SpriteControler[MAX_ACTIONS_ON_THIS_BLOCK];
		actionUsed = new bool[MAX_ACTIONS_ON_THIS_BLOCK];
		for (int x = 0; x < MAX_ACTIONS_ON_THIS_BLOCK; x++) {
			actionSprites [x] = Instantiate (spriteInfo.spritePrefab).GetComponent<SpriteControler> ();
			actionSprites [x].transform.SetParent (transform);
			actionSprites [x].transform.localPosition = new Vector3 (0, 0, 0.3f + x * 0.05f);
			actionSprites [x].name = "Action Sprite " + x;
		}

		//initialize movement directions
		movmentDirections = new SpriteControler[4];
		string[] directionNames = { "up", "right", "down", "left" };
		for (int x = 0; x < 4; x++) {
			movmentDirections [x] = Instantiate (spriteInfo.spritePrefab).GetComponent<SpriteControler> ();
			movmentDirections [x].transform.SetParent (transform);
			movmentDirections [x].transform.localPosition = new Vector3 (0, 0, 2.5f);
			movmentDirections [x].name = "Movment Direction " + directionNames [x];
			movmentDirections [x].setSprite (spriteInfo.spritesAndColors.sprite_moveLine); // sprites for this controler are always the same
			movmentDirections [x].setColor (Color.clear); //The color for this sprite is Alpha only
		}

		//initialize movement circle
		movmentCirlcle = Instantiate (spriteInfo.spritePrefab).GetComponent<SpriteControler> ();
		movmentCirlcle.transform.SetParent (transform);
		movmentCirlcle.transform.localPosition = new Vector3 (0, 0, 2.5f);
		movmentCirlcle.name = "Movement Circle";

		//initialize right connection
		rightConection = Instantiate (spriteInfo.spritePrefab).GetComponent<SpriteControler> ();
		rightConection.transform.SetParent (transform);
		rightConection.transform.localPosition = new Vector3 (0, conectionLocation, 0.1f);
		rightConection.name = "Right Conection";

		//initialize above connection
		aboveConection = Instantiate (spriteInfo.spritePrefab).GetComponent<SpriteControler> ();
		aboveConection.transform.SetParent (transform);
		aboveConection.transform.localPosition = new Vector3 (conectionLocation, 0, 0.1f);
		aboveConection.name = "Above Conection";

		moveSprite = Instantiate (spriteInfo.spritePrefab).GetComponent<SpriteControler> ();
		moveSprite.transform.SetParent (transform);
		moveSprite.transform.localPosition = new Vector3 (0, 0, 0.2f);
		moveSprite.name = "Move Sprite";

	}

	#endregion

	#region check conections

	/// <summary>
	/// Checks the conections of the unit.
	/// This should be called by every segment of a unit where that unit was
	/// before a change and by any new unit segments
	/// </summary>
	public void checkConection ()
	{
		if (attachedGridBlock.unitInstalled == null) {
			Debug.LogWarning ("this block has no program to check connections on");
			return;
		}

		if (attachedGridBlock.right != null)
			checkRight ();
		if (attachedGridBlock.up != null)
			checkAbove ();
		if (attachedGridBlock.left != null)
			attachedGridBlock.left.spriteDisplayScript.checkRight (); // check left
		if (attachedGridBlock.down != null)
			attachedGridBlock.down.spriteDisplayScript.checkAbove (); // check right
	}


	/// <summary>
	/// Checks the right sprite.
	/// Only to be called by another GridBlockSpriteDisplay script.
	/// </summary>
	public void checkRight ()
	{
		if (attachedGridBlock.unitInstalled == attachedGridBlock.right.unitInstalled) {
			rightConection.setSprite (spriteInfo.spritesAndColors.sprite_unitConecter, unitSprite.getColor ());
		} else {
			rightConection.removeSprite ();
		}
	}

	/// <summary>
	/// Checks the above sprite.
	/// Only to be called by another GridBlockSpriteDisplay script.
	/// </summary>
	public void checkAbove ()
	{
		if (attachedGridBlock.unitInstalled == attachedGridBlock.up.unitInstalled) {
			aboveConection.setSprite (spriteInfo.spritesAndColors.sprite_unitConecter, unitSprite.getColor ());
		} else {
			aboveConection.removeSprite ();
		}
	}

	#endregion

	#region check unit sprite and head

	//All things that need to be diplayed will have there own function.

	/// <summary>
	/// Updates this sprite by setting the appropriate unit sprite and checking the
	/// head sprite, or removes both sprites if no unit is present.
	/// The unit information must be setup on the grid block before this is called.
	/// </summary>
	public void updateUnitSprite () {
		if (attachedGridBlock.unitInstalled == null) {
			unitSprite.removeSprite ();
			headSprite.removeSprite ();
		} else {
			unitSprite.setSprite (spriteInfo.spritesAndColors.sprite_unit, attachedGridBlock.unitInstalled.getUnitColor ());
			checkHeadSprite ();
		}
	}

	/// <summary>
	/// Updates this sprite by setting the appropriate unit head sprite,
	/// or removes both sprites if no unit is present.
	/// Displays head if this this the correct head location.
	/// removes head sprite if it is not.
	/// </summary>
	public void checkHeadSprite() {
		if(attachedGridBlock.unitInstalled.getBlockHeadLocation().gridlocation == attachedGridBlock.gridlocation) {
			headSprite.setSprite(attachedGridBlock.unitInstalled.getUnitHeadSprite());
		} else {
			headSprite.removeSprite ();
		}
	}


	#endregion

	#region action display

	/// <summary>
	/// Displays the action.
	/// </summary>
	/// <returns>The actionID</returns>
	/// <param name="aS">action sprite.</param>
	public int displayAction (Sprite aS)
	{
		int actionID = 0;
		while (actionUsed [actionID]) {
			actionID++;
		}
		actionSprites [actionID].setSprite (aS);
		actionUsed [actionID] = true;
		return actionID;
	}

	/// <summary>
	/// Removes the action.
	/// </summary>
	/// <param name="actionID">ActionID.</param>
	public void removeAction (int actionID)
	{
		actionSprites [actionID].removeSprite ();
		actionUsed [actionID] = false;
	}

	#endregion

	#region movment display

	//vars are complex read descriptions for more info
	private SpriteControler[] movmentDirections;
	/// <summary>
	/// Unit movment overlap.
	/// The number of units ready to move over this block.
	/// </summary>
	private int[] UMO = { 0, 0, 0, 0 };
	private SpriteControler movmentCirlcle;


	//TODO work on unit overlap
	/// <summary>
	/// Display units projected movment through this block.
	/// Set cameFrom as NULL if start location. 
	/// Set isGoing as NULL if end location.
	/// </summary>
	/// <param name="cameFrom">block this unit is coming from.</param>
	/// <param name="isGoingTo">block this unit is going to.</param>
	/// <param name="display">is the unit moving over this block now or is this just a display?</param>
	public void displayMoveOnCreate (GridBlock cameFrom, GridBlock isGoingTo, bool display)
	{
		int umoModifyer;
		if (display) {
			umoModifyer = 1;
		} else {
			umoModifyer = -1;
		}

		SpriteControler moveArmIn;
		SpriteControler moveArmOut;

		if (cameFrom == attachedGridBlock.up) {
			moveArmIn = movmentDirections [0];
			UMO [0] += umoModifyer;
			moveArmIn.setColor (UMO_ColorConverter (UMO [0]));
		} else if (cameFrom == attachedGridBlock.right) {
			moveArmIn = movmentDirections [1];
			UMO [1] += umoModifyer;
			moveArmIn.setColor (UMO_ColorConverter (UMO [1]));
		} else if (cameFrom == attachedGridBlock.down) {
			moveArmIn = movmentDirections [2];
			UMO [2] += umoModifyer;
			moveArmIn.setColor (UMO_ColorConverter (UMO [2]));
		} else if (cameFrom == attachedGridBlock.left) {
			moveArmIn = movmentDirections [3];
			UMO [3] += umoModifyer;
			moveArmIn.setColor (UMO_ColorConverter (UMO [3]));
		} else {
			moveArmIn = null;
		}

		if (isGoingTo == attachedGridBlock.up) {
			moveArmOut = movmentDirections [0];
			UMO [0] += umoModifyer;
			moveArmOut.setColor (UMO_ColorConverter (UMO [0]));
		} else if (isGoingTo == attachedGridBlock.right) {
			moveArmOut = movmentDirections [1];
			UMO [1] += umoModifyer;
			moveArmOut.setColor (UMO_ColorConverter (UMO [1]));
		} else if (isGoingTo == attachedGridBlock.down) {
			moveArmOut = movmentDirections [2];
			UMO [2] += umoModifyer;
			moveArmOut.setColor (UMO_ColorConverter (UMO [2]));
		} else if (isGoingTo == attachedGridBlock.left) {
			moveArmOut = movmentDirections [3];
			UMO [3] += umoModifyer;
			moveArmOut.setColor (UMO_ColorConverter (UMO [3]));
		} else {
			moveArmOut = null;
		}

		if (UMO [0] == 0 && UMO [1] == 0 && UMO [2] == 0 && UMO [3] == 0) {
			movmentCirlcle.setColor (Color.clear);
		} else {
			movmentCirlcle.setColor (Color.white);
		}
	}

	private Color UMO_ColorConverter (int numberOverlap)
	{
		switch (numberOverlap) {
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
	public void moveAction(UnitScript unit){
		if(unit.getBlockHeadLocation().right!=null){
			unit.getBlockHeadLocation ().right.spriteDisplayScript.moveSprite.setSprite(spriteInfo.spritesAndColors.sprite_moveTo);
		}
		if(unit.getBlockHeadLocation().left!=null){
			unit.getBlockHeadLocation ().left.spriteDisplayScript.moveSprite.setSprite(spriteInfo.spritesAndColors.sprite_moveTo);
		}
		if(unit.getBlockHeadLocation()){}

	}

	#endregion

	// Update is called once per frame
	void Update ()
	{
	
	}
}
