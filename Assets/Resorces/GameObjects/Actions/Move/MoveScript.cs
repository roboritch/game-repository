using UnityEngine;
using System.Collections;

//TODO Move Action
public class MoveScript : ActionScript {

	// Use this for initialization
	void Start() {

	}
	
	// Update is called once per frame
	void Update() {
	
	}
	public override void act() {
		
		if (unit.blockList.Count < unit.MaxProgramLength) {
			unit.blockList.AddFirst ();

		}
		else {
			unit.blockList.RemoveLast();
			unit.blockList.AddFirst();
			//a.spriteDisplayScript.updateUnitSprite;
		}
	}
		
}
