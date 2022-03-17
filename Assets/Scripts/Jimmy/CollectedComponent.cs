using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedComponent : MonoBehaviour
{
  public int Body { get; set; } = -1;
  public int Engine { get; set; } = -1;
  public int Wings { get; set; } = -1;
  public int Propellers { get; set; } = -1;
  public int OilTank { get; set; } = -1;
  public int Wheels { get; set; } = -1;
  public int Extra { get; set; } = -1;

  public Dictionary<string, int> GetAllComponent()
  {
    var result = new Dictionary<string, int>
    {
      {"Body", Body},
      {"Engines", Engine},
      {"Wings", Wings},
      {"Propellers", Propellers},
      {"OilTank", OilTank},
      {"Wheels", Wheels},
      {"Extra", Extra}
    };
    return result;
  }

  public override string ToString()
  {
    return $"{Body} {Engine} {Wings} {Propellers} {OilTank} {Wheels} {Extra}";
  }
}