namespace UnderworldManager.Core.Models
{
    public class SimpleResult
    {
        public int Roll { get; set; }
        public int TargetValue { get; set; }
        public bool Success { get; set; }
        public int SuccessLevel { get; set; }
        public bool Crit { get; set; }
        public int Input { get; set; }

        public SimpleResult(int roll, int targetValue, bool success, int successLevel, bool crit, int input)
        {
            Roll = roll;
            TargetValue = targetValue;
            Success = success;
            SuccessLevel = successLevel;
            Crit = crit;
            Input = input;
        }
    }
} 