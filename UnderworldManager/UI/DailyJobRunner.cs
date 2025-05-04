using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using UnderworldManager.Business;
using UnderworldManager.Models;

namespace UnderworldManager
{
  /// <summary>
  /// Each gang member does a basic skill check. If they succeed, they get a reward.
  /// If they fail, they get threat increases.
  /// After each round theres a city watch check based on threat level, to see if they get chased.
  /// If they get chased they get a chance to escape or fight. 
  /// if they fail they get captured. Fighting will give an injury and increase capture time.
  /// If they escape, you can chose to end daily job or push your luck and continue.
  /// Each round you also roll to get a rumor. Rumors can be: Missions or maybe events (which are not added yet)
  /// Earning is based on how well gang members roll. Or maybe some are steady pay and others are riskier. 
  /// Expansion options:
  /// Muscles are always just hired as security, and they get paid a flat rate. For a high success rate the reward would increase. Failing big would increase threat.
  /// Burglars are dependant on their skill, and they must succeed their roll to earn. Failing would increase threat.
  /// Schemers are very dependant on their skill, and they must succeed +2 to earn significant money their roll to earn. Failing big would increase threat.
  /// SmoothTalkers are very dependant on their skill, and they must succeed +2 to earn significant money their roll to earn. Failing big would increase threat.
  /// Rumors: All members get a extra streewise check: on a +2 roll a rumor is earned.
  /// </summary>
  public record DailyJobResult(int Earnings, int Threat, List<Character> CritSuccess);

  public class DailyJobRunner
  {
    private readonly Roster _roster;
    private readonly ConflictEngine _conflictEngine;
    private int _threat;

    public DailyJobRunner(Roster roster, ConflictEngine conflictEngine, int threat)
    {
      _roster = roster;
      _conflictEngine = conflictEngine;
      _threat = threat;
    }

    public void Run()
    {
      Console.Out.WriteLine();
      Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
      Console.Out.WriteLine("║                    DAILY JOBS                             ║");
      Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
      Console.Out.WriteLine("║ Your gang members are working their daily jobs...         ║");
      Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
      Console.Out.WriteLine($"║ Current Threat Level: {_threat}                          ║");
      Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
      Console.Out.WriteLine();

      foreach (var member in _roster.Active)
      {
        Console.Out.WriteLine($"║ {member.Name} is working...                           ║");
        if (member.Profession == null)
        {
          Console.Out.WriteLine($"║ {member.Name} has no profession and cannot work     ║");
          continue;
        }
        var result = _conflictEngine.Run(new SimpleSkillCheck(member.Profession.Skill, SetChallengeLevel(_threat)), member);
        var modifiedSkill = result.DiceRoll.TestedSkillValue + result.Check.Difficulty;
        Console.Out.Write($"{member.Name} the {member.Profession.Profession}: {result.Check.ChallengeLevel}(+{result.Check.Difficulty}) {result.Check.Skill} Check: ({result.DiceRoll.TestedSkillValue}+{result.Check.Difficulty}={modifiedSkill}): ");
        PrintRollResult(result.DiceRoll);
      }

      Console.Out.WriteLine();
      Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
      Console.Out.WriteLine("║                    DAILY RESULTS                          ║");
      Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
      Console.Out.WriteLine("║ Press Enter to continue...                                ║");
      Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
      Console.ReadLine();
    }

    private static void PrintRollResult(SimpleResult result)
    {
      Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
      Console.Out.WriteLine("║                      ROLL RESULT                          ║");
      Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
      Console.Out.WriteLine($"║ Roll: {result.Roll}                                      ║");
      Console.Out.WriteLine($"║ Target: {result.TestedSkillValue}                        ║");
      Console.Out.WriteLine($"║ Success Level: {result.Successlevel}                     ║");
      Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }

    private static ChallengeLevel SetChallengeLevel(int threat)
    {
      if (threat < 20)
        return ChallengeLevel.Easy;
      if (threat < 40)
        return ChallengeLevel.Medium;
      else
        return ChallengeLevel.Hard;
    }

    public DailyJobResult SingleRun()
    {
      var earnings = 0;
      var critSuccess = new List<Character>();

      foreach (var member in _roster.Active)
      {
        if (member.Profession == null)
        {
          continue;
        }
        var result = _conflictEngine.Run(new SimpleSkillCheck(member.Profession.Skill, SetChallengeLevel(_threat)), member);
        if (result.DiceRoll.Success)
        {
          earnings += 10 * result.DiceRoll.Successlevel;
          if (result.DiceRoll.Successlevel >= 2)
          {
            critSuccess.Add(member);
          }
        }
        else
        {
          _threat += 5;
        }
      }

      return new DailyJobResult(earnings, _threat, critSuccess);
    }
  }

  public enum DailyJobState
  {
    Running,
    AgentCaptured,
    Aborted
  }

  public class SimpleAttributeCheck
  {
    public CoreAttribute Attribute { get; set; }
    public ChallengeLevel ChallengeLevel { get; set; }
    public int Difficulty { get { return (int)ChallengeLevel; } }

    public SimpleAttributeCheck(CoreAttribute attribute, ChallengeLevel challengeLevel)
    {
      Attribute = attribute;
      ChallengeLevel = challengeLevel;
    }
  }
}