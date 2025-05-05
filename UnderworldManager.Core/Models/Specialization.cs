using System;
using UnderworldManager.Core.Models;

namespace UnderworldManager.Models
{
    public class Specialization
    {
        public string Name { get; set; } = string.Empty;
        public Skill Skill { get; set; }
        public int Value { get; set; }
        public string Description { get; set; } = string.Empty;
        public string SuccessText { get; set; } = string.Empty;
        public string FailText { get; set; } = string.Empty;
        public ChallengePenalty ChallengePenalty { get; set; }

        public Specialization(Skill skill, int value)
        {
            Skill = skill;
            Value = value;
        }
    }
} 