using UnityEngine;
using System.Collections;

public class MoveInfo : ActionButtonInfo{
	public override ActionScript getNewInstanceOfAction (){
		return new MoveScript();
	}
}
