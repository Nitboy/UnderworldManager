namespace UnderworldManager
{
  public enum GangAction
  {
    HitTheStreets, //Daily job/earning
    LayLow, //no cost, -threat
    Tavern, // - medium cost, + rumors
    Mission, // requires rumor+gear, earns reputation, huge payoff
    Market, // buy gear and weapons
    Fence, // sell gear and weapons
    Healer, // heal up an injured member
    Bailiff, // Post bail for a captured member
    BribeTheWatch, // -threat, medium cost
    Training // -low cost, +skill, member training cannot earn this week.      
  }
}