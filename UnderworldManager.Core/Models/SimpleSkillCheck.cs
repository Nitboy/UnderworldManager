using UnderworldManager.Core.Models;

namespace UnderworldManager.Core.Models
{
    public class SimpleSkillCheck
    {
        public Skill Skill { get; set; }
        public ChallengeLevel ChallengeLevel { get; set; }
        public int Difficulty { get { return (int)ChallengeLevel; } }
        public CoreAttribute CoreAttribute { get; set; }

        public SimpleSkillCheck(Skill skill, ChallengeLevel challengeLevel)
        {
            Skill = skill;
            ChallengeLevel = challengeLevel;
            CoreAttribute = SkillUtils.GetCoreAttribute(skill);
        }
    }
} 