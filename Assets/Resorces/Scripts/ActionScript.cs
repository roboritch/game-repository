using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

/// <summary>
/// Action script.
/// Each unit has a set of actions. These actions can be placed into a list 
/// and executed in sequence with other actions.
/// </summary>
public abstract class ActionScript{
  /// <summary>The unit performing this action. </summary>
  protected UnitScript unit;
  protected float actionTime;
  /// <summary>Some actions may require more than one action Id. </summary>
  private int actionID;
  //Set for each display of the action
  //each move. Attack or action must be a seprate instance of this script.

  public int ActionID {
    get {
      return actionID;
    }
    set {
      actionID = value;
    }
  }


  protected ActionScript(UnitScript u){
    unit = u;
  }



  /// <summary> Perform this action when called by the unit's action list. </summary>
  public abstract void act();

  /// <summary>The amount of time it takes to animate the unit preforming this action.</summary>
  /// <returns>The time.</returns>
  public virtual float getActionTime(){
    return actionTime;
  }

  /// <summary>Calls the GUI to display this action's posible outputs on the game. </summary>
  public abstract void displayUserSelection();

  /// <summary> Displaies the finished action that the user has chosen. </summary>
  public abstract void displayFinishedAction();

  /// <summary>Calls this to get the GUI to remove displayed images during reset or when action is compleated.</summary>
  public abstract void removeActionRepresentationDisplay();

  /// <summary> Removes the user selection display this can be called when the action is selected or when it is canceld. </summary>
  public abstract void removeUserSelectionDisplay();

  /// <summary>
  /// This is called when a user has selected their action location.
  /// </summary>
  /// <param name="blockSelected">Block selected.</param>
  public abstract void userSelectedAction(GridBlock blockSelected);

  public abstract SerializedCompletedAction serializeAction();

  public abstract void loadAction(SerializedCompletedAction s);

}