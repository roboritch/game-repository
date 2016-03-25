using UnityEngine;
using System.Collections;

public class MoveInfo : ActionButtonInfo{
	public override ActionScript getNewInstanceOfAction(UnitScript unit){
		return new MoveScript(unit);
	}



}
