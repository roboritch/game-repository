using System;

/// <summary>
/// Each action queue must be sent to the other players using serealizable information.
/// Each action must save and load their information using this struct.
/// The unit performing the action must be given to it when constructed.
/// </summary>
[System.Serializable]
public struct SerializedCompletedAction{
	public GridLocation locationToPerformAction;
	public int actionAmountInt;
	public System.Type actionType;
}