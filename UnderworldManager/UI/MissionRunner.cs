using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnderworldManager.Models;
using UnderworldManager.Business;

namespace UnderworldManager
{
    public class MissionRunner
    {
        private MissionState _state;
        private int _round = 1;
        private Gang _gang;
        private List<Character> _memberCritSuccess = new List<Character>();
        private Roster _roster;
        private Mission _mission;
        private ConflictEngine _conflictEngine;
        private List<SkillCheckResult> _challengeOutcomes = new List<SkillCheckResult>();
        private List<CharacterInjury> _agentsInjured = new List<CharacterInjury>();
        private List<CharacterCapture> _agentsCaptured = new List<CharacterCapture>();
        private int _threat;

        public MissionRunner(Roster roster, Mission mission, ConflictEngine conflictEngine)
        {
            _roster = roster;
            _mission = mission;
            _conflictEngine = conflictEngine;
            _threat = _mission.ThreatLevel;
            _gang = new Gang("Temporary Gang", 1, 0, roster.Active, roster.Active.First());
        }

        public MissionResult Run()
        {
            Console.Out.WriteLine();
            Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.Out.WriteLine("║                    MISSION START                          ║");
            Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.Out.WriteLine("║ Sending your gang on a mission...                        ║");
            Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.Out.WriteLine($"║ Current Threat Level: {_threat}                          ║");
            Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.Out.WriteLine();

            _state = MissionState.Running;
            while (_state == MissionState.Running)
            {
                Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
                Console.Out.WriteLine($"║                      ROUND {_round}                      ║");
                Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");

                foreach (var member in _gang.Roster.Active)
                {
                    Console.Out.WriteLine($"║ {member.Name} is working...                           ║");
                    if (member.Profession == null)
                    {
                        Console.Out.WriteLine($"║ {member.Name} has no profession and cannot work     ║");
                        continue;
                    }
                    var result = HandleEventsOfResult(member, DoChallenge(member, new SimpleSkillCheck(member.Profession.Skill, SetChallengeLevel(_threat))));
                    if (!result)
                    {
                        _state = MissionState.AgentCaptured;
                        break;
                    }
                }

                if (_state == MissionState.Running)
                {
                    Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
                    Console.Out.WriteLine("║ Press Enter to continue or 'q' to quit...                ║");
                    Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
                    
                    var input = Console.ReadLine();
                    if (input?.ToLower() == "q")
                    {
                        _state = MissionState.Aborted;
                    }
                    else
                    {
                        _round++;
                    }
                }
            }

            Console.Out.WriteLine();
            Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.Out.WriteLine("║                    MISSION RESULTS                        ║");
            Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.Out.WriteLine($"║ Total Earnings: {GetCurrentEarnings()} gold              ║");
            Console.Out.WriteLine($"║ Threat Increase: {_threat}                               ║");
            Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.Out.WriteLine("║ Press Enter to continue...                                ║");
            Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.ReadLine();

            return new MissionResult(GetCurrentEarnings(), _threat, _memberCritSuccess);
        }

        private static void PrintRollResult(SimpleResult result)
        {
            Console.Out.WriteLine($"Roll: {result.Roll} Target: {result.TestedSkillValue} Success Level: {result.Successlevel}");
        }

        private bool HandleEventsOfResult(Character member, SimpleResult result)
        {
            if (result.Successlevel >= 2)
            {
                Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
                Console.Out.WriteLine("║                    CRITICAL SUCCESS                       ║");
                Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
                Console.Out.WriteLine($"║ {member.Name} has achieved a critical success!           ║");
                Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
                _memberCritSuccess.Add(member);
            }
            else if (result.Successlevel <= -2)
            {
                Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
                Console.Out.WriteLine("║                    CRITICAL FAILURE                       ║");
                Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
                Console.Out.WriteLine($"║ {member.Name} has attracted unwanted attention!          ║");
                Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
                
                if (!AvoidCapture(member, SetChallengeLevel(_threat)))
                {
                    Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
                    Console.Out.WriteLine("║                    AGENT CAPTURED                        ║");
                    Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
                    Console.Out.WriteLine($"║ {member.Name} has been captured by the city watch!      ║");
                    Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
                    return false;
                }
            }
            return true;
        }

        private SimpleResult DoChallenge(Character character, SimpleSkillCheck check)
        {
            var result = _conflictEngine.Run(check, character);
            _challengeOutcomes.Add(new SkillCheckResult(new SkillCheck(check.Skill, check.ChallengeLevel, "Standard check", "Success", "Failure", ChallengePenalty.None), result.DiceRoll));
            var modifiedSkill = result.DiceRoll.TestedSkillValue + check.Difficulty;
            var professionName = character.Profession != null ? character.Profession.Profession.ToString() : "Unemployed";
            Console.Out.Write($"{character.Name} the {professionName}: {check.ChallengeLevel}(+{check.Difficulty}) {check.Skill} Check: ({result.DiceRoll.TestedSkillValue}+{check.Difficulty}={modifiedSkill}): ");
            PrintRollResult(result.DiceRoll);
            return result.DiceRoll;
        }

        private static ChallengeLevel SetChallengeLevel(int threat)
        {
            if (threat < 20)
                return ChallengeLevel.Easy;
            if (threat < 40)
                return ChallengeLevel.Medium;
            else
                return ChallengeLevel.Hard;
        }

        private bool AvoidCapture(Character character, ChallengeLevel challengeLevel)
        {
            Console.Out.WriteLine($"***** City Watch is chasing {character.Name}");

            var check = new SimpleSkillCheck(Skill.Athletics, challengeLevel);
            var result = _conflictEngine.Run(check, character);
            var modifiedSkill = result.DiceRoll.TestedSkillValue + check.Difficulty;
            Console.Out.Write($"***** {character.Name}: {check.ChallengeLevel}(+{check.Difficulty}) {check.Skill} Check: ({result.DiceRoll.TestedSkillValue}+{check.Difficulty}={modifiedSkill}): ");
            PrintRollResult(result.DiceRoll);

            if (!result.DiceRoll.Success)
            {
                // agent capture
                var severity = _threat / 10;
                Console.Out.WriteLine($"***** {character.Name} is captured and will be locked up for threat/10 weeks: {severity}");
                _agentsCaptured.Add(new CharacterCapture(character, severity));
                character.Capture(severity);
                return false;
            }
            else if (result.DiceRoll.Successlevel == 0)
            {
                // agent escaped but sustains a minor injury
                Console.Out.WriteLine($"***** {character.Name} escaped but sustains a minor injury (Ready next week).");
                _agentsInjured.Add(new CharacterInjury(character, 0));
                character.Injury(0);
                return false;
            }
            else if (result.DiceRoll.Successlevel > 0)
            {
                // agent escapes unharmed
                Console.Out.WriteLine($"***** {character.Name} escaped unharmed");
            }

            return true;
        }

        private int GetCurrentEarnings()
        {
            return _challengeOutcomes.Sum(x => x.DiceRoll.Successlevel) * 2;
        }

        private void RunCriticalSuccess(Character character, SimpleSkillCheck check)
        {
            var result = _conflictEngine.Run(check, character);
            var modifiedSkill = result.DiceRoll.TestedSkillValue + check.Difficulty;
            Console.Out.Write($"***** {character.Name}: {check.ChallengeLevel}(+{check.Difficulty}) {check.Skill} Check: ({result.DiceRoll.TestedSkillValue}+{check.Difficulty}={modifiedSkill}): ");
            PrintRollResult(result.DiceRoll);

            if (!result.DiceRoll.Success)
            {
                _state = MissionState.ChallengeFailed;
                return;
            }

            if (result.DiceRoll.Successlevel >= 2)
            {
                _memberCritSuccess.Add(character);
            }
        }

        private void RunMissionChallenge(Character character, SimpleSkillCheck check)
        {
            var result = _conflictEngine.Run(check, character);
            var modifiedSkill = result.DiceRoll.TestedSkillValue + check.Difficulty;
            Console.Out.Write($"{character.Name}: {check.ChallengeLevel}(+{check.Difficulty}) {check.Skill} Check: ({result.DiceRoll.TestedSkillValue}+{check.Difficulty}={modifiedSkill}): ");
            PrintRollResult(result.DiceRoll);

            if (!result.DiceRoll.Success)
            {
                _state = MissionState.ChallengeFailed;
                return;
            }

            if (result.DiceRoll.Successlevel >= 2)
            {
                _memberCritSuccess.Add(character);
            }
        }
    }

    public enum MissionState
    {
        Running,
        ChallengeFailed,
        MissionFailed,
        AgentCaptured,
        Aborted
    }

    public record MissionResult(int Earnings, int Threat, List<Character> CritSuccess);
}