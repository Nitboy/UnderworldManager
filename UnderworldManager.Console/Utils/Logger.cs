namespace UnderworldManager.Utils
{
    public interface ILogger<T>
    {
        void LogInformation(string message);
        void LogError(string message);
    }

    public class Logger<T> : ILogger<T>
    {
        public void LogInformation(string message)
        {
            System.Console.WriteLine($"[INFO] {message}");
        }

        public void LogError(string message)
        {
            System.Console.WriteLine($"[ERROR] {message}");
        }
    }
} 