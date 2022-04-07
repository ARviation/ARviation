public enum MorseCode : int
{
  A = 0,
  V = 1,
  L = 2,
  H = 3,
  M = 4,
  W = 5,
  T = 6,
  F = 7,
  O = 8,
  E = 9,
  U = 10,
  S = 11,
  P = 12,
  R = 13,
}

public enum CorrectMorseCode : int
{
  Fuselage = MorseCode.A,
  Engine = MorseCode.V,
  Propeller = MorseCode.F,
  Wings = MorseCode.W,
  Wheels = MorseCode.O,
  FuelTank = MorseCode.E,
  Tail = MorseCode.U,
}