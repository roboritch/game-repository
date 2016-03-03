using UnityEngine;
using System.Collections;

public class WallScript : UnitScript {
	[SerializeField] private int health;
	public bool canDestroy;
	public override void receiveDamage (int damageAmount)
	{
		if (canDestroy && health > damageAmount) {
			health = health - damageAmount;
		}
		if (canDestroy && health <= damageAmount) {
			destroyUnit();
		}
			
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
