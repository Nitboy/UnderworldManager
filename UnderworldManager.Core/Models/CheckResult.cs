namespace UnderworldManager.Core.Models
{
    public class CheckResult
    {
        public SimpleResult Result { get; set; }
        public string Description { get; set; }
        public SimpleResult DiceRoll => Result;
        public bool Success => Result.Success;

        public CheckResult(SimpleResult result, string description)
        {
            Result = result;
            Description = description;
        }

        public CheckResult(SimpleResult result, CharacterAttribute attribute, ChallengeLevel challengeLevel)
        {
            Result = result;
            Description = $"{attribute.Name} check against {challengeLevel}";
        }
    }
} 