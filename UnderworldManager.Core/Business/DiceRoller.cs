using System;
using UnderworldManager.Core.Models;

namespace UnderworldManager.Core.Business
{
  public record SimpleInput(int Value, int Difficulty = 0, int Modifiers = 0);

  public class RandomRoller : IRoller
  {
    public Random R = new Random();
    public int RollD100()
    {
      return R.Next(1, 101);
    }

    public int RollD10()
    {
      return R.Next(1, 11);
    }

    public int RollD6()
    {
      return R.Next(1, 7);
    }

    public int RollD4()
    {
      return R.Next(1, 5);
    }

    public int RollD3()
    {
      return R.Next(1, 4);
    }

    public SimpleResult Simple(SimpleInput input)
    {
      var roll = RollD100();
      var skillAtChallenge = input.Value + input.Difficulty;
      bool hasDoubles = roll % 11 == 0;
      var success = roll <= skillAtChallenge;
      int successLevel;

      if (hasDoubles && success)
        successLevel = 6;
      else if (hasDoubles && !success)
        successLevel = -6;
      else
        successLevel = GetSuccessLevel(roll, skillAtChallenge);

      return new SimpleResult(roll, skillAtChallenge, success, successLevel, hasDoubles, input.Value);
    }

    public int Roll(int min, int max)
    {
      return R.Next(min, max + 1);
    }

    private static int GetSuccessLevel(int roll, int input)
    {
      var difference = input - roll;
      var level = difference / 10;
      if (level > 6)
        return 6;
      if (level < -6)
        return -6;
      return level;
    }
  }

  public class RollerStub : IRoller
  {
    public int ReturnValue = 0;
    public int RollD100()
    {
      return ReturnValue;
    }

    public int RollD10()
    {
      return ReturnValue;
    }

    public int RollD6()
    {
      return ReturnValue;
    }

    public int RollD4()
    {
      return ReturnValue;
    }

    public int RollD3()
    {
      return ReturnValue;
    }

    public SimpleResult Simple(SimpleInput input)
    {
      return new SimpleResult(ReturnValue, input.Value + input.Difficulty, true, 0, false, input.Value);
    }

    public int Roll(int min, int max)
    {
      return ReturnValue;
    }
  }

  public class DiceRoller : IRoller
  {
    private readonly Random _random;

    public DiceRoller()
    {
      _random = new Random();
    }

    public int RollD100()
    {
      return _random.Next(1, 101);
    }

    public int RollD10()
    {
      return _random.Next(1, 11);
    }

    public int RollD6()
    {
      return _random.Next(1, 7);
    }

    public int RollD4()
    {
      return _random.Next(1, 5);
    }

    public int RollD3()
    {
      return _random.Next(1, 4);
    }

    public SimpleResult Simple(SimpleInput input)
    {
      var roll = RollD100();
      var skillAtChallenge = input.Value + input.Difficulty;
      bool hasDoubles = roll % 11 == 0;
      var success = roll <= skillAtChallenge;
      int successLevel;

      if (hasDoubles && success)
        successLevel = 6;
      else if (hasDoubles && !success)
        successLevel = -6;
      else
        successLevel = GetSuccessLevel(roll, skillAtChallenge);

      return new SimpleResult(roll, skillAtChallenge, success, successLevel, hasDoubles, input.Value);
    }

    public int Roll(int min, int max)
    {
      return _random.Next(min, max + 1);
    }

    private static int GetSuccessLevel(int roll, int input)
    {
      var difference = input - roll;
      var level = difference / 10;
      if (level > 6)
        return 6;
      if (level < -6)
        return -6;
      return level;
    }
  }
}