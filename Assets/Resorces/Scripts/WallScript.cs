using UnityEngine;
using System.Collections;

public class WallScript : UnitScript{
	[SerializeField] private int health;
	public bool canDestroy;

	/// <summary>
	/// Decrements health if wall is destroyable.</summary>
	/// <returns>Whether the wall was destroyed.</returns>
	/// <param name="damageAmount">Damage amount.</param>
	public override bool removeBlock(int damageAmount){
		if(canDestroy){
			health -= damageAmount;
			if (health <= 0) {
				destroyUnit ();
			}
			return true;
		}
		return false;
	}

	// Use this for initialization.
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start(){
	
	}
	
	// Update is called once per frame.
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update(){
	
	}
}
