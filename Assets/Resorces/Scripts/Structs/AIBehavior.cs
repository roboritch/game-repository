using System;

/// <summary>
/// Movement behavior of AI unit.
/// </summary>
public enum MoveDirBehavior {
	/// <summary>
	/// Don't move.
	/// </summary>
	IDLE,
	/// <summary>
	/// Move towards the target location.
	/// </summary>
	TOWARD,
	/// <summary>
	/// Move away from the target location.
	/// </summary>
	AWAY
}

/// <summary>
/// Movement scope behavior of AI unit.
/// </summary>
public enum MoveScopeBehavior {
	/// <summary>
	/// Move relative to the closest target.
	/// </summary>
	MICRO,
	/// <summary>
	/// Move relative to the local densest area of targets.
	/// </summary>
	MACRO,
	/// <summary>
	/// Move relative to the global densest area of targets.
	/// </summary>
	GLOBAL
}

/// <summary>
/// Target behavior of AI unit.
/// </summary>
public enum MoveTargetBehavior {
	/// <summary>
	/// Move relative to target unit heads.
	/// </summary>
	HEAD,
	/// <summary>
	/// Move relative to whole target units.
	/// </summary>
	BODY
}

/// <summary>
/// Attack behavior of AI unit.
/// </summary>
public enum AttackBehavior {
	/// <summary>
	/// Attack one enemy as many times as possible.
	/// </summary>
	MICRO,
	/// <summary>
	/// Attack as many enemies as possible.
	/// </summary>
	MACRO
}