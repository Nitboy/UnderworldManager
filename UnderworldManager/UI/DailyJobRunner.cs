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
      Console.Out.WriteLine("DAILY JOBS");
      Console.Out.WriteLine("Assigning jobs to your gang members...");
      Console.Out.WriteLine();

      foreach (var member in _roster.Active)
      {
        Console.Out.WriteLine($"{member.Name} is working...");
        if (member.Profession == null)
        {
          Console.Out.WriteLine($"{member.Name} has no profession and cannot work");
          continue;
        }

        var result = DoChallenge(member, new SimpleSkillCheck(member.Profession.Skill, SetChallengeLevel(_threat)));
        PrintRollResult(result);

        if (result.Successlevel >= 2)
        {
          Console.Out.WriteLine();
          Console.Out.WriteLine("CRITICAL SUCCESS");
          Console.Out.WriteLine($"{member.Name} has achieved a critical success!");
          _roster.RemoveFromRoster(member);
        }
        else if (result.Successlevel <= -2)
        {
          Console.Out.WriteLine();
          Console.Out.WriteLine("CRITICAL FAILURE");
          Console.Out.WriteLine($"{member.Name} has attracted unwanted attention!");
          
          if (!AvoidCapture(member, SetChallengeLevel(_threat)))
          {
            Console.Out.WriteLine();
            Console.Out.WriteLine("AGENT CAPTURED");
            Console.Out.WriteLine($"{member.Name} has been captured by the city watch!");
            _roster.RemoveFromRoster(member);
          }
        }

        Console.Out.WriteLine();
        Console.Out.WriteLine("Press Enter to continue...");
        Console.ReadLine();
      }

      Console.Out.WriteLine();
      Console.Out.WriteLine("DAILY RESULTS");
      Console.Out.WriteLine($"Total Earnings: {GetCurrentEarnings()} gold");
      Console.Out.WriteLine($"Threat Increase: {_threat}");
      Console.Out.WriteLine();
      Console.Out.WriteLine("Press Enter to continue...");
      Console.ReadLine();
    }

    private static void PrintRollResult(SimpleResult result)
    {
      Console.Out.WriteLine($"Roll: {result.Roll} Target: {result.TestedSkillValue} Success Level: {result.Successlevel}");
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

    private SimpleResult DoChallenge(Character character, SimpleSkillCheck check)
    {
        var result = _conflictEngine.Run(check, character);
        return result.DiceRoll;
    }

    private int GetCurrentEarnings()
    {
        return 10; // Default earnings for now
    }

    private bool AvoidCapture(Character character, ChallengeLevel challengeLevel)
    {
        var check = new SimpleSkillCheck(Skill.Athletics, challengeLevel);
        var result = _conflictEngine.Run(check, character);
        return result.DiceRoll.Success;
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