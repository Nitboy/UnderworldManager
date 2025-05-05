using UnderworldManager.Core.Models;

namespace UnderworldManager.Core.Models
{
    public class Challenge
    {
        public Skill RequiredSkill { get; set; }
        public ChallengeLevel Difficulty { get; set; }
        public string Description { get; set; }

        public Challenge(Skill requiredSkill, ChallengeLevel difficulty, string description)
        {
            RequiredSkill = requiredSkill;
            Difficulty = difficulty;
            Description = description;
        }
    }
} 