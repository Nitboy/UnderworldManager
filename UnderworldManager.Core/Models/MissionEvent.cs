namespace UnderworldManager.Models
{
    public class MissionEvent
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public bool Success { get; set; }
        public string Details { get; set; }

        public MissionEvent(string type, string description, bool success, string details)
        {
            Type = type;
            Description = description;
            Success = success;
            Details = details;
        }
    }
} 