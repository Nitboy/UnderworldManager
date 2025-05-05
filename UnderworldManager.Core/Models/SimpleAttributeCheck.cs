namespace UnderworldManager.Core.Models
{
    public class SimpleAttributeCheck
    {
        public CoreAttribute Attribute { get; set; }
        public ChallengeLevel ChallengeLevel { get; set; }
        public int Difficulty { get { return (int)ChallengeLevel; } }

        public SimpleAttributeCheck(CoreAttribute attribute, ChallengeLevel challengeLevel)
        {
            Attribute = attribute;
            ChallengeLevel = challengeLevel;
        }
    }
} 