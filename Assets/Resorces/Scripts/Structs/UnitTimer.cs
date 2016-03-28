using System;

[System.Serializable]
public struct UnitTimer{
	public float time;
	public float maxTime;
	public float ticAmount;

	public static bool operator ==(UnitTimer a, UnitTimer b){
		return a.maxTime == 0;
	}

	public static bool operator !=(UnitTimer a, UnitTimer b){
		return a.maxTime != 0;
	}
}