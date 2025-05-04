using System.Threading.Tasks;
using UnderworldManager.Models;

namespace UnderworldManager.Services
{
    public interface ICharacterService
    {
        Task<Character> GetCharacterAsync(int id);
        Task UpdateCharacterAsync(Character character);
    }
} 