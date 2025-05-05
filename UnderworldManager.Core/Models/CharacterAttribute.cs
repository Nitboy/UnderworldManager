using System;
using UnderworldManager.Core.Models;

namespace UnderworldManager.Core.Models
{
  public class CharacterAttribute
  {
    public CoreAttribute Type { get; set; }
    public int Value { get; set; }
    public int Total => Value + Increases;
    public int Increases { get; private set; }
    public string Name => Type.ToString();

    public CharacterAttribute(CoreAttribute type, int value)
    {
      Type = type;
      Value = value;
      Increases = 0;
    }

    public void Train(int value)
    {
      Increases += value;
    }
  }
}
