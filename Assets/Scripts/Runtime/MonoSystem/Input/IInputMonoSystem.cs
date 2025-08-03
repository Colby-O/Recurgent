using PlazmaGames.Core.MonoSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Recursive.MonoSystem
{
    public interface IInputMonoSystem : IMonoSystem
    {
        public UnityEvent JumpAction { get; }
        public UnityEvent InteractCallback { get; }
        public UnityEvent LeftMouseAction { get; }
        public UnityEvent RightMouseAction { get; }
        public UnityEvent RestartAction { get; }

        public UnityEvent Slot1Action { get; }
        public UnityEvent Slot2Action { get; }
        public UnityEvent Slot3Action { get; }
        public UnityEvent Slot4Action { get; }

        public Vector2 RawMovement { get; }
        public Vector2 RawLook { get; }
    }
}
