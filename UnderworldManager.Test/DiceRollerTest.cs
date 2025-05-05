using UnderworldManager.Core.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnderworldManager.Test
{
  [TestClass]
  public class DiceRollerTest
  {
    private DiceRoller diceRoller;
    public DiceRollerTest()
    {
      diceRoller = new DiceRoller();
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
        Assert.AreEqual(sl, result.SuccessLevel);
      }

      if (result.Success && result.Crit)
      {
        Assert.AreEqual(6, result.SuccessLevel);
      }
      else if (!result.Success && result.Crit)
      {
        Assert.AreEqual(-6, result.SuccessLevel);
      }
    }

    [TestMethod]
    public void FullChurn()
    {
      for (int i = 1; i < 101; i++)
      {
        var setRoller = new RollerStub();
        setRoller.ReturnValue = i;
        diceRoller = new DiceRoller();
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
        var skill = i;
        var result = diceRoller.Simple(new SimpleInput(skill));

        if (result.Roll % 11 == 0)  // Check if it's a critical roll
        {
          if (result.Success)
          {
            Console.WriteLine("Success Roll was " + result.Roll + " Skill was " + skill);
            Assert.AreEqual(6, result.SuccessLevel);
          }
          else
          {
            Console.WriteLine("Fail Roll was " + result.Roll + " Skill was " + skill);
            Assert.AreEqual(-6, result.SuccessLevel);
          }
        }
      }
    }
  }
}


