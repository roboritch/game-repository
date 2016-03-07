using UnityEngine;
using System.Collections;

/// <summary>
/// Action button info.
/// All new actions must create their own child of this script.
/// </summary>
public class ActionButtonInfo : MonoBehaviour{
	public string actionName;
	public TextAsset discription;
	/// <summary>
	/// Gets the new instance of action.
	/// The action type must be set through a new child of this object.
	/// </summary>
	/// <returns>The new instance of action.</returns>
	public virtual ActionScript getNewInstanceOfAction(){
		return null;
	}

	public string descriptionText(){
		return discription.text;
	}

}
