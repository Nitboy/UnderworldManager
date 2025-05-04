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
    public static CoreAttribute GetCoreAttribute(Skill skill)
    {
      CoreAttribute attribute = CoreAttribute.Strength;
      switch (skill)
      {
        case Skill.MeleeCombat:
        case Skill.Athletics:
        case Skill.Intimidation:
          attribute = CoreAttribute.Strength;
          break;
        case Skill.Streetwise:
        case Skill.Perception:
        case Skill.Medicine:
        case Skill.Ambition:
        case Skill.Scheming:
        case Skill.Forgery:
        case Skill.Alchemy:
          attribute = CoreAttribute.Intelligence;
          break;
        case Skill.RangedCombat:
        case Skill.SleightOfHand:
        case Skill.Stealth:
        case Skill.Lockpicking:
        case Skill.Acrobatics:
          attribute = CoreAttribute.Agility;
          break;
        case Skill.Etiquette:
        case Skill.Charm:
        case Skill.Leadership:
        case Skill.Mercantile:
        case Skill.Looks:
        case Skill.Loyalty:
        case Skill.Deception:
          attribute = CoreAttribute.Charisma;
          break;
      }
      return attribute;
    }
  }
}
