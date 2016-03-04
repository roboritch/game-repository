using UnityEngine;
using System.Collections;

//TODO Move Action
public class MoveScript : ActionScript {
	public Direction moveDir;
	// Use this for initialization
	void Start() {

	}
	
	// Update is called once per frame
	void Update() {
	
	}
	public override void act() {
		
		if (unit.getLength() < unit.MaxProgramLength) {
			unit.addBlock (unit.getBlockHeadLocation().getAdj(moveDir));

		}
		else {
			unit.removeBlock();
			unit.addBlock(unit.getBlockHeadLocation().getAdj(moveDir));

		}
	}

	public override void display() {
	}

	public override void removeDisplay() {
	}

}
