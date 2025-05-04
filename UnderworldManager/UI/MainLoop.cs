using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnderworldManager.Business;
using UnderworldManager.Models;
using UnderworldManager.Game;
using static System.Formats.Asn1.AsnWriter;

namespace UnderworldManager
{
  public class MainLoop
  {
    private readonly Game.Game _game;
    private bool gameRunning = true;
    private GameState state;

    public MainLoop(Game.Game game)
    {
      _game = game;
    }


    public void Run()
    {
      Console.Out.WriteLine();
      Console.Out.WriteLine("UNDERWORLD MANAGER");
      Console.Out.WriteLine("A game of crime, power, and survival in the underworld");
      Console.Out.WriteLine();
      
      _game.StartRound();
      
      do
      {
        switch (state)
        {
          case GameState.Main:
            RunMain();
            break;
          case GameState.Player:
            RunPlayer();
            break;
          case GameState.Roster:
            RunRoster();
            break;
          case GameState.Training:
            RunTraining();
            break;
          case GameState.DailyJob:
            RunDailyJob();
            break;
          case GameState.LayLow:
            LayLow();
            break;
          case GameState.Mission:
            RunMission();
            break;
          case GameState.EndOfWeek:
            RunEndOfWeek();
            break;
          case GameState.GameOver:
            Console.Out.WriteLine();
            Console.Out.WriteLine("GAME OVER");
            Console.Out.WriteLine("Final Statistics:");
            Console.Out.WriteLine($"Player: {_game.Player.Name}");
            Console.Out.WriteLine($"Gang: {_game.Gang.Name}");
            Console.Out.WriteLine($"Weeks Survived: {_game.GameRounds}");
            Console.Out.WriteLine($"Final Gold: {_game.Gang.Gold}");
            Console.Out.WriteLine($"Final Threat Level: {_game.Threat}");
            Console.Out.WriteLine();
            Console.Out.WriteLine("Thank you for playing Underworld Manager!");
            break;
        }
      } while (gameRunning);
    }

    private void RunTraining()
    {
      if (_game.HasTimeLeft())
      {
        Console.Out.WriteLine();
        Console.Out.WriteLine("TRAINING");
        Console.Out.WriteLine("Select a character to train:");
        
        var characters = _game.Gang.Roster.Active;
        for (int i = 0; i < characters.Count; i++)
        {
          Console.Out.WriteLine($"{i + 1} - {characters[i].ShortTrainingPrint()}");
        }
        
        var choice = Utility.GetNumber(1, characters.Count);
        var characterToTrain = characters[choice - 1];
        
        Console.Out.WriteLine();
        Console.Out.WriteLine("CHARACTER DETAILS");
        Console.Out.WriteLine(characterToTrain.LongTrainingPrint());
        
        if (characterToTrain.ExperienceUnspent == 0)
        {
          Console.Out.WriteLine();
          Console.Out.WriteLine("Character is not ready for training - No unspent experience");
          state = GameState.Main;
          return;
        }
        
        Console.Out.WriteLine();
        Console.Out.WriteLine("Select a skill to train:");
        Console.Out.WriteLine();

        var skills = CharacterSkills.ListAllSKillsByCoreAttribute();
        PrintSkillsWithAttributeHeading(skills, characterToTrain);
        
        List<Skill> allSkills = new List<Skill>();
        foreach (var skillGroup in skills.Values)
        {
          allSkills.AddRange(skillGroup);
        }
        
        var skillChoice = Utility.GetNumber(1, allSkills.Count);
        var skillToTrain = allSkills[skillChoice - 1];

        _game.RunTraining(characterToTrain, skillToTrain);
        characterToTrain.AssignNewProfession();
        
        Console.Out.WriteLine();
        Console.Out.WriteLine($"{characterToTrain.Name} is now a {characterToTrain.Profession}");
      }
      else
      {
        Console.Out.WriteLine();
        Console.Out.WriteLine("Not enough time left to train");
      }

      Console.Out.WriteLine();
      Console.Out.WriteLine("Press Enter to continue...");
      Console.ReadLine();
      state = GameState.Main;
    }

    private void PrintSkillsWithAttributeHeading(Dictionary<CoreAttribute, List<Skill>> skills, Character character)
    {
      foreach (var attributeSkills in skills)
      {
        Console.Out.WriteLine($"{attributeSkills.Key}({character.GetAttributeTotal(attributeSkills.Key)}):");
        foreach (var skill in attributeSkills.Value)
        {
          Console.Out.WriteLine($"  {skill}({character.GetSkillTotal(skill)})");
        }
      }
    }

    private void RunDailyJob()
    {
      if (_game.HasTimeLeft())
      {
        Console.Out.WriteLine();
        Console.Out.WriteLine("HITTING THE STREETS");
        Console.Out.WriteLine("Sending your gang out to earn some gold...");
        
        var success = _game.RunDailyJob();
        
        if (success)
        {
          Console.Out.WriteLine("Your gang has earned some gold and experience.");
          Console.Out.WriteLine("Threat level has increased.");
        }
        else
        {
          Console.Out.WriteLine("Failed to run daily job.");
        }
        Console.Out.WriteLine();
        Console.Out.WriteLine("Press Enter to continue...");
        Console.ReadLine();
      }
      else
      {
        Console.Out.WriteLine();
        Console.Out.WriteLine("INVALID ACTION");
        Console.Out.WriteLine("Not enough time left to hit the streets");
      }

      state = GameState.Main;
    }

    private void RunEndOfWeek()
    {
      if (!_game.HasTimeLeft())
      {
        Console.Out.WriteLine();
        Console.Out.WriteLine("END OF WEEK");
        Console.Out.WriteLine($"Week {_game.GameRounds} has come to an end");
        Console.Out.WriteLine();
        
        // Pay salary      
        Console.Out.WriteLine("Paying salaries to gang members...");
        var wages = _game.Gang.PaySalary();
        Console.Out.WriteLine($"Total wages paid: {wages} gold");
        Console.Out.WriteLine($"Current treasury: {_game.Gang.Gold} gold");
        Console.Out.WriteLine();
        
        Console.Out.WriteLine("Visiting the Cappa to pay your dues...");
        Console.Out.WriteLine("As a tier 1 gang, you must pay 100 gold per week");
        
        if(_game.Gang.PayCappa(100))
        {
          Console.Out.WriteLine("The Cappa accepts your payment and bribes the watch");
          Console.Out.WriteLine("The secret peace is maintained for another week");
          _game.EndRound();
        }
        else
        {
          Console.Out.WriteLine("BANKRUPT - Your gang has run out of gold");
          Console.Out.WriteLine("The Cappa makes an example of your gang");
          Console.Out.WriteLine("The sharks eat well today...");
          Console.Out.WriteLine();
          Console.Out.WriteLine("GAME OVER");
          state = GameState.GameOver;
          gameRunning = false;
          return;
        }
      }
      else
      {
        Console.Out.WriteLine();
        Console.Out.WriteLine("You must wait until the end of the week to visit the Cappa");
      }

      state = GameState.Main;
    }

    private void RunPlayer()
    {
      Console.Out.WriteLine();
      Console.Out.WriteLine("PLAYER CHARACTER");
      Console.Out.WriteLine(_game.Player);
      Console.Out.WriteLine();
      Console.Out.WriteLine("Available Actions:");
      Console.Out.WriteLine("s - Train a skill");
      Console.Out.WriteLine("a - Train an attribute");
      Console.Out.WriteLine("b - Back to main menu");
      Console.Out.WriteLine();

      Console.Out.Write("Input > ");
      var input = Console.In.ReadLine();
      var inputfix = input?.Trim().ToLower();
      
      switch (inputfix)
      {
        case "s":
          Console.Out.WriteLine();
          Console.Out.WriteLine("SKILL TRAINING");
          Console.Out.WriteLine("Select a skill to train:");
          // TODO: Implement skill training
          break;
        case "a":
          Console.Out.WriteLine();
          Console.Out.WriteLine("ATTRIBUTE TRAINING");
          Console.Out.WriteLine("Select an attribute to train:");
          // TODO: Implement attribute training
          break;
        case "b":
          state = GameState.Main;
          break;
        default:
          Console.Out.WriteLine();
          Console.Out.WriteLine("INVALID INPUT");
          Console.Out.WriteLine("Please enter a valid option (s, a, or b)");
          break;
      }
    }

    private void RunMission()
    {
      Console.Out.WriteLine();
      Console.Out.WriteLine("AVAILABLE JOBS");
      Console.Out.WriteLine();
      
      var availableJobs = GenerateAvailableJobs();
      for (int i = 0; i < availableJobs.Count; i++)
      {
        var job = availableJobs[i];
        Console.Out.WriteLine($"{i + 1} - {job.Item2}");
        Console.Out.WriteLine($"    Estimated value: {_game.GetMissionValue(job.Item3)} gold");
        Console.Out.WriteLine($"    Base threat: {_game.GetBaseThreatLevel(job.Item3)}");
        Console.Out.WriteLine();
      }
      
      var choice = Utility.GetNumber(1, availableJobs.Count) - 1;
      var mission = new Mission(
        availableJobs[choice].Item1,
        _game.GetMissionValue(availableJobs[choice].Item3),
        _game.GetBaseThreatLevel(availableJobs[choice].Item3),
        availableJobs[choice].Item3);

      Console.Out.WriteLine();
      Console.Out.WriteLine("MISSION START");
      
      var missionRunner = new MissionRunner(_game.Gang.Roster, mission, new Business.ConflictEngine());
      var result = missionRunner.Run();
      
      Console.Out.WriteLine();
      Console.Out.WriteLine("MISSION RESULT");
      
      switch (result)
      {
        case var r when r.Earnings > 0:
          Console.Out.WriteLine($"JOB SUCCESS - Your gang has earned {mission.EstimatedValue} gold");
          break;
        case var r when r.Earnings == 0:
          Console.Out.WriteLine($"JOB FAILED - The mission was not completed successfully");
          break;
        default:
          Console.Out.WriteLine($"UNKNOWN RESULT - Something unexpected happened");
          break;
      }

      _game.Gang.MissionSuccess(mission.EstimatedValue, mission.IsHonorable);
      state = GameState.EndOfWeek;
    }

    private List<Tuple<MissionType, string, bool>> GenerateAvailableJobs()
    {
      Console.Out.WriteLine();
      Console.Out.WriteLine("GENERATING JOBS");
      Console.Out.WriteLine("Contacting your network for available jobs...");
      Console.Out.WriteLine();

      Random rand = new Random();
      List<Tuple<MissionType, string, bool>> missions = new List<Tuple<MissionType, string, bool>>();
      
      for (int i = 0; i < 3; i++)
      {
        var missionType = (MissionType)rand.Next(Enum.GetNames(typeof(MissionType)).Length);
        var description = MissionSkillChecks.GetMissionDescription(missionType);
        var isHonorable = MissionSkillChecks.IsHonorable(missionType);
        
        Console.Out.WriteLine($"Job {i + 1}: {description}");
        Console.Out.WriteLine($"Type: {missionType}");
        Console.Out.WriteLine($"Honor: {(isHonorable ? "Honorable" : "Dishonorable")}");
        Console.Out.WriteLine();
        
        missions.Add(new Tuple<MissionType, string, bool>(missionType, description, isHonorable));
      }

      return missions;
    }

    private void RunRoster()
    {
      Console.Out.WriteLine();
      Console.Out.WriteLine("GANG ROSTER");
      Console.Out.WriteLine(_game.Gang);
      Console.Out.WriteLine();
      Console.Out.WriteLine("Available Actions:");
      Console.Out.WriteLine("h - Hire new member");
      Console.Out.WriteLine("f - Fire a member");
      Console.Out.WriteLine("t - Train a member");
      Console.Out.WriteLine("b - Back to main menu");
      Console.Out.WriteLine();

      Console.Out.Write("Input > ");
      var input = Console.In.ReadLine();
      var inputfix = input?.Trim().ToLower();
      
      switch (inputfix)
      {
        case "h":
          Console.Out.WriteLine();
          Console.Out.WriteLine("HIRING NEW MEMBER");
          Console.Out.WriteLine("Select a character to hire:");
          // TODO: Implement hiring
          break;
        case "f":
          Console.Out.WriteLine();
          Console.Out.WriteLine("FIRING A MEMBER");
          Console.Out.WriteLine("Select a character to fire:");
          // TODO: Implement firing
          break;
        case "t":
          Console.Out.WriteLine();
          Console.Out.WriteLine("TRAINING A MEMBER");
          Console.Out.WriteLine("Select a character to train:");
          // TODO: Implement member training
          break;
        case "b":
          state = GameState.Main;
          break;
        default:
          Console.Out.WriteLine();
          Console.Out.WriteLine("INVALID INPUT");
          Console.Out.WriteLine("Please enter a valid option (h, f, t, or b)");
          break;
      }
    }

    private void RunMain()
    {
        Console.Out.WriteLine();
        Console.Out.WriteLine("UNDERWORLD MANAGER");
        Console.Out.WriteLine("A game of crime, power, and survival in the underworld");
        Console.Out.WriteLine();
        Console.Out.WriteLine($"Player: {_game.Player.Name} of the {_game.Gang.Name}");
        Console.Out.WriteLine($"Week: {_game.GameRounds}");
        Console.Out.WriteLine($"Time Left: {_game.WeekTimer}");
        Console.Out.WriteLine($"Gold: {_game.Gang.Gold}");
        Console.Out.WriteLine();
        Console.Out.WriteLine($"Active Members: {_game.Gang.Roster.Active.Count}");
        Console.Out.WriteLine($"Threat Level: {_game.Threat}");
        Console.Out.WriteLine();
        Console.Out.WriteLine("Earning difficulty increases at:");
        Console.Out.WriteLine("- 20 threat");
        Console.Out.WriteLine("- 40 threat");
        Console.Out.WriteLine();
        Console.Out.WriteLine("MAIN MENU");
        Console.Out.WriteLine("Character Management:");
        Console.Out.WriteLine("p - View Player Character Details");
        Console.Out.WriteLine("r - View Gang Roster");
        Console.Out.WriteLine("t - Training");
        Console.Out.WriteLine();
        
        if (_game.HasTimeLeft())
        {
            Console.Out.WriteLine("Available Actions:");
            Console.Out.WriteLine("s - Hit the streets (Earn money)");
            Console.Out.WriteLine("l - Lay Low (Reduce threat)");
        }
        else
        {
            Console.Out.WriteLine("End of Week Actions:");
            Console.Out.WriteLine("v - Visit Cappa (Pay weekly dues)");
        }
        Console.Out.WriteLine();
        Console.Out.Write("Input > ");
        var input = Console.In.ReadLine();
        var inputfix = input?.Trim().ToLower();

        switch (inputfix)
        {
            case "p":
                state = GameState.Player;
                break;
            case "r":
                state = GameState.Roster;
                break;
            case "t":
                state = GameState.Training;
                break;
            case "s":
                if (_game.HasTimeLeft())
                {
                    state = GameState.DailyJob;
                }
                else
                {
                    Console.Out.WriteLine();
                    Console.Out.WriteLine("INVALID ACTION");
                    Console.Out.WriteLine("No time left this week to hit the streets");
                }
                break;
            case "l":
                if (_game.HasTimeLeft())
                {
                    LayLow();
                }
                else
                {
                    Console.Out.WriteLine();
                    Console.Out.WriteLine("INVALID ACTION");
                    Console.Out.WriteLine("No time left this week to lay low");
                }
                break;
            case "v":
                if (!_game.HasTimeLeft())
                {
                    state = GameState.EndOfWeek;
                }
                else
                {
                    Console.Out.WriteLine();
                    Console.Out.WriteLine("INVALID ACTION");
                    Console.Out.WriteLine("You must wait until the end of the week to visit the Cappa");
                }
                break;
            default:
                Console.Out.WriteLine();
                Console.Out.WriteLine("INVALID INPUT");
                Console.Out.WriteLine("Please enter a valid option (p, r, t, s, l, or v)");
                break;
        }
    }

    private void LayLow()
    {
      if (_game.SpendTime(GangAction.LayLow))
      {
        Console.Out.WriteLine();
        Console.Out.WriteLine("LAYING LOW");
        Console.Out.WriteLine("The gang is laying low to shake some heat...");
        
        var result = _game.LayLow();
        
        Console.Out.WriteLine($"Threat Reduction: {result.ThreatReduction}");
        Console.Out.WriteLine($"Time Spent: {result.TimeSpent}");
        Console.Out.WriteLine();
        Console.Out.WriteLine("Press Enter to continue...");
        Console.ReadLine();
      }
      else
      {
        Console.Out.WriteLine();
        Console.Out.WriteLine("INVALID ACTION");
        Console.Out.WriteLine("Not enough time left to lay low");
      }
    }
  }
}