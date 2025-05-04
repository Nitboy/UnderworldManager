using System.Text;

namespace UnderworldManager.Models
{
  public class Gang
  {
    public string Name { get; set; }

    public int Tier { get; set; }

    // Count up when pulling an honorable thief job
    public int Fame { get; set; }

    // Count up when pulling violent robbery, assassination, kidnapping or the like.
    public int Infamy { get; set; }
    public int Gold { get; set; }

    public Character Garrista { get; set; }
    public List<Character> Members { get; set; }

    public Roster Roster { get; set; }

    public Gang(string name, int tier, int gold, List<Character> members, Character garrista)
    {
      Name = name;
      Tier = tier;
      Gold = gold;
      Members = members;
      Garrista = garrista;
      Roster = new Roster();
      ResetRoster();
    }

    public void ResetRoster()
    {
      Roster.Clear();
      Roster.AutoRoster(Members);
    }

    public override string? ToString()
    {
      var sb = new StringBuilder();
      sb.AppendLine($"Name: {Name}");
      sb.AppendLine($"Tier: {Tier}");
      sb.AppendLine($"Gold: {Gold}");
      sb.AppendLine($"Garrista: {Garrista.Name}. {Garrista.Rank1Role} and {Garrista.Rank2Role} Rated: {Garrista.Rating}");
      sb.AppendLine($"Members: {Members.Count}");

      return sb.ToString();
    }

    internal void MissionSuccess(int income, bool honorable)
    {
      // chars on roster must earn extra personal gold or gang reputation.
      // Roster should also earn fame/infamy

      if(honorable)
      {
        Fame++;
      }
      else
      {
        Infamy++;
      }

      Gold += income;
    }

    internal int PaySalary()
    {
      var totalWageToPay = Members.Sum(m => m.Wage);
  
      foreach (var character in Members)
      {
        Gold -= character.Wage;
        character.Gold += character.Wage;        
      }

      if(Gold < 0)
      {
        // Garrista must use personal gold to keep gang running.
        Garrista.Gold += Gold;
        Gold = 0;
      }
      return totalWageToPay;
    }

    internal bool PayCappa(int cappaDues)
    {
      Gold -= cappaDues;

      if (Gold < 0)
      {
        // Garrista must use personal gold to keep gang running.
        Garrista.Gold += Gold;
        Gold = 0;

        if(Garrista.Gold < 0)
        {
          // Garrista is bankrupt
          // Cappa will replace Garrista with a new one.
          // Game over
          // TODO: Implement Game Over
          return false;
        }

      }

      return true;
    }

    internal void AdvanceTime()
    {
      foreach(var character in Members)
      {
        character.AdvanceTime();
      }      
    }
  }
}