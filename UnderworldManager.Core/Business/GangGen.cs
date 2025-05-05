using System;
using UnderworldManager.Core.Models;
using UnderworldManager.Models;

namespace UnderworldManager.Core.Business
{
  public class GangGen
  {
    private Random r = new Random();
    public Gang CreateGang(int tier = 1, string? name = null)
    {
      CharGen cg = new CharGen();
      Random random = new Random();
      int adjectiveIndex = random.Next(colorfulAdjectives.Count);
      int suffixIndex = random.Next(gangSuffixes.Count);

      string gangName = name ?? colorfulAdjectives[adjectiveIndex] + " " + gangSuffixes[suffixIndex];

      var garrista = cg.CreateEpic(tier + 1, null, false);

      var rank1memberCount = 5 + tier * random.Next(tier, 6 * tier);
      var rank2memberCount = 1 + (tier - 1) * random.Next(6);
      var rank3memberCount = 1 + (tier - 2) * random.Next(6);

      var members = new List<Character>
      {
        garrista
      };

      for (int i = 0; i < rank1memberCount; i++)
      {
        members.Add(cg.CreateCharacter());
      }
      if (tier > 1)
      {
        for (int i = 0; i < rank2memberCount; i++)
        {
          members.Add(cg.CreateCharacter(2));
        }
      }
      if (tier > 2)
      {
        for (int i = 0; i < rank3memberCount; i++)
        {
          members.Add(cg.CreateCharacter(3));
        }
      }

      return new Gang(gangName, tier, ((tier - 1) * 50) + random.Next(100) * tier * tier, members, garrista);
    }

    List<string> colorfulAdjectives = new List<string>()
            {
                "Deadly",
                "Ruthless",
                "Fearsome",
                "Bloodthirsty",
                "Dreadful",
                "Merciless",
                "Vicious",
                "Savage",
                "Cruel",
                "Ferocious",
                "Terrifying",
                "Lethal",
                "Deadly",
                "Menacing",
                "Mercenary",
                "Vigilante",
                "Nefarious",
                "Spiteful",
                "Nefarious",
                "Hateful"
            };
    List<string> gangSuffixes = new List<string>()
            {
                "Gang",
                "Outlaws",
                "Crew",
                "Boys",
                "Mobs",
                "Thugs",
                "Gangs",
                "Punks",
                "Hounds",
                "Bastards",
                "Mercenaries",
                "Bandits",
                "Brigands",
                "Cutthroats",
                "Raiders",
                "Scoundrels",
                "Ruffians",
                "Marauders",
                "Brigades",
                "Outlaws",
                "Warriors",
                "Companions",
                "Rogues",
                "Hooligans",
                "Plunderers",
                "Swashbucklers",
                "Pillagers",
                "Adventurers"
            };



  }
}