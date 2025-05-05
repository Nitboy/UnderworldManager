using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnderworldManager.Core.Models;

namespace UnderworldManager.Core.Models
{
  public class Mission
  {
    public MissionType Type { get; set; }
    public int EstimatedValue { get; set; }
    public int ThreatLevel { get; set; }
    public bool IsHonorable { get; set; }
    public List<Challenge> Challenges { get; set; } = new List<Challenge>();
    public Skill Skill { get; set; }
    public int Difficulty { get; set; }
    public int ExperienceReward { get; set; }
    public int GoldReward { get; set; }
    public int ReputationReward { get; set; }

    public Mission(MissionType type, int estimatedValue, int threatLevel, bool isHonorable)
    {
      Type = type;
      EstimatedValue = estimatedValue;
      ThreatLevel = threatLevel;
      IsHonorable = isHonorable;
      Challenges = new List<Challenge>();
      Skill = GetSkillForMissionType(type);
      Difficulty = GetDifficultyForMissionType(type);
      ExperienceReward = estimatedValue / 10;
      GoldReward = estimatedValue;
      ReputationReward = estimatedValue / 100;
    }

    private static Skill GetSkillForMissionType(MissionType type)
    {
      switch (type)
      {
        case MissionType.Assassination:
          return Skill.MeleeCombat;
        case MissionType.Burglary:
          return Skill.Stealth;
        case MissionType.Robbery:
          return Skill.Intimidation;
        case MissionType.Smuggling:
          return Skill.Deception;
        case MissionType.Protection:
          return Skill.Athletics;
        case MissionType.InformationGathering:
          return Skill.Perception;
        case MissionType.Sabotage:
          return Skill.Scheming;
        case MissionType.Kidnapping:
          return Skill.SleightOfHand;
        default:
          throw new ArgumentException($"Unknown mission type: {type}");
      }
    }

    private static int GetDifficultyForMissionType(MissionType type)
    {
      switch (type)
      {
        case MissionType.Assassination:
          return (int)ChallengeLevel.VeryHard;
        case MissionType.Burglary:
          return (int)ChallengeLevel.Hard;
        case MissionType.Robbery:
          return (int)ChallengeLevel.Normal;
        case MissionType.Smuggling:
          return (int)ChallengeLevel.Easy;
        case MissionType.Protection:
          return (int)ChallengeLevel.Normal;
        case MissionType.InformationGathering:
          return (int)ChallengeLevel.Easy;
        case MissionType.Sabotage:
          return (int)ChallengeLevel.Hard;
        case MissionType.Kidnapping:
          return (int)ChallengeLevel.VeryHard;
        default:
          throw new ArgumentException($"Unknown mission type: {type}");
      }
    }
  }
} 