using UnityEngine;

namespace Recursive.MonoSystem
{
    public abstract class DialogueEvent : ScriptableObject
    {
        public abstract void OnEnter();

        public abstract void OnUpdate();

        public abstract void OnExit();

        public virtual bool CanProceed() => true;
    }
}
