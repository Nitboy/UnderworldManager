using System.Runtime.Serialization;

namespace UnderworldManager.Models
{
  internal class GameOverException : Exception
  {
    public GameOverException()
    {
    }

    public GameOverException(string? message) : base(message)
    {
    }

    public GameOverException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
  }
}