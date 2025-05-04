namespace UnderworldManager.Models
{
    public class SkillCheck
    {
        public Skill Skill { get; set; }
        public ChallengeLevel ChallengeLevel { get; set; }
        public string Description { get; set; }
        public string SuccessText { get; set; }
        public string FailText { get; set; }
        public ChallengePenalty ChallengePenalty { get; set; }

        public int Difficulty { get { return (int)ChallengeLevel; } }
        public CoreAttribute CoreAttribute { get; set; }

        public SkillCheck(Skill skill, ChallengeLevel challengeLevel, string description, string successText, string failText, ChallengePenalty challengePenalty)
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
} 