using UnderworldManager.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnderworldManager.Test
{
  [TestClass]
  public class DiceRollerTest
  {
    private DiceRoller diceRoller;
    public DiceRollerTest()
    {
      diceRoller = new DiceRoller(new RandomRoller());
    }

    [TestMethod]
    public void SuccessLevel()
    {
      Assert.AreEqual(0, DiceRoller.GetSuccessLevel(50, 50));
      Assert.AreEqual(0, DiceRoller.GetSuccessLevel(41, 50));
      Assert.AreEqual(0, DiceRoller.GetSuccessLevel(59, 50));
      Assert.AreEqual(1, DiceRoller.GetSuccessLevel(40, 50));
      Assert.AreEqual(1, DiceRoller.GetSuccessLevel(31, 50));
      Assert.AreEqual(2, DiceRoller.GetSuccessLevel(30, 50));
      Assert.AreEqual(2, DiceRoller.GetSuccessLevel(21, 50));
      Assert.AreEqual(-1, DiceRoller.GetSuccessLevel(60, 50));
      Assert.AreEqual(-1, DiceRoller.GetSuccessLevel(69, 50));
      Assert.AreEqual(-2, DiceRoller.GetSuccessLevel(70, 50));
      Assert.AreEqual(-2, DiceRoller.GetSuccessLevel(79, 50));
      Assert.AreEqual(-6, DiceRoller.GetSuccessLevel(61, 1));
    }

    [TestMethod]
    [DataRow(33)]
    [DataRow(49)]
    [DataRow(65)]
    public void SimpleRoll(int skill)
    {
      var result = diceRoller.Simple(new SimpleInput(skill));
      if (result.Roll <= skill)
      {
        Assert.IsTrue(result.Success);
      }
      else
      {
        Assert.IsFalse(result.Success);
      }

      if (!result.Crit)
      {
        Console.WriteLine("Roll was " + result.Roll + " Skill was " + skill);
        var sl = (skill - result.Roll) / 10;
        if (sl > 6)
        {
          sl = 6;
        }
        if (sl < -6)
        {
          sl = -6;
        }
        Assert.AreEqual(sl, result.Successlevel);
      }

      if (result.Success && result.Crit)
      {
        Assert.AreEqual(6, result.Successlevel);
      }
      else if (!result.Success && result.Crit)
      {
        Assert.AreEqual(-6, result.Successlevel);
      }
    }

    [TestMethod]
    public void FullChurn()
    {
      for (int i = 1; i < 101; i++)
      {
        var setRoller = new RollerStub();
        setRoller.ReturnValue = i;
        diceRoller = new DiceRoller(setRoller);
        for (int j = 1; j < 101; j++)
        {
          Console.WriteLine("Roll was " + i + " Skill was " + j);
          SimpleRoll(j);
        }
      }
    }

    [TestMethod]
    [DataRow(11)]
    [DataRow(22)]
    [DataRow(33)]
    [DataRow(44)]
    [DataRow(55)]
    [DataRow(66)]
    [DataRow(77)]
    [DataRow(88)]
    [DataRow(99)]
    public void CritRoll(int setRoll)
    {
      for (int i = 1; i < 101; i++)
      {
        var setRoller = new RollerStub();
        setRoller.ReturnValue = setRoll;
        var skill = i;
        var roller = new DiceRoller(setRoller);
        var result = roller.Simple(new SimpleInput(skill));

        if (result.Success)
        {
          Console.WriteLine("Success Roll was " + result.Roll + " Skill was " + skill);
          Assert.AreEqual(6, result.Successlevel);
        }
        else
        {
          Console.WriteLine("Fail Roll was " + result.Roll + " Skill was " + skill);
          Assert.AreEqual(-6, result.Successlevel);
        }
      }

    }
  }
}


