using UnityEngine;
using System.Collections;

public class GridBlockSpriteDisplay : MonoBehaviour {

	#region basic vars

	private CreatePlayGrid spriteInfo;
	/*using gameObject.getComponent has a larger overhead than a direct refrence */
	private GridBlock attachedGridBlock;
	public float conectionLocation = 0.5f;
	//TODO hard code this
	private int MAX_ACTIONS_ON_THIS_BLOCK = 4;
	// this may nead to be changed

	#endregion

	#region spriteControlers

	private SpriteControler unitSprite;
	private SpriteControler headSprite;
	private SpriteControler[] actionSprites;
	private bool[] actionUsed;
	private SpriteControler aboveConection;
	private SpriteControler rightConection;

	#endregion

	#region script startup

	// all sprite layers are created on load to minimize the impact of changing sprites
	void Start() { 
		spriteInfo = GetComponentInParent<CreatePlayGrid>();
		attachedGridBlock = GetComponent<GridBlock>();

		unitSprite = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
		unitSprite.transform.SetParent(transform);
		unitSprite.transform.localPosition.Set(0, 0, 0.1f);
		unitSprite.name = "Unit Sprite";

		headSprite = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
		headSprite.transform.SetParent(transform);
		headSprite.transform.localPosition.Set(0, 0, 0.2f);
		headSprite.name = "Head Sprite";

		actionSprites = new SpriteControler[MAX_ACTIONS_ON_THIS_BLOCK];
		actionUsed = new bool[MAX_ACTIONS_ON_THIS_BLOCK];
		for(int x = 0; x < MAX_ACTIONS_ON_THIS_BLOCK; x++) {
			actionSprites[x] = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
			actionSprites[x].transform.SetParent(transform);
			actionSprites[x].transform.localPosition.Set(0, 0, 0.3f);
			actionSprites[x].name = "Action Sprite";
		}

		rightConection = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
		rightConection.transform.SetParent(transform);
		rightConection.transform.localPosition.Set(0, conectionLocation, 0.1f);
		rightConection.name = "Right Conection";

		aboveConection = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
		aboveConection.transform.SetParent(transform);
		aboveConection.transform.localPosition.Set(conectionLocation, 0, 0.1f);
		aboveConection.name = "Above Conection";
	}

	#endregion

	#region check conections

	/// <summary>
	/// Checks the conections of the unit.
	/// This should be called by every segment of a unit where that unit was
	/// before a change and by any new unit segments
	/// </summary>
	public void checkConection() {
		if(attachedGridBlock.programInstalled == null) {
			Debug.LogWarning("this block has no program to check conections on");
			return;
		}

		if(attachedGridBlock.right != null)
		checkRight();
		if(attachedGridBlock.up != null)
		checkAbove();
		if(attachedGridBlock.left != null)
		attachedGridBlock.left.spriteDisplayScript.checkRight(); // check left
		if(attachedGridBlock.down != null)
		attachedGridBlock.down.spriteDisplayScript.checkAbove(); // check right
	}


	/// <summary>
	/// Checks the right.
	/// only to be called by another GridBlockSpriteDisplay script
	/// </summary>
	public void checkRight() {
		if(attachedGridBlock.programInstalled == attachedGridBlock.right.programInstalled) {
			rightConection.setSprite(spriteInfo.sprite_unitConecter, unitSprite.getColor());
		} else {
			rightConection.removeSprite();
		}
	}

	/// <summary>
	/// Checks the above.
	///  only to be called by another GridBlockSpriteDisplay script
	/// </summary>
	public void checkAbove() {
		if(attachedGridBlock.programInstalled == attachedGridBlock.up.programInstalled) {
			aboveConection.setSprite(spriteInfo.sprite_unitConecter, unitSprite.getColor());
		} else {
			aboveConection.removeSprite();
		}
	}

	#endregion

	#region check unit sprite and head

	//All things that need to be diplayed will have there own function

	/// <summary>
	/// Display a sprite at a specified distanceAbove this grid space.
	/// The unit information must be setup on the grid block before this is called
	/// If no unit is found the units sprite and the head sprite will be removed from this location
	/// </summary>
	public void checkUnitSprite() {
		if(attachedGridBlock.programInstalled == null) {
			unitSprite.removeSprite();
			headSprite.removeSprite();
		} else {
			unitSprite.setSprite(spriteInfo.sprite_unit, attachedGridBlock.programInstalled.getUnitColor());
		}
	}

	/// <summary>
	/// displays head if this this the correct head location.
	/// removes head sprite if it is not.
	/// </summary>
	public void checkHeadSprite() {
		if(attachedGridBlock.programHead) {
			headSprite.setSprite(attachedGridBlock.programInstalled.getUnitHeadSprite());
		} else {
			headSprite.removeSprite();
		}
	}


	/// <summary>
	/// displays the action.
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



	// Update is called once per frame
	void Update() {
	
	}
}
