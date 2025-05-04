using System.Collections.Generic;
using UnderworldManager.Models;
using UnderworldManager.Business;

namespace UnderworldManager.Game
{
    public class DailyJobRunner
    {
        private readonly List<Character> _characters;
        private readonly JobGenerator _jobGenerator;

        public DailyJobRunner(List<Character> characters, JobGenerator jobGenerator)
        {
            _characters = characters;
            _jobGenerator = jobGenerator;
        }

        public void Run()
        {
            foreach (var character in _characters)
            {
                SingleRun(character);
            }
        }

        public DailyJobResult SingleRun(Character character)
        {
            var job = _jobGenerator.GenerateJob(character);
            var result = character.SkillCheck(job.Skill, job.Difficulty);
            var earnings = 0;
            var critSuccess = new List<Character>();
            var threat = 0;

            if (result is SkillCheckResultWrapper skillResult)
            {
                Console.WriteLine($"Roll: {skillResult.DiceRoll.Roll} vs {skillResult.DiceRoll.TestedSkillValue}");
                Console.WriteLine($"Success: {skillResult.DiceRoll.Success}");
                Console.WriteLine($"Success Level: {skillResult.DiceRoll.Successlevel}");
                if (skillResult.DiceRoll.Success)
                {
                    character.AddExperience(job.ExperienceReward);
                    character.AddGold(job.GoldReward);
                    character.AddReputation(job.ReputationReward);
                    earnings = job.GoldReward;
                    if (skillResult.DiceRoll.Successlevel >= 2)
                    {
                        critSuccess.Add(character);
                    }
                }
                else
                {
                    threat += 5;
                }
            }

            return new DailyJobResult(earnings, threat, critSuccess);
        }
    }

    public record DailyJobResult(int Earnings, int Threat, List<Character> CritSuccess);
} 