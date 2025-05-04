using System.Diagnostics.Metrics;
using System;
using UnderworldManager.Models;

namespace UnderworldManager.Business
{
  public class CharGen
  {
    private Random r = new Random();
    public Character CreateCharacter(int rank = 1, string? name = null, bool isMain = false)
    {
      var dice = new RandomRoller();

      var strength = RollStat(dice);
      var intelligence = RollStat(dice);
      var agility = RollStat(dice);
      var charisma = RollStat(dice);
      var gold = RollGold(dice, rank);

      var character = new Character(name ?? GenerateName(),
        isMain, strength,
        intelligence,
        agility,
        charisma,
        gold,
        rank);
      
      if(rank > 1)
      {
        character.AwardBonusXp(50 * (rank - 1));

        character.Strength.Train(dice.RollD6() * (rank - 1));
        character.Agility.Train(dice.RollD6() * (rank - 1));
        character.Charisma.Train(dice.RollD6() * (rank - 1));
        character.Intelligence.Train(dice.RollD6() * (rank - 1));

        List<Skill> skillList = GetRandomSkills(dice.RollD10());

        foreach (var skill in skillList)
        {
          character.Increase(skill, dice.RollD6() * (rank - 1));
        }

        var attribute = character.Get2DominantAttribute();
        var dominantSkills = CharacterSkills.GetSkillsByAttribute(attribute.Item1);
        foreach (var skill in dominantSkills)
        {
          character.Increase(skill, dice.RollD6());
        }
        var dominant2ndSkills = CharacterSkills.GetSkillsByAttribute(attribute.Item2);
        foreach (var skill in dominant2ndSkills)
        {
          character.Increase(skill, dice.RollD3());
        }
      }
      character.AssignNewProfession();
      character.AssignNewRoles();
      return character;
    }

    private List<Skill> GetRandomSkills(int v)
    {
      var skills = new List<Skill>();
      Random r = new Random();
      for (int i = 0; i < v; i++)
      {
        skills.Add((Skill)r.Next(22));
      }
      return skills;
      
    }

    public Character CreateEpic(int rank = 3, string? name = null, bool isMain = false)
    {
      return CreateCharacter(rank, GenerateEpicName(), isMain);
    }
    public Character CreateNoble(int rank = 3, string? name = null, bool isMain = false)
    {
      return CreateCharacter(rank, GenerateNobleName(), isMain);
    }

    private string GenerateName()
    {
      return reiklanderNames[r.Next(reiklanderNames.Count)] + " "
        + medievalGermanSurnames[r.Next(medievalGermanSurnames.Count)];
    }

    private string GenerateNobleName()
    {
      return reiklanderNames[r.Next(reiklanderNames.Count)] + " von "
        + imperialCityNames[r.Next(imperialCityNames.Count)];
    }

    private string GenerateEpicName()
    {
      return epicNames[r.Next(epicNames.Count)];
    }

    private int RollStat(RandomRoller dice)
    {
      return 20 + dice.RollD10() + dice.RollD10();
    }

    private int RollGold(RandomRoller dice, int Rank)
    {
      return dice.RollD10() + dice.RollD10() * Rank + dice.RollD100() * Rank - 1;
    }

    List<string> imperialCityNames = new List<string>()
            {
                "Altdorf",
                "Averheim",
                "Bögenhafen",
                "Carroburg",
                "Erengrad",
                "Esstfeld",
                "Grünburg",
                "Holswig-Schliestein",
                "Kislev",
                "Middenheim",
                "Nuln",
                "Pfeildorf",
                "Quenelles",
                "Reikland",
                "Remas",
                "Talabheim",
                "Wissenland",
                "Wolfenburg",
                "Zundap",
                "Adrath",
                "Ambergrund",
                "Bachhofen",
                "Bartenstein",
                "Blutfels",
                "Burgstreif",
                "Diebitsch",
                "Eilhart",
                "Goromond",
                "Heimburg",
                "Kallenfels",
                "Lauterbach",
                "Mittelreich",
                "Nasavend",
                "Oberhausen",
                "Pfeilhafen",
                "Quenlingen",
                "Reinharz",
                "Schmieden",
                "Talabrand",
                "Untergard",
                "Vogelsang",
                "Wolkenburg",
                "Zwergenfeld",
                "Adlerhorst",
                "Ambergau",
                "Bachlauf",
                "Bartenhof",
                "Blutström",
                "Burggraf",
                "Dornfeld",
                "Eilfort",
                "Gormond",
                "Heimgard",
                "Kallenfeld",
                "Lautern",
                "Mittenreich",
                "Nasawald",
                "Oberhafen",
                "Pfeilreich",
                "Quentling",
                "Reinhafen",
                "Schmiedhof",
                "Talabrunn",
                "Untereich"
            };

    List<string> reiklanderNames = new List<string>()
            {
                "Alfred",
                "Alois",
                "Balthazar",
                "Bruno",
                "Caroline",
                "Charlotte",
                "Claudia",
                "Dieter",
                "Erich",
                "Friedrich",
                "Gertrude",
                "Heinrich",
                "Hildegard",
                "Ingrid",
                "Johann",
                "Karl",
                "Kurt",
                "Ludwig",
                "Margarete",
                "Maximilian",
                "Merta",
                "Nina",
                "Oskar",
                "Paula",
                "Rudolf",
                "Siegfried",
                "Theresa",
                "Ulrich",
                "Viktor",
                "Werner",
                "Xaver",
                "Yvette",
                "Zoe",
                "Adolf",
                "Berta",
                "Curtis",
                "Dora",
                "Emil",
                "Frieda",
                "Gustav",
                "Hermann",
                "Inga",
                "Johanna",
                "Klaus",
                "Ludwig",
                "Marta",
                "Norbert",
                "Ottilie",
                "Petra",
                "Quincy",
                "Rosa",
                "Sophie",
                "Tobias",
                "Ursula",
                "Veronika",
                "Wanda",
                "Xenia",
                "Yvonne",
                "Zacharias"
            };
    List<string> medievalGermanSurnames = new List<string>()
            {
                "Bauer",
                "Berger",
                "Fischer",
                "Schmidt",
                "Schneider",
                "Müller",
                "Weber",
                "Wagner",
                "Becker",
                "Schulz",
                "Hoffmann",
                "Schäfer",
                "Koch",
                "Baumann",
                "Richter",
                "Klein",
                "Wolf",
                "Schröder",
                "Neumann",
                "Schwarz",
                "Zimmermann",
                "Braun",
                "Krämer",
                "Hofmann",
                "Hartmann",
                "Lange",
                "Schmitt",
                "Werner",
                "Schmitz",
                "Krause",
                "Meier",
                "Lehmann",
                "Schmid",
                "Schulze",
                "Maier",
                "Köhler",
                "Herzog",
                "Walter",
                "Mayer",
                "Huber",
                "Kaiser",
                "Fuchs",
                "Peters",
                "Lang",
                "Scholz",
                "Möller",
                "Weiß",
                "Jung",
                "Hahn",
                "Keller",
                "Vogel",
                "Roth",
                "Beck",
                "Lorenz",
                "Böhm",
                "Hayes",
                "Horn",
                "Buchner",
                "Adler",
                "Auerbach",
                "Bach",
                "Bauer",
                "Becker",
                "Berg",
                "Brandt",
                "Braun",
                "Bremen",
                "Buchholz",
                "Danzig",
                "Dorn",
                "Eckstein",
                "Eichhorn",
                "Falkenhausen",
                "Fischer",
                "Flieger",
                "Freudenreich",
                "Freytag",
                "Goltz",
                "Graf",
                "Hammerstein",
                "Hartmann",
                "Heidelberg",
                "Hochstaden",
                "Hohenzollern"
        };

    List<string> epicNames = new List<string>()
            {
                "Aethis",
                "Alaric",
                "Balthazar",
                "Boris",
                "Caradryel",
                "Crom",
                "Damian",
                "Darius",
                "Dorn",
                "Durin",
                "Eldar",
                "Erevan",
                "Faerin",
                "Fenris",
                "Grombrindal",
                "Grommash",
                "Gruumsh",
                "Hakon",
                "Harald",
                "Hendrik",
                "Ingvar",
                "Izrador",
                "Jarek",
                "Kaldor",
                "Karaash",
                "Kazador",
                "Korbin",
                "Lokoth",
                "Malekith",
                "Marek",
                "Morathi",
                "Nagash",
                "Nathrakh",
                "Nergal",
                "Njal",
                "Norscan",
                "Odin",
                "Rasputin",
                "Rhun",
                "Rorik",
                "Sauron",
                "Sigmar",
                "Sigmund",
                "Sigurd",
                "Tchar'zanek",
                "Thorgar",
                "Thornn",
                "Thrudgelmir",
                "Ungrim",
                "Valten",
                "Vulcan",
                "Wulfrik",
                "Zarathustra"
            };
  }
}