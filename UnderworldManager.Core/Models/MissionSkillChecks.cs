using System;
using System.Collections.Generic;

namespace UnderworldManager.Core.Models
{
    public static class MissionSkillChecks
    {
        public static Skill GetSkill(MissionType missionType)
        {
            switch (missionType)
            {
                case MissionType.StealingFromWealthyIndividuals:
                case MissionType.RobbingCaravans:
                case MissionType.BreakingIntoVaults:
                case MissionType.PickingPockets:
                    return Skill.Stealth;
                case MissionType.CompletingAssassinationContracts:
                case MissionType.Kidnapping:
                case MissionType.HijackingMerchantShips:
                case MissionType.SabotagingRivals:
                    return Skill.MeleeCombat;
                case MissionType.SellingStolenGoods:
                case MissionType.ParticipatingInIllegalGambling:
                case MissionType.RunningProtectionRackets:
                case MissionType.RunningABrothel:
                case MissionType.Blackmailing:
                    return Skill.Deception;
                case MissionType.SmugglingContraband:
                case MissionType.CounterfeitingCurrency:
                case MissionType.HackingIntoAccounts:
                case MissionType.ImpersonatingOfficials:
                case MissionType.ForgeryOfDocuments:
                    return Skill.Forgery;
                case MissionType.InfiltratingGangsAndStealing:
                case MissionType.ProvidingSecurity:
                    return Skill.Streetwise;
                default:
                    throw new NotImplementedException($"No skill defined for mission type: {missionType}");
            }
        }

        public static int GetDifficulty(MissionType missionType)
        {
            switch (missionType)
            {
                case MissionType.StealingFromWealthyIndividuals:
                case MissionType.RobbingCaravans:
                case MissionType.CompletingAssassinationContracts:
                case MissionType.BreakingIntoVaults:
                case MissionType.Kidnapping:
                case MissionType.HijackingMerchantShips:
                case MissionType.SabotagingRivals:
                    return 40; // Hard missions
                case MissionType.SellingStolenGoods:
                case MissionType.SmugglingContraband:
                case MissionType.CounterfeitingCurrency:
                case MissionType.InfiltratingGangsAndStealing:
                case MissionType.HackingIntoAccounts:
                case MissionType.ImpersonatingOfficials:
                case MissionType.ForgeryOfDocuments:
                    return 20; // Medium missions
                case MissionType.ParticipatingInIllegalGambling:
                case MissionType.RunningProtectionRackets:
                case MissionType.ProvidingSecurity:
                case MissionType.PickingPockets:
                    return 0; // Easy missions
                default:
                    throw new NotImplementedException($"No difficulty defined for mission type: {missionType}");
            }
        }

        public static bool IsHonorable(MissionType missionType)
        {
            switch (missionType)
            {
                case MissionType.StealingFromWealthyIndividuals:
                case MissionType.RobbingCaravans:
                case MissionType.CompletingAssassinationContracts:
                case MissionType.RunningProtectionRackets:
                case MissionType.Kidnapping:
                case MissionType.HijackingMerchantShips:
                case MissionType.Blackmailing:
                case MissionType.RunningABrothel:
                case MissionType.SabotagingRivals:
                    return false;
                case MissionType.SellingStolenGoods:
                case MissionType.ParticipatingInIllegalGambling:
                case MissionType.SmugglingContraband:
                case MissionType.BreakingIntoVaults:
                case MissionType.CounterfeitingCurrency:
                case MissionType.InfiltratingGangsAndStealing:
                case MissionType.ProvidingSecurity:
                case MissionType.PickingPockets:
                case MissionType.HackingIntoAccounts:
                case MissionType.ImpersonatingOfficials:
                case MissionType.ForgeryOfDocuments:
                    return true;
                default:
                    throw new NotImplementedException($"No honor status defined for mission type: {missionType}");
            }
        }

        public static string GetMissionDescription(MissionType missionType)
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
                    return "Smuggling contraband (e.g. weapons, drugs, or other illegal goods)";
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
                    return "Hijacking merchant ships or raiding coastal towns";
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
                default:
                    throw new NotImplementedException($"No description defined for mission type: {missionType}");
            }
        }

        public static List<GearType> GetRequiredGearByMissionType(MissionType missionType)
        {
            return RequiredGear[missionType];
        }

        public static Dictionary<MissionType, List<GearType>> RequiredGear = new Dictionary<MissionType, List<GearType>>()
        {
            { MissionType.StealingFromWealthyIndividuals, new List<GearType> { GearType.GrapplingHook, GearType.Lockpicks, GearType.SmokeBomb } },
            { MissionType.RobbingCaravans, new List<GearType> { GearType.Crossbow, GearType.Poison, GearType.Rope } },
            { MissionType.CompletingAssassinationContracts, new List<GearType> { GearType.Crossbow, GearType.Poison, GearType.DisguiseKit } },
            { MissionType.SellingStolenGoods, new List<GearType> { GearType.StolenGoods, GearType.Cart } },
            { MissionType.ParticipatingInIllegalGambling, new List<GearType> { GearType.DistractionItems } },
            { MissionType.SmugglingContraband, new List<GearType> { GearType.Contraband, GearType.Cart } },
            { MissionType.RunningProtectionRackets, new List<GearType> { GearType.MeleeWeapons } },
            { MissionType.BreakingIntoVaults, new List<GearType> { GearType.Lockpicks, GearType.SmokeBomb } },
            { MissionType.CounterfeitingCurrency, new List<GearType> { GearType.ForgeryTools } },
            { MissionType.Kidnapping, new List<GearType> { GearType.Rope, GearType.SmokeBomb } },
            { MissionType.InfiltratingGangsAndStealing, new List<GearType> { GearType.DisguiseKit, GearType.SmokeBomb } },
            { MissionType.ProvidingSecurity, new List<GearType> { GearType.MeleeWeapons } },
            { MissionType.PickingPockets, new List<GearType> { GearType.DistractionItems } },
            { MissionType.HijackingMerchantShips, new List<GearType> { GearType.Crossbow, GearType.Rope } },
            { MissionType.Blackmailing, new List<GearType> { GearType.DisguiseKit } },
            { MissionType.RunningABrothel, new List<GearType> { GearType.Brothel, GearType.Prostitutes } },
            { MissionType.HackingIntoAccounts, new List<GearType> { GearType.ForgeryTools } },
            { MissionType.ImpersonatingOfficials, new List<GearType> { GearType.NobleAtire, GearType.ForgeryTools } },
            { MissionType.ForgeryOfDocuments, new List<GearType> { GearType.ForgeryTools } },
            { MissionType.SabotagingRivals, new List<GearType> { GearType.Poison, GearType.SmokeBomb } }
        };
    }
} 