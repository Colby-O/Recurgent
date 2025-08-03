using PlazmaGames.Core.MonoSystem;
using Recursive.Player;
using UnityEngine;

namespace Recursive.MonoSystem
{
    public interface IRecorderMonoSystem : IMonoSystem
    {
        public Recording[] Recordings();
        public Recording[] ActiveRecordings();
        public void SelectSlot(int id);
        public void SetSelectedRecording(Recording recording);
        public Recording GetSelectedRecording();
        public void ClearAllSlots();
    }
}
