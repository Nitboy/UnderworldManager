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
    private DailyJobState _state;

    private Gang _gang;
    private ConflictEngine _conflictEngine;
    private List<SimpleAttributeCheckResult> _attributeCheckOutcomes = new List<SimpleAttributeCheckResult>();
    private List<SimpleSkillCheckResult> _skillCheckOutcomes = new List<SimpleSkillCheckResult>();
    private List<CharacterInjury> _agentsInjured = new List<CharacterInjury>();
    private List<CharacterCapture> _agentsCaptured = new List<CharacterCapture>();

    private List<Character> _memberCritSuccess = new List<Character>();
    private int _threat;
    private readonly int _basicEarningFactor;
    private readonly int _skillEarningFactor;
    private int _round = 1;

    public DailyJobRunner(Gang gang, ConflictEngine conflictEngine, int startingThreat, int basicEarningFactor = 2, int skillEarningFactor = 3)
    {
      _gang = gang;
      _conflictEngine = conflictEngine;
      _threat = startingThreat;
      this._basicEarningFactor = basicEarningFactor;
      this._skillEarningFactor = skillEarningFactor;
    }

    public DailyJobResult SingleRun()
    {
      Console.Out.WriteLine($"");
      Console.Out.WriteLine($"The gang is going out for their daily jobs.");
      Console.Out.WriteLine($"Your roster for this job is:");
      Console.Out.WriteLine(_gang.Roster);

      List<Character> removeFromRoster = new List<Character>();

      // foreach gang member. Do a skill check based on their profession.
      foreach (var member in _gang.Roster.Active)
      {
        SimpleResult result;
        ChallengeLevel challengeLevel = SetChallengeLevel(_threat);

        if (ProfessionMapper.IsBasicProfession(member.Profession))
        {
          var attribute = ProfessionMapper.FindCoreAttributeByProfession(member.Profession);
          var check = new SimpleAttributeCheck(attribute, challengeLevel);
          result = DoChallenge(member, check);
        }
        else
        {
          var mainSkill = ProfessionMapper.FindSkillByProfession(member.Profession);
          var check = new SimpleSkillCheck(mainSkill, challengeLevel);
          result = DoChallenge(member, check);
        }

        var memberIsSafe = HandleEventsOfResult(member, result);
        if (!memberIsSafe)
        {
          removeFromRoster.Add(member);
        }

        if (result.Success && result.Crit)
        {
          _memberCritSuccess.Add(member);
        }
      }

      foreach (var member in removeFromRoster)
      {
        _gang.Roster.RemoveFromRoster(member);
      }

      // Update on missions stats. Current success level and threat
      var currentEarnings = GetCurrentEarnings();
      Console.Out.WriteLine($"Your earnings is {currentEarnings}. Current threat level is {_threat}");
      Console.Out.WriteLine($"Members Captured: {_agentsCaptured.Count}. Members Injured: {_agentsInjured.Count}.");
      Console.Out.WriteLine();
      var earnings = GetCurrentEarnings();
      return new DailyJobResult(earnings, _threat, _memberCritSuccess);
    }

    public DailyJobResult Run()
    {
      Console.Out.WriteLine();
      Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
      Console.Out.WriteLine("║                    DAILY JOB START                        ║");
      Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
      Console.Out.WriteLine("║ Sending your gang out to earn some gold...                ║");
      Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
      Console.Out.WriteLine($"║ Current Threat Level: {_threat}                          ║");
      Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
      Console.Out.WriteLine();

      _state = DailyJobState.Running;
      while (_state == DailyJobState.Running)
      {
        Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.Out.WriteLine($"║                      ROUND {_round}                      ║");
        Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");

        foreach (var member in _gang.Roster.Active)
        {
          Console.Out.WriteLine($"║ {member.Name} is working...                           ║");
          var result = HandleEventsOfResult(member, DoChallenge(member, new SimpleSkillCheck(member.Profession.Skill, SetChallengeLevel(_threat))));
          if (!result)
          {
            _state = DailyJobState.AgentCaptured;
            break;
          }
        }

        if (_state == DailyJobState.Running)
        {
          Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
          Console.Out.WriteLine("║ Press Enter to continue or 'q' to quit...                ║");
          Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
          
          var input = Console.ReadLine();
          if (input?.ToLower() == "q")
          {
            _state = DailyJobState.Aborted;
          }
          else
          {
            _round++;
          }
        }
      }

      Console.Out.WriteLine();
      Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
      Console.Out.WriteLine("║                    DAILY JOB RESULTS                      ║");
      Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
      Console.Out.WriteLine($"║ Total Earnings: {GetCurrentEarnings()} gold              ║");
      Console.Out.WriteLine($"║ Threat Increase: {_threat}                               ║");
      Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
      Console.Out.WriteLine("║ Press Enter to continue...                                ║");
      Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
      Console.ReadLine();

      return new DailyJobResult(GetCurrentEarnings(), _threat, _memberCritSuccess);
    }

    private bool HandleEventsOfResult(Character member, SimpleResult result)
    {
      if (result.SuccessLevel >= 2)
      {
        Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.Out.WriteLine("║                    CRITICAL SUCCESS                       ║");
        Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
        Console.Out.WriteLine($"║ {member.Name} has achieved a critical success!           ║");
        Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
        _memberCritSuccess.Add(member);
      }
      else if (result.SuccessLevel <= -2)
      {
        Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.Out.WriteLine("║                    CRITICAL FAILURE                       ║");
        Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
        Console.Out.WriteLine($"║ {member.Name} has attracted unwanted attention!          ║");
        Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
        
        if (!AvoidCapture(member, SetChallengeLevel(_threat)))
        {
          Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
          Console.Out.WriteLine("║                    AGENT CAPTURED                        ║");
          Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
          Console.Out.WriteLine($"║ {member.Name} has been captured by the city watch!      ║");
          Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
          return false;
        }
      }
      return true;
    }

    private int GetCurrentEarnings()
    {
      return _attributeCheckOutcomes.Sum(x => x.DiceRoll.Successlevel) * _basicEarningFactor + _skillCheckOutcomes.Sum(x => x.DiceRoll.Successlevel) * _skillEarningFactor;
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

    private bool AvoidCapture(Character character, ChallengeLevel challengeLevel)
    {
      Console.Out.WriteLine($"***** City Watch is chasing {character.Name}");

      var check = new SimpleSkillCheck(Skill.Athletics, challengeLevel);
      var result = _conflictEngine.Run(check, character);
      var modifiedSkill = result.DiceRoll.Input.Skill + check.Difficulty;
      Console.Out.Write($"***** {character.Name}: {check.ChallengeLevel}(+{check.Difficulty}) {check.Skill} Check: ({result.DiceRoll.Input.Skill}+{check.Difficulty}={modifiedSkill}): ");
      PrintRollResult(result.DiceRoll);

      if (!result.DiceRoll.Success)
      {
        // agent capture
        var severity = _threat / 10;
        Console.Out.WriteLine($"***** {character.Name} is captured and will be locked up for threat/10 weeks: {severity}");
        _agentsCaptured.Add(new CharacterCapture(character, severity));
        character.Capture(severity);
        return false;

      }
      else if (result.DiceRoll.Successlevel == 0)
      {
        // agent escaped but sustains a minor injury
        Console.Out.WriteLine($"***** {character.Name} escaped but sustains a minor injury (Ready next week).");
        _agentsInjured.Add(new CharacterInjury(character, 0));
        character.Injury(0);
        return false;

      }
      else if (result.DiceRoll.Successlevel > 0)
      {
        // agent escapes unharmed
        Console.Out.WriteLine($"***** {character.Name} escaped unharmed");
      }

      return true;
    }

    private SimpleResult DoChallenge(Character character, SimpleAttributeCheck check)
    {
      var result = _conflictEngine.Run(check, character);
      _attributeCheckOutcomes.Add(result);
      var modifiedSkill = result.DiceRoll.Input.Skill + check.Difficulty;
      Console.Out.Write($"{character.Name} the {character.Profession}: {check.ChallengeLevel}(+{check.Difficulty}) {check.Attribute} Check: ({result.DiceRoll.Input.Skill}+{check.Difficulty}={modifiedSkill}): ");
      PrintRollResult(result.DiceRoll);
      return result.DiceRoll;
    }

    private SimpleResult DoChallenge(Character character, SimpleSkillCheck check)
    {
      var result = _conflictEngine.Run(check, character);
      _skillCheckOutcomes.Add(result);
      var modifiedSkill = result.DiceRoll.Input.Skill + check.Difficulty;
      Console.Out.Write($"{character.Name} the {character.Profession}: {check.ChallengeLevel}(+{check.Difficulty}) {check.Skill} Check: ({result.DiceRoll.Input.Skill}+{check.Difficulty}={modifiedSkill}): ");

      PrintRollResult(result.DiceRoll);
      return result.DiceRoll;
    }
    private static void PrintRollResult(SimpleResult result)
    {
      Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
      Console.Out.WriteLine("║                      ROLL RESULT                          ║");
      Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
      Console.Out.WriteLine($"║ Roll: {result.Roll}                                      ║");
      Console.Out.WriteLine($"║ Target: {result.Target}                                  ║");
      Console.Out.WriteLine($"║ Success Level: {result.SuccessLevel}                     ║");
      Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
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

  public class SimpleSkillCheck
  {
    public Skill Skill { get; set; }
    public ChallengeLevel ChallengeLevel { get; set; }
    public int Difficulty { get { return (int)ChallengeLevel; } }
    public CoreAttribute CoreAttribute { get; set; }

    public SimpleSkillCheck(Skill skill, ChallengeLevel challengeLevel)
    {
      Skill = skill;
      ChallengeLevel = challengeLevel;
      CoreAttribute = CharacterAttribute.GetCoreAttribute(skill);
    }
  }
}
}