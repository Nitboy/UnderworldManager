using UnderworldManager.Business;

namespace UnderworldManager.Models
{
    public class SimpleResult
    {
        public int Roll { get; set; }
        public int TestedSkillValue { get; set; }
        public bool Success { get; set; }
        public int Successlevel { get; set; }
        public bool Crit { get; set; }
        public SimpleInput Input { get; set; }

        public SimpleResult(int roll, int testedSkillValue, bool success, int successlevel, bool crit, SimpleInput input)
        {
            Roll = roll;
            TestedSkillValue = testedSkillValue;
            Success = success;
            Successlevel = successlevel;
            Crit = crit;
            Input = input;
        }
    }
} 