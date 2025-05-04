using System.Collections.Generic;

namespace UnderworldManager.Models
{
    public class Challenge
    {
        public Skill Skill { get; set; }
        public ChallengeLevel ChallengeLevel { get; set; }
        public string Description { get; set; }
        public string SuccessText { get; set; }
        public string FailText { get; set; }
        public ChallengePenalty ChallengePenalty { get; set; }

        public int Difficulty { get { return (int)ChallengeLevel; } }
        public CoreAttribute CoreAttribute { get; set; }

        public Challenge(Skill skill, ChallengeLevel challengeLevel, string description, string successText, string failText, ChallengePenalty challengePenalty)
        {
            Skill = skill;
            ChallengeLevel = challengeLevel;
            Description = description;
            SuccessText = successText;
            FailText = failText;
            ChallengePenalty = challengePenalty;

            CoreAttribute = SkillUtils.GetCoreAttribute(skill);
        }
    }

    public enum ChallengeLevel
    {
        Hard = 0,
        Medium = 20,
        Easy = 40
    }

    public enum ChallengePenalty
    {
        None = 0,
        Fail = 1,
        ThreatLevelIncrease = 2,
        Lessloot = 4,
        PenaltyToNextRoll = 8,
        AgentInjured = 16,
        AgentCaptured = 32,
        AgentMustMeleeThreat = 64, // Hit once
        MustEliminateThreat = 128, // Hit until injury or threat eliminated. (Must have toughness back?)
    }

    public enum ChallengeReward
    {
        ThreatLevelReduction,
        Extraloot,
        BonusOnNextRoll,
    }
} 