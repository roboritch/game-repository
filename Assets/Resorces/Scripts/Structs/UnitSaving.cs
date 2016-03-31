using System;

[System.Serializable]
public struct UnitSaving{
	public string unitNameToLoad;

	public int currentMaxLength;
	public int currentMaxMove;
	public int currentAttackPow;
	public int currentMaxAttackActions;

	public GridLocation[] currentBlockLocations;

	//Be carful when syncing time with network
	public UnitTimer currentUnitTimer;
}
	