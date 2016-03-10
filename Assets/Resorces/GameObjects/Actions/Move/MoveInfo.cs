using UnityEngine;
using System.Collections;

public class MoveInfo : ActionButtonInfo{
	public override ActionScript getNewInstanceOfAction (UnitScript unit){
		ActionScript temp = new MoveScript ();
		temp.setUnit (unit);
		return temp;
	}
}
