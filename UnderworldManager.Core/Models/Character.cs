using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnderworldManager.Core.Models;
using UnderworldManager.Core.Business;

namespace UnderworldManager.Core.Models
{
  public enum Rank1Roles
  {
    Enforcer,
    Muscle,
    Hitman,
    Bouncer,
    Mastermind,
    Forger,
    ConArtist,
    Scribe,
    Pickpocket,
    Burglar,
    Thief,
    EscapeArtist,
    SmoothTalker,
    Negotiator,
    Prostitute
  }

  public class Rank1RoleMapping
  {
    public static Dictionary<CoreAttribute, List<Rank1Roles>> RoleList { get; set; }

    static Rank1RoleMapping()
    {
      RoleList = new Dictionary<CoreAttribute, List<Rank1Roles>>
        {
            { CoreAttribute.Strength, new List<Rank1Roles> { Rank1Roles.Enforcer, Rank1Roles.Muscle, Rank1Roles.Hitman, Rank1Roles.Bouncer } },
            { CoreAttribute.Intelligence, new List<Rank1Roles> { Rank1Roles.Mastermind, Rank1Roles.Forger, Rank1Roles.ConArtist, Rank1Roles.Scribe } },
            { CoreAttribute.Agility, new List<Rank1Roles> { Rank1Roles.Pickpocket, Rank1Roles.Burglar, Rank1Roles.Thief, Rank1Roles.EscapeArtist } },
            { CoreAttribute.Charisma, new List<Rank1Roles> { Rank1Roles.SmoothTalker, Rank1Roles.Negotiator, Rank1Roles.Prostitute } }
        };
    }

    public static List<Rank1Roles> GetRolesForAttribute(CoreAttribute attribute)
    {
      return RoleList[attribute];
    }
  }

  public enum Rank2Roles
  {
    Quartermaster,
    Mercenaries_leader,
    Bodyguard,
    Forger,
    Spy,
    Burglar,
    Escort,
    Scout,
    Enforcer,
    Bouncer,
    Politician,
    Prostitute,
    Pickpocket,
    Con_Artist,
    Entertainer,
    Negotiator,
    Assassin,
    Grappler,
    Smuggler,
    Acrobat,
    Mastermind,
    Diplomat
  }

  public class Rank2RoleMapping
  {
    public static Dictionary<Tuple<CoreAttribute, CoreAttribute>, List<Rank2Roles>> Roles { get; set; }

    static Rank2RoleMapping()
    {
      Roles = new Dictionary<Tuple<CoreAttribute, CoreAttribute>, List<Rank2Roles>>
        {
            { new Tuple<CoreAttribute, CoreAttribute>(CoreAttribute.Strength, CoreAttribute.Intelligence), new List<Rank2Roles> { Rank2Roles.Quartermaster, Rank2Roles.Mercenaries_leader, Rank2Roles.Bodyguard, Rank2Roles.Forger } },
            { new Tuple<CoreAttribute, CoreAttribute>(CoreAttribute.Intelligence, CoreAttribute.Strength), new List<Rank2Roles> { Rank2Roles.Quartermaster, Rank2Roles.Mercenaries_leader, Rank2Roles.Bodyguard, Rank2Roles.Forger } },
            { new Tuple<CoreAttribute, CoreAttribute>(CoreAttribute.Intelligence, CoreAttribute.Agility), new List<Rank2Roles> { Rank2Roles.Spy, Rank2Roles.Burglar, Rank2Roles.Escort, Rank2Roles.Scout } },
            { new Tuple<CoreAttribute, CoreAttribute>(CoreAttribute.Agility, CoreAttribute.Intelligence), new List<Rank2Roles> { Rank2Roles.Spy, Rank2Roles.Burglar, Rank2Roles.Escort, Rank2Roles.Scout } },
            { new Tuple<CoreAttribute, CoreAttribute>(CoreAttribute.Strength, CoreAttribute.Charisma), new List<Rank2Roles> { Rank2Roles.Enforcer, Rank2Roles.Bouncer, Rank2Roles.Politician, Rank2Roles.Prostitute } },
            { new Tuple<CoreAttribute, CoreAttribute>(CoreAttribute.Charisma, CoreAttribute.Strength), new List<Rank2Roles> { Rank2Roles.Enforcer, Rank2Roles.Bouncer, Rank2Roles.Politician, Rank2Roles.Prostitute } },
            { new Tuple<CoreAttribute, CoreAttribute>(CoreAttribute.Agility, CoreAttribute.Charisma), new List<Rank2Roles> { Rank2Roles.Pickpocket, Rank2Roles.Con_Artist, Rank2Roles.Entertainer, Rank2Roles.Negotiator } },
            { new Tuple<CoreAttribute, CoreAttribute>(CoreAttribute.Charisma, CoreAttribute.Agility), new List<Rank2Roles> { Rank2Roles.Pickpocket, Rank2Roles.Con_Artist, Rank2Roles.Entertainer, Rank2Roles.Negotiator } },
            { new Tuple<CoreAttribute, CoreAttribute>(CoreAttribute.Strength, CoreAttribute.Agility), new List<Rank2Roles> { Rank2Roles.Assassin, Rank2Roles.Grappler, Rank2Roles.Smuggler, Rank2Roles.Acrobat } },
            { new Tuple<CoreAttribute, CoreAttribute>(CoreAttribute.Agility, CoreAttribute.Strength), new List<Rank2Roles> { Rank2Roles.Assassin, Rank2Roles.Grappler, Rank2Roles.Smuggler, Rank2Roles.Acrobat } },
            { new Tuple<CoreAttribute, CoreAttribute>(CoreAttribute.Intelligence, CoreAttribute.Charisma), new List<Rank2Roles> { Rank2Roles.Mastermind, Rank2Roles.Diplomat, Rank2Roles.Con_Artist, Rank2Roles.Negotiator } },
            { new Tuple<CoreAttribute, CoreAttribute>(CoreAttribute.Charisma, CoreAttribute.Intelligence), new List<Rank2Roles> { Rank2Roles.Mastermind, Rank2Roles.Diplomat, Rank2Roles.Con_Artist, Rank2Roles.Negotiator } }
        };
    }

    public static List<Rank2Roles> GetClassesForAttributes(CoreAttribute attribute1, CoreAttribute attribute2)
    {
      return Roles[new Tuple<CoreAttribute, CoreAttribute>(attribute1, attribute2)];
    }
  }

  public class Character
  {
    public bool Injured { get; set; }
    public int InjuryTime { get; set; }
    public bool ServingJailTime { get; set; }
    public int CurrentJailTime { get; set; }

    public bool IsTraining { get; set; }
    public int TrainingTime { get; set; }    


    public string Name { get; set; }
    public ProfessionInfo? Profession { get; set; }
    public Rank1Roles Rank1Role { get; set; }
    public Rank2Roles? Rank2Role { get; set; }

    public int Gold { get; set; }
    public bool PlayerCharacter { get; private set; }
    public int Rank { get; set; }
    //{
    //  get
    //  {
    //    if (ExperienceSpent / 10 > 0)
    //      return ExperienceSpent / 10;
    //    else
    //      return 1;
    //  }
    //}

    public int Rating
    {
      get
      {
        return Strength.Total + Agility.Total + Intelligence.Total + Charisma.Total;
      }
    }
    public int Wage
    {
      get
      {
        return Rank * 5;
      }
    }
    public int ExperienceEarned
    {
      get
      {
        return ExperienceSpent + ExperienceUnspent;
      }
    }
    public int ExperienceSpent { get; private set; }
    public int ExperienceUnspent { get; private set; }
    public CharacterAttribute Strength { get; set; }
    public CharacterAttribute Intelligence { get; set; }
    public CharacterAttribute Agility { get; set; }
    public CharacterAttribute Charisma { get; set; }
    public List<CharacterAttribute> Attributes => new List<CharacterAttribute> { Strength, Intelligence, Agility, Charisma };

    public CharacterAttribute GetAttributeByType(CoreAttribute attribute)
    {
      switch (attribute)
      {
        case CoreAttribute.Strength:
          return Strength;
        case CoreAttribute.Intelligence:
          return Intelligence;
        case CoreAttribute.Agility:
          return Agility;
        case CoreAttribute.Charisma:
          return Charisma;
        default:
          throw new ArgumentException($"Unknown attribute type: {attribute}");
      }
    }

    public int GetAttributeTotal(CoreAttribute attribute)
    {
      return GetAttributeByType(attribute).Total;
    }

    public bool Increase(CoreAttribute attribute, int value = 1, bool critIncrease = false)
    {
      if (critIncrease)
      {
        ExperienceUnspent++;
      }
      if (ExperienceUnspent < value)
      {
        return false;
      }
      switch (attribute)
      {
        case CoreAttribute.Strength:
          Strength.Train(value);
          break;
        case CoreAttribute.Intelligence:
          Intelligence.Train(value);
          break;
        case CoreAttribute.Agility:
          Agility.Train(value);
          break;
        case CoreAttribute.Charisma:
          Charisma.Train(value);
          break;
        default:
          throw new NotImplementedException();
      }
      ExperienceUnspent -= value;
      ExperienceSpent += value;
      return true;
    }

    public int GetSkillTotal(Skill skill)
    {
      var baseValue = GetAttributeBySkill(skill).Total;
      if (SkillIncreases.ContainsKey(skill))
        baseValue += SkillIncreases[skill].Increases;
      return baseValue;
    }

    public bool Increase(Skill skillToTrain, int value = 1, bool critIncrease = false)
    {
      if (critIncrease)
      {
        ExperienceUnspent++;
      }
      if (ExperienceUnspent < value)
      {
        return false;
      }

      if (SkillIncreases.ContainsKey(skillToTrain))
      {
        SkillIncreases[skillToTrain].Increases += value;
      }
      else
      {
        SkillIncreases.Add(skillToTrain, new CharacterSkills(skillToTrain, value));
      }

      ExperienceUnspent -= value;
      ExperienceSpent += value;
      return true;
    }

    public Dictionary<Skill, CharacterSkills> SkillIncreases { get; set; }

    private IRoller Dice { get; set; }

    public Character(string name, bool isPlayerCharacter, int strength, int intelligence, int agility, int charisma, int gold, int rank = 1)
    {
      Name = name;
      PlayerCharacter = isPlayerCharacter;
      Gold = gold;
      Rank = rank;
      Strength = new CharacterAttribute(CoreAttribute.Strength, strength);
      Intelligence = new CharacterAttribute(CoreAttribute.Intelligence, intelligence);
      Agility = new CharacterAttribute(CoreAttribute.Agility, agility);
      Charisma = new CharacterAttribute(CoreAttribute.Charisma, charisma);
      SkillIncreases = new Dictionary<Skill, CharacterSkills>();
      Dice = new DiceRoller();
      AssignNewRoles();
      AssignNewProfession();
    }

    public void AssignNewProfession()
    {
      Skill? bestSkill = GetBestSkill();
      if (bestSkill != null)
      {
        Profession = ProfessionMapper.FindProfessionBySkill(bestSkill.Value);
      }
      else
      {
        CoreAttribute dominantAttribute = GetDominantAttribute();
        Profession = ProfessionMapper.FindProfessionByAttribute(dominantAttribute);
      }
    }

    private Skill? GetBestSkill()
    {
      // uses GetSkillTotal to find the best Skills for the character
      // returns null if no skills are trained.
      CoreAttribute dominantAttribute = GetDominantAttribute();

      Skill? bestSkill = null;
      int bestSkillTotal = 0;
      if (SkillIncreases.Count == 0) return null;
      foreach (var skill in SkillIncreases.Keys)
      {
        var skillTotal = GetSkillTotal(skill);
        if (skillTotal > bestSkillTotal)
        {
          bestSkill = skill;
          bestSkillTotal = skillTotal;
        }
        if (skillTotal == bestSkillTotal)
        {
          if (SkillUtils.GetCoreAttribute(skill) == dominantAttribute)
          {
            bestSkill = skill;
            bestSkillTotal = skillTotal;
          }
        }
      }
      return bestSkill;
    }

    public void AssignNewRoles()
    {
      Random r = new Random();
      CoreAttribute dominantAttribute = GetDominantAttribute();

      var roles = Rank1RoleMapping.GetRolesForAttribute(dominantAttribute);
      Rank1Role = roles[r.Next(roles.Count)];
      if (Rank > 1)
      {
        var att = Get2DominantAttribute();
        var classes = Rank2RoleMapping.GetClassesForAttributes(att.Item1, att.Item2);
        Rank2Role = classes[r.Next(classes.Count)];
      }
    }

    public (CoreAttribute, CoreAttribute) Get2DominantAttribute()
    {
      int max1 = Math.Max(Math.Max(Strength.Total, Agility.Total),
        Math.Max(Intelligence.Total, Charisma.Total));
      CoreAttribute max1Type = GetDominantAttribute();

      int max2;
      if (max1Type == CoreAttribute.Strength)
      {
        max2 = Math.Max(Agility.Total, Math.Max(Intelligence.Total, Charisma.Total));
      }
      else if (max1Type == CoreAttribute.Agility)
      {
        max2 = Math.Max(Strength.Total, Math.Max(Intelligence.Total, Charisma.Total));
      }
      else if (max1Type == CoreAttribute.Intelligence)
      {
        max2 = Math.Max(Agility.Total, Math.Max(Strength.Total, Charisma.Total));
      }
      else
      {
        max2 = Math.Max(Agility.Total, Math.Max(Intelligence.Total, Strength.Total));
      }

      CoreAttribute max2Type;
      if (max2 == Strength.Total && max1Type != CoreAttribute.Strength)
      {
        max2Type = CoreAttribute.Strength;
      }
      else if (max2 == Agility.Total && max1Type != CoreAttribute.Agility)
      {
        max2Type = CoreAttribute.Agility;
      }
      else if (max2 == Intelligence.Total && max1Type != CoreAttribute.Intelligence)
      {
        max2Type = CoreAttribute.Intelligence;
      }
      else
      {
        max2Type = CoreAttribute.Charisma;
      }

      return (max1Type, max2Type);
    }

    public CoreAttribute GetDominantAttribute()
    {
      int max = Math.Max(Math.Max(Strength.Total, Agility.Total),
        Math.Max(Intelligence.Total, Charisma.Total));

      if (max == Strength.Total)
      {
        return CoreAttribute.Strength;
      }
      if (max == Agility.Total)
      {
        return CoreAttribute.Agility;
      }
      if (max == Intelligence.Total)
      {
        return CoreAttribute.Intelligence;
      }
      return CoreAttribute.Charisma;
    }

    public int GetSkillRating(Skill skill)
    {
      CoreAttribute attribute = SkillUtils.GetCoreAttribute(skill);
      int skillRating = 0;
      switch (attribute)
      {
        case CoreAttribute.Strength:
          skillRating = Strength.Total;
          break;
        case CoreAttribute.Intelligence:
          skillRating = Intelligence.Total;
          break;
        case CoreAttribute.Agility:
          skillRating = Agility.Total;
          break;
        case CoreAttribute.Charisma:
          skillRating = Charisma.Total;
          break;
      }

      // Add skill training
      if (SkillIncreases.ContainsKey(skill))
      {
        skillRating += SkillIncreases[skill].Increases;
      }

      // Add Gear bonus
      // Add Perk bonus

      return skillRating;
    }

    public override string? ToString()
    {
      var sb = new StringBuilder();
      sb.AppendLine($"Name: {Name}");
      sb.AppendLine($"PlayerCharacter: {PlayerCharacter}");
      sb.AppendLine($"Profession: {Profession?.Profession}");
      var description = Profession != null ? ProfessionMapper.GetDescription(Profession.Profession) : "No profession assigned";
      sb.AppendLine(description);
      sb.AppendLine($"Rating: {Rating}");
      sb.AppendLine($"Strength: {Strength.Total}");
      sb.AppendLine($"Agility: {Agility.Total}");
      sb.AppendLine($"Intelligence: {Intelligence.Total}");
      sb.AppendLine($"Charisma: {Charisma.Total}");
      sb.AppendLine($"ExperienceEarned({ExperienceEarned}) = ExperienceSpent({ExperienceSpent}) + ExperienceUnspent({ExperienceUnspent})");
      foreach (var item in SkillIncreases)
      {
        sb.AppendLine($"{item.Key}: {GetSkillRating(item.Key)} with Increases({item.Value.Increases})");
      }
      return sb.ToString();
    }

    public string ShortPrint()
    {
      var sb = new StringBuilder();
      if (PlayerCharacter)
      {
        sb.Append($"PLAYER - ");
      }

      sb.Append($"{Name} - {Profession?.Profession} - ");

      sb.Append($"R: {Rating} - S: {Strength.Total} - A: {Agility.Total} - I: {Intelligence.Total} - C: {Charisma.Total}");
      if (Injured)
      {
        sb.Append($" - INJURED {InjuryTime} Weeks");
      }
      if (ServingJailTime)
      {
        sb.Append($" - CAPTURED {CurrentJailTime} Weeks");
      }
      return sb.ToString();
    }

    public string ShortTrainingPrint()
    {
      var sb = new StringBuilder();
      if (PlayerCharacter)
      {
        sb.Append($"PLAYER - ");
      }
      sb.Append($"{Name} - {Profession?.Profession} - ");
      sb.Append($"R: {Rating} - S: {Strength.Total} - A: {Agility.Total} - I: {Intelligence.Total} - C: {Charisma.Total} - ");
      sb.Append($"Experience Unspent: {ExperienceUnspent}");

      return sb.ToString();
    }

    public string LongTrainingPrint()
    {
      var sb = new StringBuilder();
      if (PlayerCharacter)
      {
        sb.Append($"PLAYER - ");
      }
      sb.Append($"{Name} - {Profession?.Profession} - ");
      sb.Append($"R: {Rating} - S: {Strength.Total} - A: {Agility.Total} - I: {Intelligence.Total} - C: {Charisma.Total} - ");
      foreach (var item in SkillIncreases)
      {
        sb.AppendLine($"{item.Key}: {GetSkillRating(item.Key)}");
      }

      sb.AppendLine($"Experience Total: {ExperienceEarned}");
      sb.AppendLine($"Experience Spent: {ExperienceSpent}");
      sb.AppendLine($"Experience Unspent: {ExperienceUnspent}");
  
      return sb.ToString();
    }

    public string? ExtendedCharacterSheet()
    {
      var sb = new StringBuilder();
      sb.AppendLine($"Name: {Name}");
      sb.AppendLine($"PlayerCharacter: {PlayerCharacter}");
      sb.AppendLine($"Strength: {Strength.Total}");
      sb.AppendLine($"Agility: {Agility.Total}");
      sb.AppendLine($"Intelligence: {Intelligence.Total}");
      sb.AppendLine($"Charisma: {Charisma.Total}");
      sb.AppendLine($"ExperienceEarned: {ExperienceEarned}");
      sb.AppendLine($"ExperienceSpent: {ExperienceSpent}");
      sb.AppendLine($"ExperienceUnspent: {ExperienceUnspent}");
      sb.AppendLine($"Wage: {Wage}");
      sb.AppendLine($"Gold: {Gold}");

      return sb.ToString();
    }

    internal bool IsAvailable()
    {
      return !Injured && !ServingJailTime;
    }

    internal void Injury(int time)
    {
      Injured = true;
      InjuryTime = time;
    }
    internal void Capture(int time)
    {
      ServingJailTime = true;
      CurrentJailTime = time;
    }

    internal void AdvanceTime(bool awardXP = true)
    {
      if (Injured)
      {
        InjuryTime--;
        if (InjuryTime <= 0)
        {
          Injured = false;
          InjuryTime = 0;
        }
      }
      if (ServingJailTime)
      {
        CurrentJailTime--;
        if (CurrentJailTime <= 0)
        {
          ServingJailTime = false;
          CurrentJailTime = 0;
        }
      }
      if (IsTraining)
      {
        TrainingTime--;
        if (TrainingTime <= 0)
        {
          IsTraining = false;
          TrainingTime = 0;
        }

        AssignNewProfession();
      }
      if (awardXP)
      {
        ExperienceUnspent++;
      }
    }

    internal void AwardBonusXp(int value)
    {
      ExperienceUnspent += value;
    }

    public void AddExperience(int amount)
    {
        ExperienceUnspent += amount;
    }

    public void AddGold(int amount)
    {
        Gold += amount;
    }

    public void AddReputation(int amount)
    {
        // TODO: Implement reputation system
    }

    public SkillCheckResultWrapper SkillCheck(Skill skill, int difficulty = 0)
    {
      var skillValue = GetSkillTotal(skill);
      var input = new SimpleInput(skillValue, difficulty);
      var result = Dice.Simple(input);
      return new SkillCheckResultWrapper(new SimpleSkillCheck(skill, (ChallengeLevel)difficulty), result);
    }

    public CharacterAttribute GetAttributeBySkill(Skill skill)
    {
      var attribute = SkillUtils.GetCoreAttribute(skill);
      return GetAttributeByType(attribute);
    }
  }
}