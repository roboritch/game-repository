﻿using UnityEngine;
using System.Collections;

public class WallScript : UnitScript{
  [SerializeField] private int health;
  public bool canDestroy;

  /// <summary>
  /// Decrements health if wall is destroyable.</summary>
  /// <returns>Whether the wall was destroyed.</returns>
  /// <param name="damageAmount">Damage amount.</param>
  public override bool removeBlock(int damageAmount){
    if (canDestroy){
      if (damageAmount < health){
        health -= damageAmount;
      } else{
        destroyUnit();
        return true;
      }
    }
    return false;
  }

  // Use this for initialization
  void Start(){
	
  }
	
  // Update is called once per frame
  void Update(){
	
  }
}
