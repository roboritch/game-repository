using System;
using UnityEngine;

/// <summary>
/// Grid location.
/// implements ==, != and = operations
/// </summary>
#pragma warning disable
[System.Serializable]
public struct GridLocation{
  /// <summary>X coordinate of grid location.</summary>
  public int x;
  /// <summary>Y coordinate of grid location.</summary>
  public int y;

  public GridLocation(int xl, int yl){
    x = xl;
    y = yl;
  }

  public static GridLocation operator +(GridLocation a, GridLocation b){
    return new GridLocation(a.x + b.x, a.y + b.y);
  }

  public static GridLocation operator -(GridLocation a, GridLocation b){
    return new GridLocation(a.x - b.x, a.y - b.y);
  }

  public static bool operator ==(GridLocation a, GridLocation b){
    return a.x == b.x && a.y == b.y;
  }

  public static bool operator !=(GridLocation a, GridLocation b){
    return !(a == b);
  }

}