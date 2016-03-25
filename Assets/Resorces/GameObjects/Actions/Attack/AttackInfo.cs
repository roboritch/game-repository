using UnityEngine;
using System.Collections;

public class AttackInfo : ActionButtonInfo{
	public override ActionScript getNewInstanceOfAction(UnitScript unit){
		return new AttackScript(unit);
	}
}
