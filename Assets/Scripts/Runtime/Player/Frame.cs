using UnityEngine;

namespace Recursive.Player
{
    [System.Serializable]
    public struct Frame
    {
        public float Time;
        public Vector3 Position;
        public Quaternion Rotation;

        public Frame(float time, Vector3 position, Quaternion rotation)
        {
            Time = time;
            Position = position;
            Rotation = rotation;
        }
    }
}
