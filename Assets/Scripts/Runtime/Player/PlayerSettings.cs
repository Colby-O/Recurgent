using UnityEngine;

namespace Recursive.Player
{
    [CreateAssetMenu(fileName = "DefaultPlayerMovementSettings", menuName = "Player/MovementSettings")]
    public class PlayerSettings : ScriptableObject
    {
        [Header("Movement")]
        public float Speed;
        public float WalkingForwardSpeed = 1f;
        public float WalkingBackwardSpeed = 0.5f;
        public float WalkingStrideSpeed = 1f;
        public float MovementSmoothing = 0.5f;
        public float GravityMultiplier = 1.0f;
        public float JumpForce = 1f;
        public float CrouchHeight = 1.2f;
        public float CrouchSpeedMul = 0.5f;

        [Header("Look")]
        public Vector2 Sensitivity = Vector2.one;
        public Vector2 YLookLimit;
        public bool InvertLookY = false;
        public bool InvertLookX = false;
    }
}
