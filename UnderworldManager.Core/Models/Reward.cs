namespace UnderworldManager.Models
{
    public class Reward
    {
        public string Type { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }

        public Reward(string type, int amount, string description)
        {
            Type = type;
            Amount = amount;
            Description = description;
        }
    }
} 