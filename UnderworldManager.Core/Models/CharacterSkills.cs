using UnderworldManager.Core.Models;

namespace UnderworldManager.Core.Models
{
  public class CharacterSkills
  {
    public Skill Type { get; set; }
    public int Increases { get; set; }

    public CharacterSkills(Skill type, int baseTraining = 0)
    {
      Type = type;
      Increases = baseTraining;
    }

    public void Train(int value)
    {
      Increases += value;
    }

    internal static List<Skill> ListAllSKills()
    {
      return new List<Skill>()
      {
        Skill.MeleeCombat,
        Skill.Athletics,
        Skill.Intimidation,
        Skill.Streetwise,
        Skill.Perception,
        Skill.Medicine,
        Skill.Ambition,
        Skill.Scheming,
        Skill.Forgery,
        Skill.Alchemy,
        Skill.RangedCombat,
        Skill.SleightOfHand,
        Skill.Stealth,
        Skill.Lockpicking,
        Skill.Acrobatics,
        Skill.Etiquette,
        Skill.Charm,
        Skill.Leadership,
        Skill.Mercantile,
        Skill.Looks,
        Skill.Loyalty,
        Skill.Deception
      };      
    }

    public static Dictionary<CoreAttribute, List<Skill>> ListAllSKillsByCoreAttribute()
    {
      var sKillsByCoreAttribute = new Dictionary<CoreAttribute, List<Skill>>
      {
        { CoreAttribute.Strength, GetSkillsByAttribute(CoreAttribute.Strength) },
        { CoreAttribute.Charisma, GetSkillsByAttribute(CoreAttribute.Charisma) },
        { CoreAttribute.Agility, GetSkillsByAttribute(CoreAttribute.Agility) },
        { CoreAttribute.Intelligence, GetSkillsByAttribute(CoreAttribute.Intelligence) }
      };

      return sKillsByCoreAttribute;
    }
    public static List<Skill> GetSkillsByAttribute(CoreAttribute attribute)
    {
      List<Skill> skills = new List<Skill>();
      switch (attribute)
      {
        case CoreAttribute.Strength:
          skills.Add(Skill.MeleeCombat);
          skills.Add(Skill.Athletics);
          skills.Add(Skill.Intimidation);
          break;
        case CoreAttribute.Intelligence:
          skills.Add(Skill.Streetwise);
          skills.Add(Skill.Perception);
          skills.Add(Skill.Medicine);
          skills.Add(Skill.Ambition);
          skills.Add(Skill.Scheming);
          skills.Add(Skill.Forgery);
          skills.Add(Skill.Alchemy);
          break;
        case CoreAttribute.Agility:
          skills.Add(Skill.RangedCombat);
          skills.Add(Skill.SleightOfHand);
          skills.Add(Skill.Stealth);
          skills.Add(Skill.Lockpicking);
          skills.Add(Skill.Acrobatics);
          break;
        case CoreAttribute.Charisma:
          skills.Add(Skill.Etiquette);
          skills.Add(Skill.Charm);
          skills.Add(Skill.Leadership);
          skills.Add(Skill.Mercantile);
          skills.Add(Skill.Looks);
          skills.Add(Skill.Loyalty);
          skills.Add(Skill.Deception);
          break;
      }
      return skills;
    }
  }
}