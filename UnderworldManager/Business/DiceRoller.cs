using UnderworldManager.Models;

namespace UnderworldManager.Business
{
  public record SimpleInput(int Skill, int Difficulty = 0, int Modifiers = 0);

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
  }

  public class DiceRoller
  {
    public IRoller Dice;


    public DiceRoller(IRoller roller)
    {
      Dice = roller;
    }

    public DiceRoller()
    {
      Dice = new RandomRoller();
    }

    public SimpleResult Simple(SimpleInput input)
    {
      var roll = Dice.RollD100();
      var skillAtChallenge = input.Skill + input.Difficulty + input.Modifiers;
      bool hasDoubles = roll % 11 == 0;
      var success = roll <= skillAtChallenge;
      int successLevel;

      if (hasDoubles && success)
        successLevel = 6;
      else if (hasDoubles && !success)
        successLevel = -6;
      else
        successLevel = GetSuccessLevel(roll, skillAtChallenge);

      return new SimpleResult(roll, skillAtChallenge, success, successLevel, hasDoubles, input);
    }

    public static int GetSuccessLevel(int roll, int input)
    {
      var sl = (input - roll) / 10;
      if (sl > 6)
      {
        sl = 6;
      }
      if (sl < -6)
      {
        sl = -6;
      }
      return sl;
    }
  }
}