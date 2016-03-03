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

  /// <summary> Perform this action when called by the unit's action list. </summary>
  public abstract void act();

  /// <summary>The amount of time it takes to animate the unit preforming this action.</summary>
  /// <returns>The time.</returns>
  public virtual float actionTime(){
    return 1.0f;
  }

  /// <summary>Calls the GUI to display this action on the game. </summary>
  public abstract void display(GUIScript gui);

  /// <summary>Calls this to get the GUI to remove all displayed images of this action.</summary>
  public abstract void removeDisplay(GUIScript gui);
}
