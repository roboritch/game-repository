using UnityEngine;
using System.Collections;

/// <summary>
/// Action button info.
/// All new actions must create their own child of this script.
/// </summary>
public abstract class ActionButtonInfo : MonoBehaviour{
  [SerializeField] protected string actionName;
  [SerializeField] protected TextAsset discription;

  /// <summary>
  /// Gets the new instance of action.
  /// The action type must be set through a new child of this object.
  /// </summary>
  /// <returns>The new instance of action.</returns>
  public abstract ActionScript getNewInstanceOfAction(UnitScript unit);

  public string getDescriptionText(){
    return discription.text;
  }

}
