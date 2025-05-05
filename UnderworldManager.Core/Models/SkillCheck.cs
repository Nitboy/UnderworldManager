using UnderworldManager.Core.Models;

namespace UnderworldManager.Core.Models
{
    public class SkillCheck
    {
        public Skill RequiredSkill { get; set; }
        public ChallengeLevel Difficulty { get; set; }
        public string Description { get; set; }
        public string SuccessText { get; set; }
        public string FailText { get; set; }
        public ChallengePenalty ChallengePenalty { get; set; }

        public int DifficultyValue { get { return (int)Difficulty; } }
        public CoreAttribute CoreAttribute { get; set; }

        public SkillCheck(Skill requiredSkill, ChallengeLevel difficulty, string description)
        {
            RequiredSkill = requiredSkill;
            Difficulty = difficulty;
            Description = description;

            CoreAttribute = SkillUtils.GetCoreAttribute(requiredSkill);
        }

        public SkillCheck(Challenge challenge)
        {
            RequiredSkill = challenge.RequiredSkill;
            Difficulty = challenge.Difficulty;
            Description = challenge.Description;

            CoreAttribute = SkillUtils.GetCoreAttribute(RequiredSkill);
        }
    }
} 