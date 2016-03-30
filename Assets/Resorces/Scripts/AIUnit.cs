using System;

public class AIUnit: UnitScript {

	//Attributes of this AI, for decision making.

	/// <summary>
	/// The offense/defense bias of this AI, in health percentage.
	/// 0 = defense behavior, 1 = offense behavior.
	/// </summary>
	double aggressiveness;
	/// <summary>
	/// The micro/macro bias of this AI, in percentage.
	/// 0 = micro behavior, 1 = macro behavior.
	/// </summary>
	double gameRole;

	//Current state attributes of this AI.

	/// <summary>
	/// The current move behavior of this unit.
	/// </summary>
	MoveDirBehavior moveDirB;
	/// <summary>
	/// The current move scope behavior of this unit.
	/// </summary>
	MoveScopeBehavior moveScopeB;
	/// <summary>
	/// The current target behavior of this unit.
	/// </summary>
	MoveTargetBehavior targetB;
	/// <summary>
	/// The current attack behavior of this unit.
	/// </summary>
	AttackBehavior attackB;

	public AIUnit() {
		aggressiveness = 0.5;
		gameRole = 0.5;
		//Default unit behaviour to idle upon creation.
		moveDirB = MoveDirBehavior.IDLE;
		moveScopeB = MoveScopeBehavior.MICRO;
		targetB = MoveTargetBehavior.HEAD;
		attackB = AttackBehavior.IDLE;
	}

	public override void startActing() {

		base.startActing();
	}

	/// <summary>
	/// Descriptive code of this AI unit. Follows the format:
	/// "{unitDescription}M:{moveBehavior},A:{attackBehavior}"
	/// </summary>
	/// <returns>The code string.</returns>
	public override string toString() {
		return "M:" + moveDirB + "," + moveScopeB + ",A:" + attackB;
	}

}
