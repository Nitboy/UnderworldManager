namespace UnderworldManager.Core.Models
{
    public static class SkillUtils
    {
        public static CoreAttribute GetCoreAttribute(Skill skill)
        {
            switch (skill)
            {
                case Skill.MeleeCombat:
                case Skill.Athletics:
                case Skill.Intimidation:
                    return CoreAttribute.Strength;
                case Skill.Stealth:
                case Skill.Lockpicking:
                case Skill.SleightOfHand:
                case Skill.Acrobatics:
                case Skill.RangedCombat:
                    return CoreAttribute.Agility;
                case Skill.Medicine:
                case Skill.Ambition:
                case Skill.Scheming:
                case Skill.Forgery:
                case Skill.Alchemy:
                case Skill.Perception:
                case Skill.Streetwise:
                    return CoreAttribute.Intelligence;
                case Skill.Deception:
                case Skill.Etiquette:
                case Skill.Charm:
                case Skill.Leadership:
                case Skill.Mercantile:
                case Skill.Looks:
                case Skill.Loyalty:
                    return CoreAttribute.Charisma;
                default:
                    throw new System.ArgumentException($"Unknown skill: {skill}");
            }
        }
    }
} 