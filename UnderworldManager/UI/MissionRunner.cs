using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnderworldManager.Business;
using UnderworldManager.Models;

namespace UnderworldManager
{
    public class MissionRunner
  {
    private MissionState _state;

    private Roster _roster;
    private Mission _mission;
    private ConflictEngine _conflictEngine;
    private List<SkillCheckResult> _challengeOutcomes = new List<SkillCheckResult>();
    private List<CharacterInjury> _agentsInjured = new List<CharacterInjury>();
    private List<CharacterCapture> _agentsCaptured = new List<CharacterCapture>();
    private int _threat;

    public MissionRunner(Roster roster, Mission mission, ConflictEngine conflictEngine)
    {
      _roster = roster;
      _mission = mission;
      _conflictEngine = conflictEngine;
      _threat = _mission.ThreatLevel;
    }

    public MissionResult Run()
    {
      Console.Out.WriteLine();
      Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
      Console.Out.WriteLine("║                    MISSION START                          ║");
      Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
      Console.Out.WriteLine("║ Sending your gang on a mission...                        ║");
      Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
      Console.Out.WriteLine($"║ Current Threat Level: {_threat}                          ║");
      Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
      Console.Out.WriteLine();

      _state = MissionState.Running;
      while (_state == MissionState.Running)
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
            _state = MissionState.AgentCaptured;
            break;
          }
        }

        if (_state == MissionState.Running)
        {
          Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
          Console.Out.WriteLine("║ Press Enter to continue or 'q' to quit...                ║");
          Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
          
          var input = Console.ReadLine();
          if (input?.ToLower() == "q")
          {
            _state = MissionState.Aborted;
          }
          else
          {
            _round++;
          }
        }
      }

      Console.Out.WriteLine();
      Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
      Console.Out.WriteLine("║                    MISSION RESULTS                        ║");
      Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
      Console.Out.WriteLine($"║ Total Earnings: {GetCurrentEarnings()} gold              ║");
      Console.Out.WriteLine($"║ Threat Increase: {_threat}                               ║");
      Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
      Console.Out.WriteLine("║ Press Enter to continue...                                ║");
      Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
      Console.ReadLine();

      return new MissionResult(GetCurrentEarnings(), _threat, _memberCritSuccess);
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

    private MissionResult GetMissionResult()
    {
      var successSum = _challengeOutcomes.Sum(x => x.DiceRoll.Successlevel);
      if(successSum >= 6 && _state == MissionState.Running) 
      {
        return MissionResult.AstonishingSuccess;
      }
      if (successSum >= 4 && _state == MissionState.Running)
      {
        return MissionResult.GreatSuccess;
      }
      if (successSum >= 2)
      {
        return MissionResult.GoodSuccess;
      }
      if (successSum >= 0)
      {
        return MissionResult.BasicSuccess;
      }
      if (successSum >= -2)
      {
        return MissionResult.FailedBut;
      }  
        
      return MissionResult.FailedAnd;      
    }

    private void HandlePenalty(int successLevel, ChallengePenalty challengePenalty)
    {
      //todo
      switch (challengePenalty)
      {
        case ChallengePenalty.Fail:
          _state = MissionState.MissionFailed;
          break;
        case ChallengePenalty.ThreatLevelIncrease:
          break;
        case ChallengePenalty.Lessloot:
          break;
        case ChallengePenalty.PenaltyToNextRoll:
          break;
        case ChallengePenalty.AgentInjured:
          break;
        case ChallengePenalty.AgentCaptured:
          break;
        case ChallengePenalty.AgentMustMeleeThreat:
          break;
        case ChallengePenalty.MustEliminateThreat:
          break;
      }

      switch (_state)
      {
        case MissionState.Running:
          _state = MissionState.ChallengeFailed;
          break;
        case MissionState.ChallengeFailed:
          _state = MissionState.MissionFailed;
          break;
        case MissionState.MissionFailed:
          break;
        default:
          break; 
      }
    }
  }
  public enum MissionState
  {
    Running,
    ChallengeFailed,
    MissionFailed
  }

  public enum MissionResult
  {
    BasicSuccess, // +0 SL total
    GoodSuccess, // +2 SL total
    GreatSuccess, // +4 SL total
    AstonishingSuccess, // +6 SL total
    FailedBut, // -2 SL total 
    FailedAnd, // -4 SL total
    MissionCutShort, // FailedBefore 
  }

  public record MissionOutcome(MissionResult Result, int LootModifier,  List<CharacterInjury> AgentsInjured, List<CharacterCapture> AgentsCaptured);
  public record CharacterCapture(Character Character, int Severity);
  public record CharacterInjury(Character Character, int Severity);
}