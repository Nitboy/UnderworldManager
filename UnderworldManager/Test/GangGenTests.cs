using UnderworldManager.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnderworldManager.Test
{
  [TestClass]
  public class GangGenTests
  {

    [TestMethod]
    public void CreateTier1Gang()
    {
      GangGen gg = new GangGen();
      var g = gg.CreateGang();
      Console.WriteLine(g);
    }

    [TestMethod]
    public void CreateTier2Gang()
    {
      GangGen gg = new GangGen();
      var g = gg.CreateGang(2);
      Console.WriteLine(g);
    }

    [TestMethod]
    public void CreateTier3Gang()
    {
      GangGen gg = new GangGen();
      var g = gg.CreateGang(3);
      Console.WriteLine(g);
    }

    [TestMethod]
    public void CreateTier4Gang()
    {
      GangGen gg = new GangGen();
      var g = gg.CreateGang(4);
      Console.WriteLine(g);
    }
  }


}