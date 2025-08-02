using UnityEngine;

namespace Recursive.Player
{
    [System.Serializable]
    public struct Frame
    {
        public float Time;
        public Vector3 Position;
        public Quaternion Rotation;
        public bool JumpPressed;
        public bool InteractPressed;

        public Frame(float time, Vector3 position, Quaternion rotation, bool jumpPressed, bool interactPressed)
        {
            Time = time;
            Position = position;
            Rotation = rotation;
            JumpPressed = jumpPressed;
            InteractPressed = interactPressed;
        }
    }
}
