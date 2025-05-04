using System.Xml.Linq;
using UnderworldManager.Business;
using UnderworldManager.Models;

namespace UnderworldManager
{
  public class Game
  {
    public Character Player { get; private set; }

    public Gang Gang { get; private set; }

    public int GameRounds;
    public int WeekTimer;
    public int Threat;

    private int BaseMissionValue = 100;
    private int GameRoundValueModifier = 20;
    private int FameInfameValueModifier = 10;

    private Dictionary<int, List<GangAction>> _gangActions = new Dictionary<int, List<GangAction>>();

    public Game(Character player, Gang gang)
    {
      Player = player;
      Gang = gang;
    }

    public int GetMissionValue(bool isHonorable)
    {
      if (isHonorable)
      {
        return BaseMissionValue +
              GameRoundValueModifier * GameRounds +
              FameInfameValueModifier * Gang.Fame;
      }
      else
      {
        return BaseMissionValue +
              GameRoundValueModifier * GameRounds +
              FameInfameValueModifier * Gang.Infamy;
      }
    }

    public int GetBaseThreatLevel(bool isHonorable)
    {
      if (isHonorable)
      {
        return Gang.Infamy / 10;
      }
      else
      {
        return 1 + (Gang.Infamy / 10);
      }
    }

    public void StartRound()
    {
      GameRounds++;
      WeekTimer = 7;
      Threat = GetBaseThreatLevel(true); // Cappa bribes the watch to keep the secret peace.
      Gang.ResetRoster();
    }

    public bool EndRound()
    {
      if (WeekTimer != 0)
      {
        return false;
      }
      // Heal injury
      // countdown release for captured agents.
      Gang.AdvanceTime();
      StartRound();
      return true;
    }

    public bool SpendTime(GangAction gangAction)
    {
      if (WeekTimer > 0)
      {
        WeekTimer--;
        if (!_gangActions.ContainsKey(GameRounds))
        {
          _gangActions.Add(GameRounds, new List<GangAction>());
        }
        _gangActions[GameRounds].Add(gangAction);
        return true;
      }
      return false;
    }

    public bool HasTimeLeft()
    {
      return WeekTimer > 0;
    }


    internal void LayLow()
    {
      Threat -= 5;
      if (Threat < GetBaseThreatLevel(true))
      {
        Threat = GetBaseThreatLevel(true);
      }
    }

    internal void DailyJob()
    {

    }
    private DailyJobRunner CreateDailyJobRunner()
    {
      return new DailyJobRunner(Gang, new ConflictEngine(), Threat);
    }

    public bool RunDailyJob()
    {
      if (!SpendTime(GangAction.HitTheStreets))
      {
        return false;
      }

      var dailyJobRunner = CreateDailyJobRunner();
      var earningsResult = dailyJobRunner.SingleRun();

      Gang.Gold += earningsResult.Earnings;

      // Update Threat
      Threat = earningsResult.Threat;

      var upgradeRoller = CreateUpgradeRoller();
      // Grant increases on crit success skill checks
      foreach (var member in earningsResult.CritSuccess)
      {
        upgradeRoller.CritIncrease(member);
      }

      return true;
    }

    public bool RunTraining(Character character, Skill skill)
    {
      if (!SpendTime(GangAction.Training))
      {
        return false;
      }
      var upgradeRoller = CreateUpgradeRoller();
      upgradeRoller.TrainingIncrease(character, skill, 5);
      return true;
    }

    private static CharUpgradeRoller CreateUpgradeRoller()
    {
      return new CharUpgradeRoller(new ConflictEngine());
    }
  }
}