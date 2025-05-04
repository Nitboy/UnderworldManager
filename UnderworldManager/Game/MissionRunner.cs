using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnderworldManager.Models;
using UnderworldManager.Business;

namespace UnderworldManager.Game
{
    public class MissionRunner
    {
        private readonly Roster _roster;
        private readonly Mission _mission;
        private readonly ConflictEngine _conflictEngine;
        private List<Character> _memberCritSuccess = new List<Character>();
        private int _threat;

        public MissionRunner(Roster roster, Mission mission, ConflictEngine conflictEngine)
        {
            _roster = roster;
            _mission = mission;
            _conflictEngine = conflictEngine;
            _threat = mission.ThreatLevel;
        }

        public MissionResult RunMission(Character character, Mission mission)
        {
            var skillCheckResult = character.SkillCheck(mission.Skill, mission.Difficulty);
            if (skillCheckResult is SkillCheckResultWrapper simpleResult)
            {
                if (simpleResult.DiceRoll.Success)
                {
                    character.AddExperience(mission.ExperienceReward);
                    character.AddGold(mission.GoldReward);
                    character.AddReputation(mission.ReputationReward);
                    
                    if (simpleResult.DiceRoll.Successlevel >= 2)
                    {
                        _memberCritSuccess.Add(character);
                    }
                    
                    return new MissionResult(mission.GoldReward, _threat, _memberCritSuccess);
                }
                else
                {
                    _threat += 5;
                    return new MissionResult(0, _threat, _memberCritSuccess);
                }
            }
            else
            {
                return new MissionResult(0, _threat, _memberCritSuccess);
            }
        }
    }

    public record MissionResult(int Earnings, int Threat, List<Character> CritSuccess);
} 