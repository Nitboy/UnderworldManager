using UnderworldManager.Core.Models;

namespace UnderworldManager.Core.Models
{
    public class ProfessionInfo
    {
        public Profession Profession { get; set; }
        public Skill Skill { get; set; }
        public CoreAttribute PrimaryAttribute { get; set; }

        public ProfessionInfo(Profession profession, Skill skill, CoreAttribute primaryAttribute)
        {
            Profession = profession;
            Skill = skill;
            PrimaryAttribute = primaryAttribute;
        }

        public override string ToString()
        {
            return Profession.ToString();
        }
    }
} 