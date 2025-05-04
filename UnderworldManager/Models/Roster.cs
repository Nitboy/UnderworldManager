using System.Data;
using System.Text;
using System.Xml.Linq;

namespace UnderworldManager.Models
{
  public class Roster
  {
    public List<Character> Active { get; }
    public List<Character> Inactive { get; }
    public List<Character> Muscles { get; }
    public List<Character> Schemers { get; }
    public List<Character> Burglars { get; }
    public List<Character> SmoothTalkers { get; }

    public Roster()
    {
      Muscles = new List<Character>();
      Schemers = new List<Character>();
      Burglars = new List<Character>();
      SmoothTalkers = new List<Character>();
      Active = new List<Character>();
      Inactive = new List<Character>();
    }

    public void AutoRoster(List<Character> characters)
    {
      foreach (Character character in characters)
      {
        if (character.IsAvailable())
        {
          AddToRoster(character);
        }
        else
        {
          Inactive.Add(character);
        }
      };
    }

    public void AddToRoster(Character character)
    {
      var dominant = character.GetDominantAttribute();
      switch (dominant)
      {
        case CoreAttribute.Strength:
          Muscles.Add(character);
          break;
        case CoreAttribute.Intelligence:
          Schemers.Add(character);
          break;
        case CoreAttribute.Agility:
          Burglars.Add(character);
          break;
        case CoreAttribute.Charisma:
          SmoothTalkers.Add(character);
          break;
      }

      Active.Add(character);
    }

    public void RemoveFromRoster(Character character)
    {
      var dominant = character.GetDominantAttribute();
      switch (dominant)
      {
        case CoreAttribute.Strength:
          Muscles.Remove(character);
          break;
        case CoreAttribute.Intelligence:
          Schemers.Remove(character);
          break;
        case CoreAttribute.Agility:
          Burglars.Remove(character);
          break;
        case CoreAttribute.Charisma:
          SmoothTalkers.Remove(character);
          break;
      }

      Active.Remove(character);
      Inactive.Add(character);
    }

    public void Clear()
    {
      Muscles.Clear();
      Schemers.Clear();
      Burglars.Clear();
      SmoothTalkers.Clear();
      Active.Clear();
      Inactive.Clear();
    }

    public (string, List<Character>) ListCharactersForChallenge(Skill skill)
    {
      var attribute = SkillUtils.GetCoreAttribute(skill);
      var sb = new StringBuilder();
      switch (attribute)
      {
        case CoreAttribute.Strength:
          if (!Muscles.Any())
          {
            return ("No Specialist Available", Active);
          }
          return ("Muscles", Muscles);
        case CoreAttribute.Intelligence:
          if (!Schemers.Any())
          {
            return ("No Specialist Available", Active);
          }
          return ("Schemers", Schemers);
        case CoreAttribute.Agility:
          if (!Burglars.Any())
          {
            return ("No Specialist Available", Active);
          }
          return ("Burglars", Burglars);
        case CoreAttribute.Charisma:
          if (!SmoothTalkers.Any())
          {
            return ("No Specialist Available", Active);
          }
          return ("SmoothTalkers", SmoothTalkers);
      }
      throw new NotImplementedException();
    }

    public override string? ToString()
    {
      var sb = new StringBuilder();
      sb.AppendLine($"Active:");
      foreach (var character in Active) { sb.AppendLine("  " + character.ShortPrint()); }
      if (Inactive.Any())
      {
        sb.AppendLine($"Inactive:");
        foreach (var character in Inactive) { sb.AppendLine("  " + character.ShortPrint()); }
      }
      return sb.ToString();
    }

    public string PrintRosterGrouped()
    {
      var sb = new StringBuilder();
      sb.AppendLine($"Muscles:");
      foreach (var character in Muscles) { sb.AppendLine("  " + character.ShortPrint()); }

      sb.AppendLine($"Schemers:");
      foreach (var character in Schemers) { sb.AppendLine("  " + character.ShortPrint()); }

      sb.AppendLine($"Burglars:");
      foreach (var character in Burglars) { sb.AppendLine("  " + character.ShortPrint()); }

      sb.AppendLine($"SmoothTalkers:");
      foreach (var character in SmoothTalkers) { sb.AppendLine("  " + character.ShortPrint()); }

      return sb.ToString();
    }
  }
}
