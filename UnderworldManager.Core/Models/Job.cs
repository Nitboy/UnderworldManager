using System;
using System.Collections.Generic;
using UnderworldManager.Core.Models;

namespace UnderworldManager.Models
{
    public class Job
    {
        public Skill Skill { get; set; }
        public int Difficulty { get; set; }
        public int ExperienceReward { get; set; }
        public int GoldReward { get; set; }
        public int ReputationReward { get; set; }
        public List<Challenge> Challenges { get; set; } = new List<Challenge>();
        public string Description { get; set; } = string.Empty;
        public string SuccessText { get; set; } = string.Empty;
        public string FailText { get; set; } = string.Empty;
        public ChallengePenalty ChallengePenalty { get; set; }
    }
} 