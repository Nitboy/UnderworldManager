using UnderworldManager.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnderworldManager.Test
{
  [TestClass]
  public class CharGenTests
  {
    [TestMethod]
    public void CreateCharacter()
    {
      CharGen cg = new CharGen();
      var c = cg.CreateCharacter();
      Console.WriteLine(c);
    }
    [TestMethod]
    public void CreateNoble()
    {
      CharGen cg = new CharGen();
      var c = cg.CreateNoble();
      Console.WriteLine(c);
    }
    [TestMethod]
    public void CreateEpic()
    {
      CharGen cg = new CharGen();
      var c = cg.CreateEpic();
      Console.WriteLine(c);
    }


  }


}