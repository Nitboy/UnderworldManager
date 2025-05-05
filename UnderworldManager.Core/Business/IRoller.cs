using UnderworldManager.Core.Models;

namespace UnderworldManager.Core.Business
{
    public interface IRoller
    {
        int RollD100();
        int RollD10();
        int RollD6();
        int RollD4();
        int RollD3();
        SimpleResult Simple(SimpleInput input);
        int Roll(int min, int max);
    }
} 