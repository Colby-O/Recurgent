using PlazmaGames.Core.MonoSystem;
using UnityEngine;

namespace Recursive.MonoSystem
{
    public interface IGameplayMonoSystem : IMonoSystem
    {
        public void LoadLevel(Level level);
    }
}
