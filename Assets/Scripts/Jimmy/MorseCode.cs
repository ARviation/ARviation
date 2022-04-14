public enum MorseCode : int
{
  A = 0, // Fuselage
  K = 1, // Wrong - Women 
  H = 2, // Packard
  G = 3, // Wrong - Designer
  Z = 4, // Tapered
  N = 5, // Wrong - Doctor
  F = 6, // Four
  O = 7, // Landing
  U = 8, // Tail
  E = 9, // Fuel
}







public enum CorrectMorseCode : int
{
  Fuselage = MorseCode.A,
  Engine = MorseCode.H,
  Propeller = MorseCode.F,
  Wings = MorseCode.Z,
  Wheels = MorseCode.O,
  FuelTank = MorseCode.E,
  Tail = MorseCode.U,
}