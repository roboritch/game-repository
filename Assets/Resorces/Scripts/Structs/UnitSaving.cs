using System;

public struct UnitSaving{
  public int currentMaxLength;
  public int currentMaxMove;
  public int currentAttackPow;
  public int currentMaxAttackActions;
  //Be carful when syncing time with network
  public UnitTimer currentUnitTimer;
}