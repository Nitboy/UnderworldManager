using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnderworldManager.Models;

namespace UnderworldManager.Models
{
  public class Mission
  {
    public Skill Skill { get; set; }
    public int Difficulty { get; set; }
    public int ExperienceReward { get; set; }
    public int GoldReward { get; set; }
    public int ReputationReward { get; set; }
    public bool IsHonorable { get; set; }
    public int ThreatLevel { get; set; }
    public int EstimatedValue { get; set; }

    public Mission(MissionType type, int estimatedValue, int threatLevel, bool isHonorable)
    {
      EstimatedValue = estimatedValue;
      ThreatLevel = threatLevel;
      IsHonorable = isHonorable;
      Skill = MissionSkillChecks.GetSkill(type);
      Difficulty = MissionSkillChecks.GetDifficulty(type);
      ExperienceReward = estimatedValue / 10;
      GoldReward = estimatedValue;
      ReputationReward = estimatedValue / 20;
    }
  }
} 