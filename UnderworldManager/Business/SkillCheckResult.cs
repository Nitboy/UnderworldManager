using UnderworldManager.Models;

namespace UnderworldManager.Business
{
  public class SkillCheckResult
  {
    public SkillCheck SkillCheck { get; set; }

    public SimpleResult DiceRoll { get; set; }

    public SkillCheckResult(SkillCheck skillCheck, SimpleResult diceRoll)
    {
      SkillCheck = skillCheck;
      DiceRoll = diceRoll;
    }
  }

  public class SimpleAttributeCheckResult
  {
    public SimpleAttributeCheck Check { get; set; }

    public SimpleResult DiceRoll { get; set; }

    public SimpleAttributeCheckResult(SimpleAttributeCheck check, SimpleResult diceRoll)
    {
      Check = check;
      DiceRoll = diceRoll;
    }
  }

  public class SimpleSkillCheckResult
  {
    public SimpleSkillCheck Check { get; set; }

    public SimpleResult DiceRoll { get; set; }

    public SimpleSkillCheckResult(SimpleSkillCheck check, SimpleResult diceRoll)
    {
      Check = check;
      DiceRoll = diceRoll;
    }
  } 
}