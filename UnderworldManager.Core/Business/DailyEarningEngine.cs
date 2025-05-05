using System;
using System.Collections.Generic;
using UnderworldManager.Core.Models;
using UnderworldManager.Core.Business;
using UnderworldManager.Models;

namespace UnderworldManager.Core.Business
{
  public class DailyEarningEngine
  {
    // daily earning switches between each archetype and generates a challenge.
    // Each success adds to the daily earning.
    // Fails can end up as direct conflicts or injuries.
    public int Earnings { get; set; }
    public Roster Roster { get; set; }

    public List<SkillCheckResult> ChallengeOutcomes = new List<SkillCheckResult>();

    public DailyEarningEngine(int earnings, Roster roster)
    {
      Earnings = earnings;
      Roster = roster;
    }
  }
}