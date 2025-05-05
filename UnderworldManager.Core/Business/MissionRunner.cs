using System;
using UnderworldManager.Core.Models;
using UnderworldManager.Core.Business;

namespace UnderworldManager.Core.Business
{
    public class MissionResult
    {
        public bool Success { get; set; }
        public int GoldEarned { get; set; }
        public int ThreatChange { get; set; }
        public int Casualties { get; set; }
    }

    public class MissionRunner
    {
        private readonly DiceRoller _diceRoller;
        private readonly ConflictEngine _conflictEngine;
        private readonly Roster _roster;
        private readonly Mission _mission;

        public MissionRunner(Roster roster, Mission mission, ConflictEngine conflictEngine)
        {
            _roster = roster;
            _mission = mission;
            _diceRoller = new DiceRoller();
            _conflictEngine = conflictEngine;
        }

        public MissionResult Run()
        {
            var result = new MissionResult();
            
            // Basic mission success check
            var check = new SimpleAttributeCheck(CoreAttribute.Charisma, ChallengeLevel.Normal);
            var checkResult = _conflictEngine.Run(check, _roster.Active[0]); // Use the first active character for now
            result.Success = checkResult.Success;

            if (result.Success)
            {
                // Successful mission
                result.GoldEarned = _mission.EstimatedValue;
                result.ThreatChange = 0;
                result.Casualties = 0;
            }
            else
            {
                // Failed mission
                result.GoldEarned = _mission.EstimatedValue / 2;
                result.ThreatChange = _diceRoller.Roll(1, 3);
                result.Casualties = _diceRoller.Roll(0, 1);
            }

            return result;
        }
    }
} 