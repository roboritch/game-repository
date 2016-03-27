using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable
/// <summary>
/// Unit holder contains all the units set in this objects inspector.
/// </summary>
public class UnitHolder : Singleton<UnitHolder>{
	protected UnitHolder(){
	}

	[SerializeField] private UnitPrefabAndName[] units;
	private Dictionary<string,GameObject> unitLibrary = new Dictionary<string, GameObject>(10);

	void Start(){
		for(int i = 0; i < units.Length; i++){
			unitLibrary.Add(units[i].unitName, units[i].unitPrefab);
		}

	}

	public GameObject getUnitFromName(string name){ 
		GameObject un;
		unitLibrary.TryGetValue(name, out un);
		return un;
	}


}


[System.Serializable]
public struct UnitPrefabAndName{
	public string unitName;
	public GameObject unitPrefab;
}