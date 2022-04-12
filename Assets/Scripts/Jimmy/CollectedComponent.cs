using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedComponent : MonoBehaviour
{
  public int Fuselage { get; set; } = (int) CorrectMorseCode.Fuselage;
  public int Engine { get; set; } = (int) CorrectMorseCode.Engine;
  public int Wings { get; set; } = (int) CorrectMorseCode.Wings;
  public int Propellers { get; set; } = (int) CorrectMorseCode.Propeller;
  public int FuelTank { get; set; } = (int) CorrectMorseCode.FuelTank;
  public int Wheels { get; set; } = (int) CorrectMorseCode.Wheels;
  public int Tail { get; set; } = (int) CorrectMorseCode.Tail;

  // public int Fuselage { get; set; } = -1;
  // public int Engine { get; set; } = -1;
  // public int Wings { get; set; } = -1;
  // public int Propellers { get; set; } = -1;
  // public int FuelTank { get; set; } = -1;
  // public int Wheels { get; set; } = -1;
  // public int Tail { get; set; } = -1;

  public Dictionary<string, int> GetAllComponent()
  {
    var result = new Dictionary<string, int>
    {
      {GameManager.Fuselage, Fuselage},
      {GameManager.Engine, Engine},
      {GameManager.Wings, Wings},
      {GameManager.Propeller, Propellers},
      {GameManager.FuelTank, FuelTank},
      {GameManager.Wheels, Wheels},
      {GameManager.Tail, Tail}
    };
    return result;
  }

  public override string ToString()
  {
    return $"{Fuselage} {Engine} {Wings} {Propellers} {Wheels} {FuelTank} {Tail}";
  }
}