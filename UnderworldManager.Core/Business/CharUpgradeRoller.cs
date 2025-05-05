using System;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using UnderworldManager.Core.Models;

namespace UnderworldManager.Core.Business
{
  public class CharUpgradeRoller
  {
    public ConflictEngine _conflictEngine { get; }

    public CharUpgradeRoller(ConflictEngine conflictEngine)
    {
      _conflictEngine = conflictEngine;
    }

    public bool CritIncrease(Character character)
    {
      var professionName = character.Profession != null ? character.Profession.Profession.ToString() : "Unemployed";
      Console.Out.WriteLine($"{character.Name} the {professionName} rolled a crit. They must roll higher than their attribute/skill to increase");
      bool increase = false;
      
      if (character.Profession == null)
      {
        Console.Out.WriteLine($"{character.Name} has no profession and cannot be trained.");
        return false;
      }

      if (ProfessionMapper.IsBasicProfession(character.Profession.Profession))
      {
        var attribute = character.Profession.PrimaryAttribute;
        var check = new SimpleAttributeCheck(attribute, ChallengeLevel.Hard);
        var result = DoChallenge(character, check);
        if (!result.DiceRoll.Success)
        {
          Console.Out.WriteLine($"Success! {character.Name} the {character.Profession} increases their {attribute} by 1");
          character.Increase(attribute, 1, true);
          increase = true;
        }
        else
        {
          Console.Out.WriteLine($"Failure! {character.Name} the {character.Profession} does not increase their {attribute}");
        }
      }
      else
      {
        var mainSkill = character.Profession.Skill;
        var mainAttribute = character.Profession.PrimaryAttribute;
        var attributeCheck = new SimpleAttributeCheck(mainAttribute, ChallengeLevel.Hard);
        var skillCheck = new SimpleSkillCheck(mainSkill, ChallengeLevel.Hard);
        var attributeResult = DoChallenge(character, attributeCheck);
        var skillResult = DoChallenge(character, skillCheck);

        if (!attributeResult.DiceRoll.Success)
        {
          Console.Out.WriteLine($"Success! {character.Name} the {character.Profession} increases their {mainAttribute} by 1");
          character.Increase(mainAttribute, 1, true);
          increase = true;
        }
        else
        {
          Console.Out.WriteLine($"Failure! {character.Name} the {character.Profession} does not increase their {mainAttribute}");
        }
      }

      return increase;
    }

    public int TrainingIncrease(Character character, Skill skill, int maxTraining)
    {
      var professionName = character.Profession != null ? character.Profession.Profession.ToString() : "Unemployed";
      Console.Out.WriteLine($"{character.Name} the {professionName} undergoes {skill} training. They must roll higher than their attribute/skill to increase");
      var mainAttribute = SkillUtils.GetCoreAttribute(skill);
      var attributeCheck = new SimpleAttributeCheck(mainAttribute, ChallengeLevel.Hard);
      var skillCheck = new SimpleSkillCheck(skill, ChallengeLevel.Hard);
      var trainingSuccess = 0;
      for (int i = 0; i < maxTraining; i++)
      {
        var success = TryTrainSkill(character, skillCheck);
        if(success)
        {
          trainingSuccess++;
        }
      }

      if (trainingSuccess != maxTraining)
      {
        Console.Out.WriteLine($"Finally {character.Name} the {professionName} has a chance to increase their {mainAttribute} training. They must roll higher than their attribute/skill to increase");
        var success = TryTrainAttribute(character, attributeCheck);
        if (success)
        {
          trainingSuccess++;
        }
      }
      return trainingSuccess;
    }

    public int TrainingIncrease(Character character, CoreAttribute attribute, int maxTraining)
    {
      var professionName = character.Profession != null ? character.Profession.Profession.ToString() : "Unemployed";
      Console.Out.WriteLine($"{character.Name} the {professionName} undergoes {attribute} training. They must roll higher than their attribute/skill to increase");    

      var attributeCheck = new SimpleAttributeCheck(attribute, ChallengeLevel.Hard);
      var trainingSuccess = 0;
      for (int i = 0; i < maxTraining; i++)
      {
        var success = TryTrainAttribute(character, attributeCheck);
        if (success)
        {
          trainingSuccess++;
        }
      }

      return trainingSuccess;
    }

    public bool TryTrainSkill(Character character, SimpleSkillCheck skillCheck)
    {
      bool increase = false;
      var professionName = character.Profession != null ? character.Profession.Profession.ToString() : "Unemployed";

      var result = DoChallenge(character, skillCheck);

      if (!result.DiceRoll.Success)
      {
        Console.Out.WriteLine($"Success! {character.Name} the {professionName} increases their {skillCheck.Skill} by 1");
        character.Increase(skillCheck.Skill, 1, false);
        increase = true;
      }
      else
      {
        Console.Out.WriteLine($"Failure! {character.Name} the {professionName} does not increase their {skillCheck.Skill}");
      }

      return increase;
    }

    public bool TryTrainAttribute(Character character, SimpleAttributeCheck attributeCheck)
    {
      bool increase = false;
      var professionName = character.Profession != null ? character.Profession.Profession.ToString() : "Unemployed";

      var result = DoChallenge(character, attributeCheck);

      if (!result.DiceRoll.Success)
      {
        Console.Out.WriteLine($"Success! {character.Name} the {professionName} increases their {attributeCheck.Attribute} by 1");
        character.Increase(attributeCheck.Attribute, 1, false);
        increase = true;
      }
      else
      {
        Console.Out.WriteLine($"Failure! {character.Name} the {professionName} does not increase their {attributeCheck.Attribute}");
      }

      return increase;
    }

    private CheckResult DoChallenge(Character character, SimpleSkillCheck check)
    {
      return _conflictEngine.Run(check, character);
    }

    private CheckResult DoChallenge(Character character, SimpleAttributeCheck check)
    {
      return _conflictEngine.Run(check, character);
    }
  }
}