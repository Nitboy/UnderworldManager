using System;
using System.Collections.Generic;
using UnderworldManager.Core.Models;
using UnderworldManager.Core.Business;

namespace UnderworldManager.Core.Business
{
    public class JobResult
    {
        public bool Success { get; set; }
        public int GoldEarned { get; set; }
        public int ThreatChange { get; set; }
        public int Casualties { get; set; }
    }

    public class DailyJobRunner
    {
        private readonly DiceRoller _diceRoller;
        private readonly ConflictEngine _conflictEngine;
        private readonly Character _character;
        private readonly Gang _gang;

        public DailyJobRunner(Character character, Gang gang)
        {
            _diceRoller = new DiceRoller();
            _conflictEngine = new ConflictEngine();
            _character = character;
            _gang = gang;
        }

        public JobResult SingleRun()
        {
            return RunJob(_character, _gang);
        }

        private JobResult RunJob(Character character, Gang gang)
        {
            var result = new JobResult();
            
            // Basic job success check
            var check = new SimpleAttributeCheck(CoreAttribute.Charisma, ChallengeLevel.Normal);
            var checkResult = _conflictEngine.Run(check, character);
            result.Success = checkResult.Success;

            if (result.Success)
            {
                // Successful job
                result.GoldEarned = _diceRoller.Roll(5, 10) * gang.Tier;
                result.ThreatChange = 0;
                result.Casualties = 0;
                
                // Update gang
                gang.Gold += result.GoldEarned;
            }
            else
            {
                // Failed job
                result.GoldEarned = _diceRoller.Roll(1, 3) * gang.Tier;
                result.ThreatChange = _diceRoller.Roll(0, 1);
                result.Casualties = 0;
                
                // Update gang
                gang.Gold += result.GoldEarned;
                gang.ThreatLevel += result.ThreatChange;
            }

            return result;
        }
    }
} 