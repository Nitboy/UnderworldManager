using System.Threading.Tasks;
using UnderworldManager.Core.Models;

namespace UnderworldManager.Services
{
    public interface ICharacterService
    {
        Task<Character> GetCharacterAsync(int id);
        Task UpdateCharacterAsync(Character character);
    }
} 