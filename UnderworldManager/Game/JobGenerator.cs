using UnderworldManager.Models;

namespace UnderworldManager.Game
{
    public class JobGenerator
    {
        public Job GenerateJob(Character character)
        {
            // For now, return a simple job based on the character's profession
            return new Job
            {
                Skill = character.Profession?.Skill ?? Skill.Athletics, // Default to Athletics if no profession
                Difficulty = 1,
                ExperienceReward = 1,
                GoldReward = 10,
                ReputationReward = 1
            };
        }
    }

    public class Job
    {
        public Skill Skill { get; set; }
        public int Difficulty { get; set; }
        public int ExperienceReward { get; set; }
        public int GoldReward { get; set; }
        public int ReputationReward { get; set; }
    }
} 