using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnderworldManager.Core.Models;
using UnderworldManager.Core.Business;

namespace UnderworldManager.Core.Business
{
    public class Game
    {
        public Character Player { get; private set; }
        public Gang Gang { get; private set; }
        public int GameRounds { get; private set; }
        public int WeekTimer { get; private set; }
        public int Threat { get; set; }

        private Dictionary<int, List<GangAction>> _gangActions;

        public Game(Character player, Gang gang)
        {
            Player = player;
            Gang = gang;
            WeekTimer = 0;
            GameRounds = 0;
            _gangActions = new Dictionary<int, List<GangAction>>();
            StartRound();
        }

        public void StartRound()
        {
            WeekTimer = 3;
            GameRounds++;
            Gang.ResetRoster();
        }

        public bool EndRound()
        {
            if (WeekTimer != 0)
            {
                return false;
            }
            Gang.AdvanceTime();
            StartRound();
            return true;
        }

        public bool SpendTime(GangAction gangAction)
        {
            if (WeekTimer > 0)
            {
                WeekTimer--;
                if (!_gangActions.ContainsKey(GameRounds))
                {
                    _gangActions.Add(GameRounds, new List<GangAction>());
                }
                _gangActions[GameRounds].Add(gangAction);
                return true;
            }
            return false;
        }

        public bool HasTimeLeft()
        {
            return WeekTimer > 0;
        }

        public LayLowResult LayLow()
        {
            var threatReduction = 5;
            Threat -= threatReduction;
            if (Threat < 0)
            {
                Threat = 0;
            }
            return new LayLowResult(threatReduction, 1);
        }

        internal void DailyJob()
        {
            // Implementation
        }

        private DailyJobRunner CreateDailyJobRunner(Character character)
        {
            return new DailyJobRunner(character, Gang);
        }

        public bool RunDailyJob()
        {
            if (!SpendTime(GangAction.HitTheStreets))
            {
                return false;
            }

            var totalEarnings = 0;
            var totalThreat = 0;
            var critSuccesses = new List<Character>();

            foreach (var character in Gang.Roster.Active)
            {
                var dailyJobRunner = CreateDailyJobRunner(character);
                var result = dailyJobRunner.SingleRun();
                totalEarnings += result.GoldEarned;
                totalThreat += result.ThreatChange;
                if (result.Success)
                {
                    critSuccesses.Add(character);
                }
            }

            Gang.Gold += totalEarnings;
            Threat = totalThreat;

            foreach (var member in critSuccesses)
            {
                member.AwardBonusXp(1);
            }

            return true;
        }

        public bool RunTraining(Character character, Skill skill)
        {
            if (!SpendTime(GangAction.Training))
            {
                return false;
            }

            var upgradeRoller = CreateUpgradeRoller();
            upgradeRoller.TrainingIncrease(character, skill, 5);
            return true;
        }

        private static CharUpgradeRoller CreateUpgradeRoller()
        {
            return new CharUpgradeRoller(new ConflictEngine());
        }
    }
} 