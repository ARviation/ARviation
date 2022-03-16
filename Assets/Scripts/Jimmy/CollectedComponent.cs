using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedComponent : MonoBehaviour
{
  public int body { get; set; } = -1;
  public int engine { get; set; } = -1;
  public int wings { get; set; } = -1;
  public int propellers { get; set; } = -1;
  public int oilTank { get; set; } = -1;
  public int wheels { get; set; } = -1;
  public int extra { get; set; } = -1;

  public override string ToString()
  {
    return $"{body} {engine} {wings} {propellers} {oilTank} {wheels} {extra}";
  }
}