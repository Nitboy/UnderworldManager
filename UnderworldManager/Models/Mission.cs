using System.Diagnostics.Contracts;
using UnderworldManager.Models;

namespace UnderworldManager.Models
{
  public class Mission
  {
    public List<SkillCheck> SkillChecks { get; set; }

    public Mission(MissionType missionType, int estimatedValue, int threatLevel, bool honorable)
    {
      EstimatedValue = estimatedValue;
      ThreatLevel = threatLevel;
      MissionType = missionType;
      Honorable = honorable;

      SkillChecks = MissionSkillChecks.GenerateSkillChecks(missionType);
      Description = MissionSkillChecks.GetMissionDescription(missionType);
      RequiredGear = MissionSkillChecks.GetRequiredGearByMissionType(missionType);
    }

    public MissionType MissionType { get; set; }
    public int EstimatedValue { get; set; }

    public bool Honorable { get; set; }
    public string Description { get; set; }

    public List<GearType> RequiredGear { get; set; }
    public int ThreatLevel { get; set; }
  }

  public class Challenge
  {
    public Skill Type { get; set; }
    public ChallengeLevel ChallengeTier { get; set; }
    public int Level { get { return (int)ChallengeTier; } }
    public ChallengeReward Reward { get; set; }
    public Dictionary<int, ChallengePenalty> Penalties { get; set; }
    public string Description { get; set; }
    public string SuccessText { get; set; }
    public string FailText { get; set; }

    public Challenge(Skill type, ChallengeLevel challengeTier, ChallengeReward reward, Dictionary<int, ChallengePenalty> penalties, string description, string successText, string failText)
    {
      Type = type;
      ChallengeTier = challengeTier;
      Reward = reward;
      Penalties = penalties;
      Description = description;
      SuccessText = successText;
      FailText = failText;
    }
  }
  public enum ChallengeLevel
  {
    Hard = 0,
    Medium = 20,
    Easy = 40
  }
  public enum ChallengePenalty
  {
    None = 0,
    Fail = 1,
    ThreatLevelIncrease = 2,
    Lessloot = 4,
    PenaltyToNextRoll = 8,
    AgentInjured = 16,
    AgentCaptured = 32,
    AgentMustMeleeThreat = 64, // Hit once
    MustEliminateThreat = 128, // Hit until injury or threat eliminated. (Must have toughness back?)
  }
  public enum ChallengeReward
  {
    ThreatLevelReduction,
    Extraloot,
    BonusOnNextRoll,
  }

  /// <summary>
  /// Stealing from wealthy individuals - f
  /// Robbing caravans - i
  /// Completing assassination contracts - i
  /// Selling stolen goods to fences - f
  /// Participating in illegal gambling - f
  /// Smuggling contraband(e.g.weapons, drugs, or other illegal goods) - f
  /// Running protection rackets - i
  /// Breaking into vaults or safes - f
  /// Counterfeiting currency or documents - f
  /// Kidnapping or holding hostages for ransom - i
  /// Infiltrating and stealing from other criminal organizations - f
  /// Providing "security" for businesses in exchange for payment - f
  /// Picking pockets or performing other forms of street-level theft - f
  /// Hijacking merchant ships or raiding coastal towns - i
  /// Blackmailing wealthy or powerful individuals - i
  /// Running a brothel or other illicit business - i
  /// Hacking into the accounts of wealthy merchants or nobles - f
  /// Impersonating officials or nobles to scam people out of money - f
  /// Forgery of official documents or royal decrees - f
  /// Sabotaging rival businesses or organizations for payment - i
  /// </summary>
  public enum MissionType
  {
    StealingFromWealthyIndividuals,
    RobbingCaravans,
    CompletingAssassinationContracts,
    SellingStolenGoods,
    ParticipatingInIllegalGambling,
    SmugglingContraband,
    RunningProtectionRackets,
    BreakingIntoVaults,
    CounterfeitingCurrency,
    Kidnapping,
    InfiltratingGangsAndStealing,
    ProvidingSecurity,
    PickingPockets,
    HijackingMerchantShips,
    Blackmailing,
    RunningABrothel,
    HackingIntoAccounts,
    ImpersonatingOfficials,
    ForgeryOfDocuments,
    SabotagingRivals
  }

  public class SkillCheck
  {
    public Skill Skill { get; set; }
    public ChallengeLevel ChallengeLevel { get; set; }
    public string Description { get; set; }
    public string SuccessText { get; set; }
    public string FailText { get; set; }
    public ChallengePenalty ChallengePenalty { get; set; }

    public int Difficulty { get { return (int)ChallengeLevel; } }
    public CoreAttribute CoreAttribute { get; set; }

    public SkillCheck(Skill skill, ChallengeLevel challengeLevel, string description, string successText, string failText, ChallengePenalty challengePenalty)
    {
      Skill = skill;
      ChallengeLevel = challengeLevel;
      Description = description;
      SuccessText = successText;
      FailText = failText;
      ChallengePenalty = challengePenalty;

      CoreAttribute = CharacterAttribute.GetCoreAttribute(skill);
    }
  }

  public static class MissionSkillChecks
  {
    public static List<SkillCheck> GenerateSkillChecks(MissionType missionType)
    {
      switch (missionType)
      {
        case MissionType.StealingFromWealthyIndividuals:
          return StealingFromWealthyIndividuals;
        case MissionType.RobbingCaravans:
          return MissionSkillChecks.RobbingCaravans;
        case MissionType.CompletingAssassinationContracts:
          return MissionSkillChecks.CompletingAssassinationContracts;
        case MissionType.SellingStolenGoods:
          return MissionSkillChecks.SellingStolenGoods;
        case MissionType.ParticipatingInIllegalGambling:
          return MissionSkillChecks.ParticipatingInIllegalGambling;
        case MissionType.SmugglingContraband:
          return MissionSkillChecks.SmugglingContraband;
        case MissionType.RunningProtectionRackets:
          return MissionSkillChecks.RunningProtectionRackets;
        case MissionType.BreakingIntoVaults:
          return MissionSkillChecks.BreakingIntoVaults;
        case MissionType.CounterfeitingCurrency:
          return MissionSkillChecks.CounterfeitingCurrency;
        case MissionType.Kidnapping:
          return MissionSkillChecks.Kidnapping;
        case MissionType.InfiltratingGangsAndStealing:
          return MissionSkillChecks.InfiltratingGangsAndStealing;
        case MissionType.ProvidingSecurity:
          return MissionSkillChecks.ProvidingSecurity;
        case MissionType.PickingPockets:
          return MissionSkillChecks.PickingPockets;
        case MissionType.HijackingMerchantShips:
          return MissionSkillChecks.HijackingMerchantShips;
        case MissionType.Blackmailing:
          return MissionSkillChecks.Blackmailing;
        case MissionType.RunningABrothel:
          return MissionSkillChecks.RunningABrothel;
        case MissionType.HackingIntoAccounts:
          return MissionSkillChecks.HackingIntoAccounts;
        case MissionType.ImpersonatingOfficials:
          return MissionSkillChecks.ImpersonatingOfficials;
        case MissionType.ForgeryOfDocuments:
          return MissionSkillChecks.ForgeryOfDocuments;
        case MissionType.SabotagingRivals:
          return MissionSkillChecks.SabotagingRivals;
      }
      throw new NotImplementedException();
    }
    public static bool IsHonorable(MissionType missionType)
    {
      switch (missionType)
      {
        case MissionType.StealingFromWealthyIndividuals:
          return true;
        case MissionType.RobbingCaravans:
          return false;
        case MissionType.CompletingAssassinationContracts:
          return false;
        case MissionType.SellingStolenGoods:
          return true;
        case MissionType.ParticipatingInIllegalGambling:
          return true;
        case MissionType.SmugglingContraband:
          return true;
        case MissionType.RunningProtectionRackets:
          return false;
        case MissionType.BreakingIntoVaults:
          return true;
        case MissionType.CounterfeitingCurrency:
          return true;
        case MissionType.Kidnapping:
          return false;
        case MissionType.InfiltratingGangsAndStealing:
          return true;
        case MissionType.ProvidingSecurity:
          return true;
        case MissionType.PickingPockets:
          return true;
        case MissionType.HijackingMerchantShips:
          return false;
        case MissionType.Blackmailing:
          return false;
        case MissionType.RunningABrothel:
          return false;
        case MissionType.HackingIntoAccounts:
          return true;
        case MissionType.ImpersonatingOfficials:
          return true;
        case MissionType.ForgeryOfDocuments:
          return true;
        case MissionType.SabotagingRivals:
          return false;
      }
      throw new NotImplementedException();
    }

    internal static string GetMissionDescription(MissionType missionType)
    {
      switch (missionType)
      {
        case MissionType.StealingFromWealthyIndividuals:
          return "Stealing from wealthy individuals";
        case MissionType.RobbingCaravans:
          return "Robbing caravans";
        case MissionType.CompletingAssassinationContracts:
          return "Completing assassination contracts";
        case MissionType.SellingStolenGoods:
          return "Selling stolen goods to fences";
        case MissionType.ParticipatingInIllegalGambling:
          return "Participating in illegal gambling";
        case MissionType.SmugglingContraband:
          return "Smuggling contraband(e.g.weapons, drugs, or other illegal goods)";
        case MissionType.RunningProtectionRackets:
          return "Running protection rackets";
        case MissionType.BreakingIntoVaults:
          return "Breaking into vaults or safes";
        case MissionType.CounterfeitingCurrency:
          return "Counterfeiting currency or documents";
        case MissionType.Kidnapping:
          return "Kidnapping or holding hostages for ransom";
        case MissionType.InfiltratingGangsAndStealing:
          return "Infiltrating and stealing from other criminal organizations";
        case MissionType.ProvidingSecurity:
          return "Providing \"security\" for businesses in exchange for payment";
        case MissionType.PickingPockets:
          return "Picking pockets or performing other forms of street-level theft";
        case MissionType.HijackingMerchantShips:
          return "Hijacking merchant ships";
        case MissionType.Blackmailing:
          return "Blackmailing wealthy or powerful individuals";
        case MissionType.RunningABrothel:
          return "Running a brothel or other illicit business";
        case MissionType.HackingIntoAccounts:
          return "Hacking into the accounts of wealthy merchants or nobles";
        case MissionType.ImpersonatingOfficials:
          return "Impersonating officials or nobles to scam people out of money";
        case MissionType.ForgeryOfDocuments:
          return "Forgery of official documents or royal decrees";
        case MissionType.SabotagingRivals:
          return "Sabotaging rival businesses or organizations for payment";
      }
      throw new NotImplementedException();

    }

    public static readonly List<SkillCheck> StealingFromWealthyIndividuals = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Stealth, ChallengeLevel.Medium, "Sneak past guards and security", "You quietly move past the guards and are not detected.", "You are spotted and forced to flee. The guards are now on high alert.", ChallengePenalty.ThreatLevelIncrease),
        new SkillCheck(Skill.Lockpicking, ChallengeLevel.Hard, "Pick the lock on the front door without being noticed", "You pick the lock without any problems and gain entry to the house.", "You trigger a trap while trying to pick the lock and are forced to flee.", ChallengePenalty.ThreatLevelIncrease | ChallengePenalty.PenaltyToNextRoll),
        new SkillCheck(Skill.Perception, ChallengeLevel.Easy, "Search for valuable items in the master bedroom", "You find a valuable item without any problems.", "You waste time searching and do not find anything valuable.", ChallengePenalty.Lessloot),
    };

    public static readonly List<SkillCheck> RobbingCaravans = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Perception, ChallengeLevel.Medium, "Scout the caravan to learn about their security", "You gather valuable information about the caravan's security measures.", "You don't learn anything useful and waste valuable time.", ChallengePenalty.ThreatLevelIncrease),
           new SkillCheck(Skill.Stealth, ChallengeLevel.Medium, "Sneak up to the caravan undetected", "You sneak up to the caravan without being detected.", "You are spotted by the guards and are forced to flee. The guards are now on high alert.", ChallengePenalty.ThreatLevelIncrease),
        new SkillCheck(Skill.Athletics, ChallengeLevel.Hard, "Jump onto the moving caravan", "You jump onto the caravan and land safely.", "You miss your jump and fall to the ground, alerting the guards.", ChallengePenalty.ThreatLevelIncrease | ChallengePenalty.AgentInjured),
        new SkillCheck(Skill.MeleeCombat, ChallengeLevel.Easy, "Subdue the guards without killing them", "You subdue the guards without killing them.", "You are forced to kill one of the guards to get away.", ChallengePenalty.MustEliminateThreat),
    };

    public static readonly List<SkillCheck> CompletingAssassinationContracts = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Stealth, ChallengeLevel.Medium, "Sneak into the target's residence undetected", "You enter the residence without being detected.", "You are detected and forced to flee, increasing the target's security.", ChallengePenalty.ThreatLevelIncrease),
        new SkillCheck(Skill.RangedCombat, ChallengeLevel.Hard, "Assassinate the target from a distance", "You take out the target from a distance without any problems.", "Your shot misses and alerts the target.", ChallengePenalty.AgentMustMeleeThreat | ChallengePenalty.ThreatLevelIncrease),
        new SkillCheck(Skill.MeleeCombat, ChallengeLevel.Easy, "Fend off any guards who come after you", "You successfully fend off any guards who come after you.", "You struggle in the fight and take damage.", ChallengePenalty.AgentInjured)
    };
    public static readonly List<SkillCheck> SellingStolenGoods = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Charm, ChallengeLevel.Medium, "Find a buyer for your stolen goods", "You find a buyer who is willing to pay a fair price for your stolen goods.", "You fail to find a buyer and have to lower the price.", ChallengePenalty.Lessloot),
        new SkillCheck(Skill.Deception, ChallengeLevel.Hard, "Convince the buyer the goods are legitimate", "You successfully convince the buyer that the goods are legitimate and get paid the full price.", "The buyer discovers that the goods are stolen and refuses to pay.", ChallengePenalty.Lessloot | ChallengePenalty.PenaltyToNextRoll),
        new SkillCheck(Skill.Stealth, ChallengeLevel.Easy, "Avoid getting caught by law enforcement", "You successfully avoid getting caught and escape without incident.", "You are spotted by law enforcement and have to flee the scene.", ChallengePenalty.ThreatLevelIncrease),
    };

    public static readonly List<SkillCheck> ParticipatingInIllegalGambling = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Deception, ChallengeLevel.Medium, "Bluff your way into the high-stakes game", "You successfully bluff your way into the high-stakes game and have a chance to win big.", "You fail to convince the guards to let you in and have to try a different approach.", ChallengePenalty.PenaltyToNextRoll),
        new SkillCheck(Skill.Perception, ChallengeLevel.Hard, "Read the other players' tells and gain an advantage", "You successfully read the other players' tells and are able to win big.", "You fail to read the other players' tells and end up losing your stake.", ChallengePenalty.Lessloot),
        new SkillCheck(Skill.Charm, ChallengeLevel.Easy, "Avoid suspicion and cash out your winnings", "You avoid arousing suspicion and successfully cash out your winnings.", "You arouse suspicion and have to leave the game empty-handed.", ChallengePenalty.ThreatLevelIncrease | ChallengePenalty.PenaltyToNextRoll),
    };

    public static readonly List<SkillCheck> SmugglingContraband = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Stealth, ChallengeLevel.Hard, "Sneak past border patrol and avoid detection", "You successfully sneak past the border patrol and bring the contraband to its destination.", "You are detected by the border patrol and have to fight your way out.", ChallengePenalty.AgentInjured | ChallengePenalty.MustEliminateThreat),
        new SkillCheck(Skill.Deception, ChallengeLevel.Medium, "Convince the buyer that the contraband is legitimate", "You successfully convince the buyer that the contraband is legitimate and get paid the full price.", "The buyer discovers that the contraband is illegal and refuses to pay.", ChallengePenalty.Lessloot),
        new SkillCheck(Skill.Charm, ChallengeLevel.Easy, "Avoid arousing suspicion and complete the transaction", "You avoid arousing suspicion and successfully complete the transaction.", "You arouse suspicion and have to flee the scene.", ChallengePenalty.ThreatLevelIncrease),
    };

    public static readonly List<SkillCheck> RunningProtectionRackets = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Intimidation, ChallengeLevel.Easy, "Convince the shopkeeper to pay protection money", "You successfully intimidate the shopkeeper into paying up.", "The shopkeeper refuses to pay and threatens to report you to the authorities.", ChallengePenalty.ThreatLevelIncrease | ChallengePenalty.MustEliminateThreat),
        new SkillCheck(Skill.Stealth, ChallengeLevel.Medium, "Avoid the attention of law enforcement", "You remain undetected by the police and other authorities.", "You are spotted by the police and forced to flee, increasing your threat level.", ChallengePenalty.ThreatLevelIncrease),
        new SkillCheck(Skill.Charm, ChallengeLevel.Hard, "Negotiate a favorable deal with the local gang leader", "You successfully negotiate a good deal with the gang leader.", "The gang leader refuses to make a deal with you and threatens you with violence.", ChallengePenalty.ThreatLevelIncrease | ChallengePenalty.AgentInjured),
    };

    public static readonly List<SkillCheck> BreakingIntoVaults = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Stealth, ChallengeLevel.Hard, "Sneak past guards to the vault", "You successfully sneak past the guards to the vault.", "You are spotted and forced to flee. The guards are now on high alert.", ChallengePenalty.ThreatLevelIncrease),
        new SkillCheck(Skill.Lockpicking, ChallengeLevel.Hard, "Pick the lock on the vault", "You successfully pick the lock on the vault and gain access.", "You trigger a trap while trying to pick the lock and are injured. You will need to seek medical attention before continuing your mission.", ChallengePenalty.AgentInjured | ChallengePenalty.Fail),
        new SkillCheck(Skill.Perception, ChallengeLevel.Medium, "Search for valuable items", "You find a valuable item without any problems.", "You waste time searching and do not find anything valuable.", ChallengePenalty.Lessloot),
    };

    public static readonly List<SkillCheck> CounterfeitingCurrency = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Alchemy, ChallengeLevel.Medium, "Mix the correct chemicals to create counterfeit money", "You successfully create a convincing batch of counterfeit money.", "Your attempt to create counterfeit money is unsuccessful and you waste valuable resources.", ChallengePenalty.Lessloot),
        new SkillCheck(Skill.Charm, ChallengeLevel.Hard, "Convince a local business to accept your counterfeit money", "You successfully convince a local business to accept your counterfeit money.", "The business owner realizes the money is fake and alerts the authorities.", ChallengePenalty.ThreatLevelIncrease),
        new SkillCheck(Skill.Deception, ChallengeLevel.Easy, "Disguise yourself as a courier and deliver the counterfeit money", "You successfully deliver the counterfeit money without being detected.", "Your disguise is discovered and you are forced to flee, increasing your threat level.", ChallengePenalty.ThreatLevelIncrease),
    };
    public static readonly List<SkillCheck> Kidnapping = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Stealth, ChallengeLevel.Medium, "Sneak past guards and security to get close to the target", "You quietly move past the guards and security and get close to the target.", "You are spotted and forced to flee. The guards are now on high alert.", ChallengePenalty.ThreatLevelIncrease),
        new SkillCheck(Skill.Intimidation, ChallengeLevel.Hard, "Intimidate the target to comply", "You intimidate the target into complying.", "The target resists and you must resort to force.", ChallengePenalty.AgentMustMeleeThreat),
        new SkillCheck(Skill.Athletics, ChallengeLevel.Easy, "Carry the target to safety", "You successfully carry the target to safety.", "You stumble and the target is injured in the fall.", ChallengePenalty.AgentInjured)
    };

    public static readonly List<SkillCheck> InfiltratingGangsAndStealing = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Deception, ChallengeLevel.Medium, "Infiltrate the gang by disguising yourself as one of their own", "You successfully infiltrate the gang and can move around unnoticed.", "Your disguise is not convincing enough, and you are quickly identified as an outsider. You need to flee before things escalate.", ChallengePenalty.ThreatLevelIncrease | ChallengePenalty.AgentInjured),
        new SkillCheck(Skill.SleightOfHand, ChallengeLevel.Hard, "Steal valuable items from the gang's headquarters without being detected", "You successfully steal the valuable items and leave the headquarters undetected.", "You are caught red-handed while stealing the items, and the gang is now on high alert.", ChallengePenalty.ThreatLevelIncrease | ChallengePenalty.AgentInjured | ChallengePenalty.MustEliminateThreat),
        new SkillCheck(Skill.Charm, ChallengeLevel.Medium, "Use charm to blend in with the gang members", "You successfully charm your way into the gang and are able to blend in with the members without arousing suspicion.", "The other gang members see through your ruse and become hostile. You are forced to fight your way out or flee.", ChallengePenalty.ThreatLevelIncrease | ChallengePenalty.AgentInjured | ChallengePenalty.AgentMustMeleeThreat)
    };

    public static readonly List<SkillCheck> ProvidingSecurity = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Perception, ChallengeLevel.Easy, "Spot any suspicious individuals or behavior", "You notice a suspicious individual and manage to defuse the situation before anything serious happens.", "You miss the suspicious individual, and they manage to carry out their plan, causing a major incident.", ChallengePenalty.ThreatLevelIncrease | ChallengePenalty.PenaltyToNextRoll),
        new SkillCheck(Skill.Intimidation, ChallengeLevel.Medium, "Scare off troublemakers and potential threats with a show of force", "Your show of force scares off troublemakers and potential threats.", "Your show of force backfires, and the troublemakers become more violent and aggressive.", ChallengePenalty.ThreatLevelIncrease | ChallengePenalty.AgentInjured | ChallengePenalty.MustEliminateThreat),
        new SkillCheck(Skill.MeleeCombat, ChallengeLevel.Hard, "Formulate a plan to protect the client's interests from an upcoming threat", "Your plan successfully protects the client's interests.", "Your plan fails to protect the client's interests, and they suffer a significant loss.", ChallengePenalty.ThreatLevelIncrease | ChallengePenalty.PenaltyToNextRoll),
    };

    public static readonly List<SkillCheck> PickingPockets = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Stealth, ChallengeLevel.Medium, "Blend in with the crowd and get close to the target", "You blend in with the crowd and get close to the target without being noticed.", "You fail to blend in with the crowd and the target becomes suspicious.", ChallengePenalty.AgentInjured),
        new SkillCheck(Skill.SleightOfHand, ChallengeLevel.Hard, "Slyly take the item from the target", "You manage to take the item from the target without them noticing.", "You are caught in the act and forced to flee.", ChallengePenalty.AgentInjured | ChallengePenalty.AgentCaptured),
        new SkillCheck(Skill.Acrobatics, ChallengeLevel.Easy, "Escape the area without being caught", "You successfully escape the area and avoid detection.", "You are pursued by the target or bystanders and must flee the area.", ChallengePenalty.AgentInjured | ChallengePenalty.AgentCaptured | ChallengePenalty.ThreatLevelIncrease),
    };

    public static readonly List<SkillCheck> HijackingMerchantShips = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Stealth, ChallengeLevel.Hard, "Sneak aboard the docked ship", "You successfully sneak aboard the ship without being detected.", "You are spotted and forced to fight your way off the ship.", ChallengePenalty.ThreatLevelIncrease),
        new SkillCheck(Skill.MeleeCombat, ChallengeLevel.Hard, "Subdue the guards", "You subdue the guards without any problems and gain access to the ship's cargo.", "You are overpowered and forced to flee. The guards are now on high alert.", ChallengePenalty.ThreatLevelIncrease | ChallengePenalty.PenaltyToNextRoll),
        new SkillCheck(Skill.Athletics, ChallengeLevel.Hard, "Haul the most valuable piece of loot out of the ship", "You are able to haul the most valuable piece of loot out of the ship without any problems.", "You struggle to move the loot and are forced to leave some behind.", ChallengePenalty.Lessloot),
    };

    public static readonly List<SkillCheck> Blackmailing = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Stealth, ChallengeLevel.Medium, "Sneak into the target's office and search for blackmail material", "You find incriminating documents and use them to blackmail the target into doing what you want.", "You are spotted and have to flee the premises. The target is now on high alert.", ChallengePenalty.ThreatLevelIncrease),
        new SkillCheck(Skill.Charm, ChallengeLevel.Easy, "Convince the target to meet with you", "You successfully convince the target to meet with you.", "The target does not believe you and refuses to meet with you.", ChallengePenalty.ThreatLevelIncrease),
        new SkillCheck(Skill.Intimidation, ChallengeLevel.Hard, "Threaten the target with exposure of their secrets", "You successfully intimidate the target and they give in to your demands.", "The target remains defiant and refuses to give in to your demands.", ChallengePenalty.ThreatLevelIncrease | ChallengePenalty.PenaltyToNextRoll | ChallengePenalty.AgentInjured)
    };

    public static readonly List<SkillCheck> RunningABrothel = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Charm, ChallengeLevel.Easy, "Attract customers to the brothel", "You successfully attract a steady stream of customers to the brothel.", "You are not successful in attracting customers and business is slow.", ChallengePenalty.Lessloot),
        new SkillCheck(Skill.Leadership, ChallengeLevel.Medium, "Manage the employees and ensure the brothel runs smoothly", "You successfully manage the employees and the brothel runs smoothly.", "The employees are unruly and difficult to manage. The brothel suffers as a result.", ChallengePenalty.Lessloot | ChallengePenalty.ThreatLevelIncrease),
        new SkillCheck(Skill.Intimidation, ChallengeLevel.Hard, "Intimidate a rival brothel owner into shutting down their establishment", "Your threats are convincing and the rival brothel owner agrees to shut down their establishment.", "The rival brothel owner sees through your intimidation and vows to retaliate.", ChallengePenalty.AgentCaptured | ChallengePenalty.ThreatLevelIncrease)
    };

    public static readonly List<SkillCheck> HackingIntoAccounts = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Deception, ChallengeLevel.Medium, "Disguise yourself as a wealthy merchant", "Your disguise fools the bank's clerks and they grant you access to the accounts.", "Your disguise is seen through and the bank's clerks alert the guards to your presence.", ChallengePenalty.ThreatLevelIncrease | ChallengePenalty.PenaltyToNextRoll),
        new SkillCheck(Skill.Forgery, ChallengeLevel.Hard, "Forge an official document to access the account", "Your forged document passes inspection and you gain access to the account.", "Your forgery is discovered and the guards are alerted.", ChallengePenalty.ThreatLevelIncrease | ChallengePenalty.PenaltyToNextRoll),
        new SkillCheck(Skill.Charm, ChallengeLevel.Easy, "Persuade the bank's clerk to give you access to the account", "Your persuasive words convince the clerk to grant you access to the account.", "Your attempts at persuasion fail and the clerk becomes suspicious of your intentions.", ChallengePenalty.PenaltyToNextRoll),
    };

    public static readonly List<SkillCheck> ImpersonatingOfficials = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Deception, ChallengeLevel.Medium, "Disguise yourself as an official to gain access to restricted areas", "Your disguise is convincing and you are able to access the restricted area.", "Your disguise is unconvincing and you are caught by security.", ChallengePenalty.AgentInjured | ChallengePenalty.ThreatLevelIncrease),
        new SkillCheck(Skill.Charm, ChallengeLevel.Hard, "Convince the guards that you are an official and have legitimate business in the restricted area", "Your fast-talking is successful and the guards let you through.", "The guards are suspicious of you and do not let you through.", ChallengePenalty.AgentInjured | ChallengePenalty.ThreatLevelIncrease),
        new SkillCheck(Skill.Lockpicking, ChallengeLevel.Easy, "Pick the lock on the door to the target's office", "You successfully pick the lock and gain access to the target's office.", "You trigger a trap while trying to pick the lock and are forced to flee.", ChallengePenalty.AgentInjured | ChallengePenalty.ThreatLevelIncrease),
    };

    public static readonly List<SkillCheck> ForgeryOfDocuments = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Deception, ChallengeLevel.Medium, "Disguise yourself as a legitimate document handler", "Your disguise is convincing and you are able to gain access to the documents you need.", "Your disguise fails to fool the guards and you are forced to flee.", ChallengePenalty.ThreatLevelIncrease),
        new SkillCheck(Skill.Forgery, ChallengeLevel.Hard, "Forge the necessary documents", "Your forgery is successful and the documents appear legitimate.", "Your forgery is discovered and the authorities are alerted to your deception.", ChallengePenalty.ThreatLevelIncrease | ChallengePenalty.PenaltyToNextRoll),
        new SkillCheck(Skill.Charm, ChallengeLevel.Easy, "Impersonate the legitimate document handler and pass off your forged documents", "Your performance is convincing and the documents are accepted without suspicion.", "Your performance is unconvincing and the authorities are alerted to your deception.", ChallengePenalty.ThreatLevelIncrease),
    };

    public static readonly List<SkillCheck> SabotagingRivals = new List<SkillCheck>()
    {
        new SkillCheck(Skill.Stealth, ChallengeLevel.Medium, "Sneak into your rival's headquarters undetected", "You successfully infiltrate your rival's headquarters without being detected.", "You are spotted and forced to flee. Your rival is now on high alert.", ChallengePenalty.ThreatLevelIncrease),
        new SkillCheck(Skill.Scheming, ChallengeLevel.Hard, "Sabotage your rival's operations without being caught", "Your sabotage is successful and your rival's operations are disrupted.", "Your sabotage fails and you are discovered. Your rival's operations are now even stronger.", ChallengePenalty.ThreatLevelIncrease | ChallengePenalty.PenaltyToNextRoll),
        new SkillCheck(Skill.Intimidation, ChallengeLevel.Easy, "Intimidate your rival's employees into leaving or switching sides", "Your intimidation tactics are successful and your rival's employees defect to your side or leave the organization.", "Your intimidation tactics fail and your rival's employees become even more loyal.", ChallengePenalty.ThreatLevelIncrease),
    };

    public static List<GearType> GetRequiredGearByMissionType(MissionType missionType)
    {
      return RequiredGear[missionType];
    }

    public static Dictionary<MissionType, List<GearType>> RequiredGear = new Dictionary<MissionType, List<GearType>>()
    {
      { MissionType.StealingFromWealthyIndividuals, new List<GearType>{ GearType.Lockpicks, GearType.DisguiseKit } },
      { MissionType.RobbingCaravans, new List<GearType>{ GearType.MeleeWeapons, GearType.Lockpicks } },
      { MissionType.CompletingAssassinationContracts, new List<GearType>{ GearType.Poison, GearType.Crossbow } },
      { MissionType.SellingStolenGoods, new List<GearType>{ GearType.StolenGoods, GearType.DistractionItems } },
      { MissionType.ParticipatingInIllegalGambling, new List<GearType>{ GearType.DistractionItems } },
      { MissionType.SmugglingContraband, new List<GearType>{ GearType.Contraband, GearType.Cart } },
      { MissionType.RunningProtectionRackets, new List<GearType>{ GearType.ThrowingKnives, GearType.MeleeWeapons } },
      { MissionType.BreakingIntoVaults, new List<GearType>{ GearType.Lockpicks, GearType.DisguiseKit } },
      { MissionType.CounterfeitingCurrency, new List<GearType>{ GearType.ForgeryTools, GearType.Smelter } },
      { MissionType.Kidnapping, new List<GearType>{ GearType.DisguiseKit, GearType.Rope } },
      { MissionType.InfiltratingGangsAndStealing, new List<GearType>{ GearType.Lockpicks, GearType.DisguiseKit } },
      { MissionType.ProvidingSecurity, new List<GearType>{ GearType.Crossbow, GearType.MeleeWeapons } },
      { MissionType.PickingPockets, new List<GearType>{ GearType.DisguiseKit, GearType.DistractionItems } },
      { MissionType.HijackingMerchantShips, new List<GearType>{ GearType.Crossbow, GearType.MeleeWeapons } },
      { MissionType.Blackmailing, new List<GearType>{ GearType.ForgeryTools, GearType.DisguiseKit } },
      { MissionType.RunningABrothel, new List<GearType>{ GearType.Brothel, GearType.Prostitutes } },
      { MissionType.HackingIntoAccounts, new List<GearType>{ GearType.ForgeryTools, GearType.DisguiseKit, GearType.NobleAtire } },
      { MissionType.ImpersonatingOfficials, new List<GearType>{ GearType.ForgeryTools, GearType.DisguiseKit, GearType.NobleAtire } },
      { MissionType.ForgeryOfDocuments, new List<GearType>{ GearType.ForgeryTools, GearType.DisguiseKit, GearType.NobleAtire } },
      { MissionType.SabotagingRivals, new List<GearType>{ GearType.Poison, GearType.DisguiseKit } }
    };
  }

  public struct SimpleSkillCheck
  {
    public Skill SkillToCheck { get; set; }
    public ChallengeLevel SkillChecks { get; set; }

    public SimpleSkillCheck(Skill skillToCheck, ChallengeLevel skillChecks)
    {
      SkillChecks = skillChecks;
      SkillToCheck = skillToCheck;
    }
  }

  public enum GearType
  {
    GrapplingHook,
    Lockpicks,
    SmokeBomb,
    Crossbow,
    Poison,
    DisguiseKit,
    Rope,
    ClimbingGear,
    ThrowingKnives,
    DistractionItems,
    ForgeryTools,
    Horse,
    MeleeWeapons,
    Smelter,
    Brothel,
    Prostitutes,
    NobleAtire,
    Contraband,
    Cart,
    StolenGoods,
    AlchemyKit,
  }
}
