using System;
using UnderworldManager.Core.Models;

namespace UnderworldManager.Core.Business
{
    public class JobGenerator
    {
        private readonly Random _random;
        private readonly Gang _gang;
        private readonly int _gameRounds;
        private const int BaseMissionValue = 100;
        private const int GameRoundValueModifier = 20;
        private const int FameInfameValueModifier = 10;

        public JobGenerator(Gang gang, int gameRounds)
        {
            _random = new Random();
            _gang = gang;
            _gameRounds = gameRounds;
        }

        public List<Mission> GenerateAvailableMissions(int count = 3)
        {
            var missions = new List<Mission>();
            
            for (int i = 0; i < count; i++)
            {
                var missionType = (MissionType)_random.Next(Enum.GetNames(typeof(MissionType)).Length);
                var isHonorable = MissionSkillChecks.IsHonorable(missionType);
                var difficulty = MissionSkillChecks.GetDifficulty(missionType);
                var estimatedValue = CalculateMissionValue(isHonorable);
                var threatLevel = CalculateBaseThreatLevel(isHonorable);
                
                missions.Add(new Mission(missionType, estimatedValue, threatLevel, isHonorable));
            }

            return missions;
        }

        private int CalculateMissionValue(bool isHonorable)
        {
            if (isHonorable)
            {
                return BaseMissionValue +
                      GameRoundValueModifier * _gameRounds +
                      FameInfameValueModifier * _gang.Fame;
            }
            else
            {
                return BaseMissionValue +
                      GameRoundValueModifier * _gameRounds +
                      FameInfameValueModifier * _gang.Infamy;
            }
        }

        private int CalculateBaseThreatLevel(bool isHonorable)
        {
            if (isHonorable)
            {
                return _gang.Infamy / 10;
            }
            else
            {
                return 1 + (_gang.Infamy / 10);
            }
        }
    }
} 