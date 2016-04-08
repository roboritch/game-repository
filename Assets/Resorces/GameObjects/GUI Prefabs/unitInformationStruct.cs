using UnityEngine;
using System.Collections;

[System.Serializable]
public struct UnitInformationStruct{
	public string unitNameForLoad;
	public GameObject unit;
	public Sprite unitHeadSprite;
	public Color unitColor;
	public int maxLength;
	public int maxMove;
	public int attackPow;
	public int maxAttackActions;
	public UnitTimer unitTimer;
}
