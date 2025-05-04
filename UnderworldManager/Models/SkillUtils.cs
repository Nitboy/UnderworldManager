namespace UnderworldManager.Models
{
    public static class SkillUtils
    {
        public static CoreAttribute GetCoreAttribute(Skill skill)
        {
            switch (skill)
            {
                case Skill.Stealth:
                case Skill.SleightOfHand:
                case Skill.RangedCombat:
                case Skill.Lockpicking:
                case Skill.Acrobatics:
                    return CoreAttribute.Agility;
                case Skill.MeleeCombat:
                case Skill.Athletics:
                case Skill.Intimidation:
                    return CoreAttribute.Strength;
                case Skill.Etiquette:
                case Skill.Charm:
                case Skill.Leadership:
                case Skill.Mercantile:
                case Skill.Looks:
                case Skill.Loyalty:
                case Skill.Deception:
                    return CoreAttribute.Charisma;
                case Skill.Streetwise:
                case Skill.Perception:
                case Skill.Medicine:
                case Skill.Ambition:
                case Skill.Scheming:
                case Skill.Forgery:
                case Skill.Alchemy:
                    return CoreAttribute.Intelligence;
                default:
                    throw new NotImplementedException($"No core attribute defined for skill: {skill}");
            }
        }
    }
} 