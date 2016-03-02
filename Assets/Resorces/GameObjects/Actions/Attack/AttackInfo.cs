using UnityEngine;
using System.Collections;

public class AttackInfo : ActionButtonInfo{
  public override ActionScript getNewInstanceOfAction (){
    return new MoveScript();
  }
}
