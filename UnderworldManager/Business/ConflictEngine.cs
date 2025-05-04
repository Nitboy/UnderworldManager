using UnderworldManager.Models;

namespace UnderworldManager.Business
{
  public class ConflictEngine
  {
    private DiceRoller _diceRoller;    

    public ConflictEngine(DiceRoller diceRoller)
    {
      _diceRoller = diceRoller;
    }

    public ConflictEngine()
    {
      _diceRoller = new DiceRoller();
    }

    public SkillCheckResult Run(SkillCheck challenge, Character character, int modifier = 0)
    {
      var diceResult = _diceRoller.Simple(new SimpleInput(character.GetSkillRating(challenge.Skill), challenge.Difficulty, modifier));
      var result = new SkillCheckResult(challenge, diceResult);
      return result;
    }

    public SimpleAttributeCheckResult Run(SimpleAttributeCheck challenge, Character character, int modifier = 0)
    {
      var diceResult = _diceRoller.Simple(new SimpleInput(character.GetAttributeTotal(challenge.Attribute), challenge.Difficulty, modifier));
      var result = new SimpleAttributeCheckResult(challenge, diceResult);
      return result;
    }

    public SkillCheckResultWrapper Run(SimpleSkillCheck challenge, Character character, int modifier = 0)
    {
      var diceResult = _diceRoller.Simple(new SimpleInput(character.GetSkillRating(challenge.Skill), challenge.Difficulty, modifier));
      var result = new SkillCheckResultWrapper(challenge, diceResult);
      return result;
    }
  }
}