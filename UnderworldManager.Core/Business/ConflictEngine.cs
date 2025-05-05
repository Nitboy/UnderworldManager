using System;
using UnderworldManager.Core.Models;

namespace UnderworldManager.Core.Business
{
  public class ConflictEngine
  {
    private readonly Random _random;

    public ConflictEngine()
    {
      _random = new Random();
    }

    public CheckResult Run(SimpleSkillCheck check, Character character)
    {
      var skillValue = character.GetSkillTotal(check.Skill);
      var roll = _random.Next(1, 101);
      var result = new SimpleResult(roll, skillValue, roll <= skillValue, (skillValue - roll) / 10, roll % 11 == 0, skillValue);
      return new CheckResult(result, character.GetAttributeBySkill(check.Skill), check.ChallengeLevel);
    }

    public CheckResult Run(SimpleAttributeCheck check, Character character)
    {
      var attributeValue = character.GetAttributeTotal(check.Attribute);
      var roll = _random.Next(1, 101);
      var result = new SimpleResult(roll, attributeValue, roll <= attributeValue, (attributeValue - roll) / 10, roll % 11 == 0, attributeValue);
      return new CheckResult(result, character.GetAttributeByType(check.Attribute), check.ChallengeLevel);
    }
  }
}