namespace UnderworldManager.Core.Models
{
    public class ChallengePenalty
    {
        public int GoldPenalty { get; set; }
        public int ReputationPenalty { get; set; }
        public int InjuryTime { get; set; }
        public int JailTime { get; set; }

        public ChallengePenalty(int goldPenalty = 0, int reputationPenalty = 0, int injuryTime = 0, int jailTime = 0)
        {
            GoldPenalty = goldPenalty;
            ReputationPenalty = reputationPenalty;
            InjuryTime = injuryTime;
            JailTime = jailTime;
        }
    }
} 