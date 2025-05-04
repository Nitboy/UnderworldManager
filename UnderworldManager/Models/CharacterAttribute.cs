namespace UnderworldManager.Models
{
  public class CharacterAttribute
  {
    public CoreAttribute Type { get; set; }
    public int BaseValue { get; set; }
    public int Increases { get; set; }

    public CharacterAttribute(CoreAttribute type, int baseValue)
    {
      Type = type;
      BaseValue = baseValue;
      Increases = 0;
    }

    public int Total
    {
      get
      {
        return BaseValue + Increases;
      }
    }

    public void Train(int value)
    {
      Increases += value;
    }
  }
}
