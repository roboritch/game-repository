using UnityEngine;
using System.Collections;

/*TODO Add display for selection operations, currently this script is
setup to display all unit actions after they are created.
More will need to be done to display the options for the currently
selected unit.*/
using System;


/// <summary>Grid block sprite display.</summary>
public class GridBlockSpriteDisplay : MonoBehaviour{

  #region basic vars

  /// <summary>The info of this sprite.</summary>
  private CreatePlayGrid spriteInfo;
  /*Using gameObject.getComponent has a larger overhead than a direct refrence. */
  /// <summary>The grid block of this sprite.</summary>
  private GridBlock attachedGridBlock;
  /// <summary>The connection location of this sprite.</summary>
  private float conectionLocation = 1f;
  //TODO Hard code this, this may nead to be changed.
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
  private SpriteControler aboveConnection;
  /// <summary>The right sprite controller connection.</summary>
  private SpriteControler rightConnection;


  #endregion

  //Movement display attributes.
  //Vars are complex read descriptions for more info.
  public SpriteControler[] movementDirections;
  /// <summary>
  /// Unit movment overlap.
  /// The number of units ready to move over this block.
  /// </summary>
  private int[] UMO = { 0, 0, 0, 0 };
  private SpriteControler movementCircle;

  #region script startup

  // All sprite layers are created on load to minimize the impact of changing sprites
  /// <summary>Start this instance.</summary>
  void Start(){ 
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
    for (int x = 0; x < MAX_ACTIONS_ON_THIS_BLOCK; x++){
      actionSprites[x] = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
      actionSprites[x].transform.SetParent(transform);
      actionSprites[x].transform.localPosition = new Vector3(0, 0, 0.3f + x * 0.05f);
      actionSprites[x].name = "Action Sprite " + x;
    }

    //Initialize movement directions.
    movementDirections = new SpriteControler[4];
    string[] directionNames = { "up", "right", "down", "left" };
    for (int x = 0; x < 4; x++){
      movementDirections[x] = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
      movementDirections[x].transform.SetParent(transform);
      movementDirections[x].transform.localPosition = new Vector3(0, 0, 2.5f);

      //Rotating each movement arm.
      Quaternion rot = movementDirections[x].transform.localRotation;
      rot.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f * x); //UNDONE check to see if rotation is correct
      movementDirections[x].transform.localRotation = rot;

      movementDirections[x].name = "Movment Direction " + directionNames[x];
      // Sprites for this controller are always the same.
      movementDirections[x].setSprite(spriteInfo.spritesAndColors.sprite_moveLine);
      //The color for this sprite is Alpha only.
      movementDirections[x].setColor(Color.clear);
    }

    //Initialize movement circle.
    movementCircle = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
    movementCircle.transform.SetParent(transform);
    movementCircle.transform.localPosition = new Vector3(0, 0, 2.5f);
    movementCircle.name = "Movement Circle";

    //Initialize right connection.
    rightConnection = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
    rightConnection.transform.SetParent(transform);
    rightConnection.transform.localPosition = new Vector3(conectionLocation, 0, 0.1f);
    rightConnection.name = "Right Conection";

    //Initialize above connection.
    aboveConnection = Instantiate(spriteInfo.spritePrefab).GetComponent<SpriteControler>();
    aboveConnection.transform.SetParent(transform);
    aboveConnection.transform.localPosition = new Vector3(0, -conectionLocation, 0.1f);
    aboveConnection.transform.Rotate(0f, 0f, 90f);
    aboveConnection.name = "Above Conection";

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
  public void checkConection(){
    if (attachedGridBlock.unitInstalled == null){
      return;
    }

    if (attachedGridBlock.getAdj(Direction.RIGHT) != null)
      checkRight();
    if (attachedGridBlock.getAdj(Direction.UP) != null)
      checkAbove();
    if (attachedGridBlock.getAdj(Direction.LEFT) != null)
      attachedGridBlock.getAdj(Direction.LEFT).spriteDisplayScript.checkRight(); // check left
    if (attachedGridBlock.getAdj(Direction.DOWN) != null)
      attachedGridBlock.getAdj(Direction.DOWN).spriteDisplayScript.checkAbove(); // check down
  }


  /// <summary>
  /// Checks the right sprite.
  /// Only to be called by another GridBlockSpriteDisplay script.
  /// </summary>
  public void checkRight(){
    if (attachedGridBlock.unitInstalled == attachedGridBlock.getAdj(Direction.RIGHT).unitInstalled){
      rightConnection.setSprite(spriteInfo.spritesAndColors.sprite_unitConecter, unitSprite.getColor());
    } else{
      rightConnection.removeSprite();
    }
  }

  /// <summary>
  /// Checks the above sprite.
  /// Only to be called by another GridBlockSpriteDisplay script.
  /// </summary>
  public void checkAbove(){
    if (attachedGridBlock.unitInstalled == attachedGridBlock.getAdj(Direction.UP).unitInstalled){
      aboveConnection.setSprite(spriteInfo.spritesAndColors.sprite_unitConecter, unitSprite.getColor());
    } else{
      aboveConnection.removeSprite();
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
  public void updateUnitSprite(){
    if (attachedGridBlock.unitInstalled == null){
      unitSprite.removeSprite();
      headSprite.removeSprite();
    } else{
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
  public void checkHeadSprite(){
    if (attachedGridBlock.unitInstalled.getCurrentBlockHeadLocation().gridlocation == attachedGridBlock.gridlocation){
      headSprite.setSprite(attachedGridBlock.unitInstalled.getUnitHeadSprite());
    } else{
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
  public int displayAction(Sprite aS){
    int actionID = 0;
    while (actionUsed[actionID]){
      actionID++;
      if (actionID > actionUsed.Length){
        Debug.LogError("too meny actions on this block");
        return -1;
      }
    }
    actionSprites[actionID].setSprite(aS);
    actionUsed[actionID] = true;
    return actionID;
  }

  /// <summary>
  /// Removes the action.
  /// will not act if actionID < 0 || actionID > MAX_ACTIONS_ON_THIS_BLOCK
  /// </summary>
  /// <param name="actionID">ActionID.</param>
  public void removeAction(int actionID){
    if (actionID < 0 || actionID > MAX_ACTIONS_ON_THIS_BLOCK){
      return;
    }
    actionSprites[actionID].removeSprite();
    actionUsed[actionID] = false;
  }

  #endregion

  #region movment display

  //TODO work on unit overlap
  /// <summary>
  /// Display unit's projected movement through this block.
  /// </summary>
  /// <param name="cameFrom">Block this unit is coming from.</param>
  /// <param name="isGoingTo">Block this unit is going to.</param>
  ///<returns> A refrence to the spriteControlers for moveArmIn (0) and moveArmOut (1)
  /// when the action is preformed set there color to Color.Clear</returns>
  public SpriteControler[] displayMoveOnQueue(GridBlock cameFrom, GridBlock isGoingTo){

    SpriteControler moveArmIn = null;
    SpriteControler moveArmOut = null;

    for (int i = 0; i < 4; i++){
      if (isGoingTo.getAdj(i) == cameFrom){
        moveArmOut = cameFrom.spriteDisplayScript.movementDirections[i];
        // Faster than mod 4.
        i += 2;
        if (i > 3){
          i -= 4;
        }
        moveArmIn = isGoingTo.spriteDisplayScript.movementDirections[i];
        break;
      }
    }
    //TODO Make move arm in add out visible using color.
    if (moveArmIn == null || moveArmOut == null){
      Debug.LogError("cameFrom and isGoingTo were not adjacent!\n" +
      "null was returned to the action");
      return null;
    }
    moveArmIn.setColor(Color.white);
    moveArmOut.setColor(Color.white);

    SpriteControler[] sc = { moveArmIn, moveArmOut };

    return sc;
  }

  /// <summary>
  /// UMOs color converter.
  /// not currently used
  /// </summary>
  /// <returns>The o color converter.</returns>
  /// <param name="numberOverlap">Number overlap.</param>
  private Color UMO_ColorConverter(int numberOverlap){
    switch (numberOverlap){
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
  public void displayMoveSprite(){
    moveSprite.setSprite(spriteInfo.spritesAndColors.sprite_moveTo);
  }

  /// <summary>
  /// Removes the move sprite from this display.
  /// </summary>
  public void removeMoveSprite(){
    moveSprite.removeSprite();
  }

  #endregion

  // Update is called once per frame.
  /// <summary> Update this instance.</summary>
  void Update(){
  }
}
