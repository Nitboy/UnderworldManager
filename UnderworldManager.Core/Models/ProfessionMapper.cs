using UnderworldManager.Core.Models;

namespace UnderworldManager.Core.Models
{
  public static class ProfessionMapper
  {
    public static bool IsBasicProfession(Profession profession)
    {
      switch (profession)
      {
        case Profession.Brawler:
        case Profession.Runner:
        case Profession.Apprentice:
        case Profession.Grifter:
          return true;
        case Profession.Enforcer:
        case Profession.Scout:
        case Profession.Intimidator:
        case Profession.ConArtist:
        case Profession.Lookout:
        case Profession.QuackDoctor:
        case Profession.Mastermind:
        case Profession.Plotter:
        case Profession.Forger:
        case Profession.Poisoner:
        case Profession.Sniper:
        case Profession.Pickpocket:
        case Profession.Infiltrator:
        case Profession.Locksmith:
        case Profession.CatBurglar:
        case Profession.Diplomat:
        case Profession.Charm:
        case Profession.Fence:
        case Profession.Seducer:
        case Profession.Quartermaster:
        case Profession.MasterOfDisguise:
        case Profession.Lieutenant:
          return false;
        default:
          throw new ArgumentException("Invalid profession type");
      }
    }

    public static ProfessionInfo FindProfessionByAttribute(CoreAttribute attribute)
    {
      var profession = GetBasicProfessionByAttribute(attribute);
      return new ProfessionInfo(profession, FindSkillByProfession(profession), attribute);
    }

    private static Profession GetBasicProfessionByAttribute(CoreAttribute attribute)
    {
      switch (attribute)
      {
        case CoreAttribute.Strength:
          return Profession.Brawler;
        case CoreAttribute.Agility:
          return Profession.Runner;
        case CoreAttribute.Intelligence:
          return Profession.Apprentice;
        case CoreAttribute.Charisma:
          return Profession.Grifter;
        default:
          throw new ArgumentException("Invalid attribute type");
      }
    }

    public static ProfessionInfo FindProfessionBySkill(Skill skill)
    {
      var profession = GetProfessionBySkill(skill);
      return new ProfessionInfo(profession, skill, SkillUtils.GetCoreAttribute(skill));
    }

    private static Profession GetProfessionBySkill(Skill skill)
    {
      switch (skill)
      {
        case Skill.MeleeCombat:
          return Profession.Enforcer;
        case Skill.Athletics:
          return Profession.Scout;
        case Skill.Intimidation:
          return Profession.Intimidator;
        case Skill.Streetwise:
          return Profession.ConArtist;
        case Skill.Perception:
          return Profession.Lookout;
        case Skill.Medicine:
          return Profession.QuackDoctor;
        case Skill.Ambition:
          return Profession.Mastermind;
        case Skill.Scheming:
          return Profession.Plotter;
        case Skill.Forgery:
          return Profession.Forger;
        case Skill.Alchemy:
          return Profession.Poisoner;
        case Skill.RangedCombat:
          return Profession.Sniper;
        case Skill.SleightOfHand:
          return Profession.Pickpocket;
        case Skill.Stealth:
          return Profession.Infiltrator;
        case Skill.Lockpicking:
          return Profession.Locksmith;
        case Skill.Acrobatics:
          return Profession.CatBurglar;
        case Skill.Etiquette:
          return Profession.Diplomat;
        case Skill.Charm:
          return Profession.ConArtist;
        case Skill.Leadership:
          return Profession.Lieutenant;
        case Skill.Mercantile:
          return Profession.Fence;
        case Skill.Looks:
          return Profession.Seducer;
        case Skill.Loyalty:
          return Profession.Quartermaster;
        case Skill.Deception:
          return Profession.MasterOfDisguise;
        default:
          throw new ArgumentException("Invalid skill");
      }
    }

    public static CoreAttribute FindCoreAttributeByProfession(Profession profession)
    {
      switch (profession)
      {
        case Profession.Brawler:
          return CoreAttribute.Strength;
        case Profession.Runner:
          return CoreAttribute.Agility;
        case Profession.Apprentice:
          return CoreAttribute.Intelligence;
        case Profession.Grifter:
          return CoreAttribute.Charisma;
        default:
          var skill = FindSkillByProfession(profession);
          return SkillUtils.GetCoreAttribute(skill);
      }
    }

    public static Skill FindSkillByProfession(Profession profession)
    {
      switch (profession)
      {
        case Profession.Brawler:
          return Skill.MeleeCombat;
        case Profession.Runner:
          return Skill.Athletics;
        case Profession.Apprentice:
          return Skill.Alchemy;
        case Profession.Grifter:
          return Skill.Streetwise;
        case Profession.Enforcer:
          return Skill.MeleeCombat;
        case Profession.Scout:
          return Skill.Athletics;
        case Profession.Intimidator:
          return Skill.Intimidation;
        case Profession.ConArtist:
          return Skill.Streetwise;
        case Profession.Lookout:
          return Skill.Perception;
        case Profession.QuackDoctor:
          return Skill.Medicine;
        case Profession.Mastermind:
          return Skill.Ambition;
        case Profession.Plotter:
          return Skill.Scheming;
        case Profession.Forger:
          return Skill.Forgery;
        case Profession.Poisoner:
          return Skill.Alchemy;
        case Profession.Sniper:
          return Skill.RangedCombat;
        case Profession.Pickpocket:
          return Skill.SleightOfHand;
        case Profession.Infiltrator:
          return Skill.Stealth;
        case Profession.Locksmith:
          return Skill.Lockpicking;
        case Profession.CatBurglar:
          return Skill.Acrobatics;
        case Profession.Diplomat:
          return Skill.Etiquette;
        case Profession.Charm:
          return Skill.Charm;
        case Profession.Fence:
          return Skill.Mercantile;
        case Profession.Seducer:
          return Skill.Looks;
        case Profession.Quartermaster:
          return Skill.Loyalty;
        case Profession.MasterOfDisguise:
          return Skill.Deception;
        case Profession.Lieutenant:
          return Skill.Leadership;
        default:
          throw new ArgumentException("Invalid profession type");
      }
    }

    public static string GetDescription(Profession job)
    {
      switch (job)
      {
        case Profession.Brawler:
          return "A brawler is a new member who relies on their raw physical strength to get by. They may not be trained in any particular weapon or fighting style, but they are tough and not afraid to get their hands dirty.";
        case Profession.Runner:
          return "A runner is a new member who is fast and agile, able to dart in and out of tight spaces and navigate difficult terrain. They may not have any specific training in combat or thievery, but they are quick on their feet and can get the job done.";
        case Profession.Apprentice:
          return "An apprentice is a new member who is eager to learn and develop their skills. They may not have any specific skills yet, but they are intelligent and eager to prove themselves to the gang.";
        case Profession.Grifter:
          return "A grifter is a new member who has a way with words and a talent for deception. They may not have any specific training in thievery or combat, but they are charismatic and able to talk their way out of tricky situations.";
        case Profession.Enforcer:
          return "The Enforcer - This profession specializes in using physical force to intimidate or eliminate rivals, protect the gang's interests, and enforce the gang's rules. They are typically skilled in hand-to-hand combat and the use of melee weapons.";
        case Profession.Scout:
          return "The Scout - This profession is responsible for reconnaissance, surveillance, and gathering intelligence on potential targets or threats. They rely on their physical agility and endurance to navigate difficult terrain and evade detection.";
        case Profession.Intimidator:
          return "The Intimidator - This profession is a master of psychological warfare, using their imposing presence, verbal threats, and physical violence to intimidate rivals, coerce informants, and extract information.";
        case Profession.ConArtist:
          return "The Con Artist - This profession is a skilled manipulator who can blend into the crowd and gain the trust of unsuspecting marks. They specialize in confidence schemes, identity theft, and other forms of fraud.";
        case Profession.Lookout:
          return "The Lookout - This profession is responsible for keeping watch and detecting any potential threats or opportunities. They have keen senses and are skilled at spotting potential dangers or valuable targets.";
        case Profession.QuackDoctor:
          return "The Quack Doctor - This profession is a skilled healer who can treat wounds and injuries suffered by gang members. They may also use their knowledge of medicine to create poisons or other harmful substances.";
        case Profession.Mastermind:
          return "The Mastermind - This profession is the strategic mastermind behind the gang's operations, planning heists, assigning roles to gang members, and making important decisions. They are driven by their ambition to gain power and wealth.";
        case Profession.Plotter:
          return "The Plotter - This profession is a master of deception and subterfuge, using their cleverness and guile to manipulate rivals and allies alike. They may specialize in creating false identities, forging documents, or planting false evidence.";
        case Profession.Forger:
          return "The Forger - This profession is a skilled craftsman who can create high-quality forgeries of documents or currency. They are often used to create false identities or launder money.";
        case Profession.Poisoner:
          return "The Poisoner - This profession is skilled in the creation of deadly poisons and other harmful substances. They may be hired to eliminate rivals or create a competitive advantage for the gang.";
        case Profession.Sniper:
          return "The Sniper - This profession is a skilled marksman who can take out targets from a distance. They may specialize in using bows, crossbows, or other ranged weapons.";
        case Profession.Pickpocket:
          return "The Pickpocket - This profession is a skilled thief who can steal valuable items from unsuspecting targets without being detected. They may specialize in pickpocketing, lockpicking, or other forms of theft.";
        case Profession.Infiltrator:
          return "The Infiltrator - This profession is a master of stealth and can move quietly and undetected through enemy territory. They may be used to plant evidence, gather intelligence, or eliminate key targets.";
        case Profession.Locksmith:
          return "The Locksmith - This profession is a skilled craftsman who can pick locks and bypass security measures. They may be used to gain access to valuable targets or break into secure locations.";
        case Profession.CatBurglar:
          return "The Cat Burglar - This profession is a skilled thief who can climb walls, jump between rooftops, and perform other acrobatic feats to gain access to high-value targets.";
        case Profession.Diplomat:
          return "Diplomat - A smooth-talking negotiator who can navigate the complex social hierarchies of the criminal underworld. They are responsible for brokering deals and alliances with other gangs and influential figures.";
        case Profession.Charm:
          return "Con Artist - A charismatic trickster who can talk their way out of any situation. They are responsible for recruiting new members, gaining the trust of potential victims, and convincing them to part with their valuables.";
        case Profession.Fence:
          return "Fence - A skilled trader who specializes in buying and selling stolen goods. They are responsible for finding buyers for the gang's loot and negotiating favorable prices.";
        case Profession.Seducer:
          return "Seducer - A master of manipulation who uses their charm and beauty to gain access to valuable information or targets. They may be used to gather intelligence, create distractions, or eliminate key targets.";
        case Profession.Quartermaster:
          return "Quartermaster - A trusted member who is responsible for managing the gang's resources and supplies. They ensure that gang members have the equipment and resources they need to carry out their missions.";
        case Profession.MasterOfDisguise:
          return "Master of Disguise - A skilled infiltrator who can blend in with any crowd and assume any identity. They may be used to gather intelligence, plant evidence, or eliminate key targets.";
        case Profession.Lieutenant:
          return "Lieutenant - A trusted member who is responsible for leading and coordinating gang operations. They ensure that gang members work together effectively and follow the gang's rules and objectives.";
        default:
          throw new ArgumentException("Invalid profession type");
      }
    }
  }
}