using System;

[System.Serializable]
public struct UnitSaving{
	public ControlType controlType;

	public int currentMaxLength;
	public int currentMaxMove;
	public int currentAttackPow;
	public int currentMaxAttackActions;

	//Be carful when syncing time with network
	public UnitTimer currentUnitTimer;
}


public enum ControlType{
	AI,
	PLAYER,
	NETWORK,
}