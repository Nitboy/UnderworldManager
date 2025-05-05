using UnderworldManager.Core.Models;

namespace UnderworldManager.Models
{
    public record CharacterCapture(Character Character, int Severity);
    public record CharacterInjury(Character Character, int Severity);
} 