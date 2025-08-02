using PlazmaGames.Attribute;
using PlazmaGames.Core;
using Recursive.MonoSystem;
using UnityEngine;

namespace Recursive.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Controller : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CharacterController _controller;
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _feet;
        [SerializeField] private PlayerSettings _settings;

        [Header("Grounded Check")]
        [SerializeField] private LayerMask _groundedCheckIgnoreLayer;
        [SerializeField] private float _groundedCheckDst = 0.1f;
        [SerializeField] private float _groundedCheckRadius = 0.1f;

        [Header("Debug -- Movement")]
        [SerializeField, ReadOnly] private Vector3 _movementSpeed;
        [SerializeField, ReadOnly] private Vector3 _currentVel;
        [SerializeField, ReadOnly] private float _velY;

        [Header("Debug -- Look")]
        [SerializeField, ReadOnly] private float _smoothedXRot;
        [SerializeField, ReadOnly] private float _smoothedYRot;

        private float gravity = -9.81f;

        private IInputMonoSystem _input;
        
        private bool IsGrounded()
        {
            bool isGrounded = Physics.SphereCast(_feet.position, _groundedCheckRadius, -transform.up, out RaycastHit hit, _groundedCheckDst, ~_groundedCheckIgnoreLayer);
            return isGrounded;
        }

        private void Jump()
        {
            if (IsGrounded()) _velY = _settings.JumpForce;
        }

        private void ProcessGravity()
        {
            if (IsGrounded() && _velY < 0.0f) _velY = 0.0f;
            else _velY += _settings.GravityMultiplier * gravity * Time.deltaTime;

            _movementSpeed.y = _velY;
        }

        private void ProcessMovement()
        {
            float dirSpeed = (_input.RawMovement.y == 1) ? _settings.WalkingForwardSpeed : _settings.WalkingBackwardSpeed;
            float forwardSpeed = _input.RawMovement.y * _settings.Speed * dirSpeed;
            float rightSpeed = _input.RawMovement.x * _settings.Speed * _settings.WalkingStrideSpeed;

            _movementSpeed = Vector3.SmoothDamp(
                _movementSpeed,
                new Vector3(
                    rightSpeed,
                    0,
                    forwardSpeed),
                ref _currentVel,
                _settings.MovementSmoothing
            );
        }

        private void ProcessLook()
        {
            // Head Rotation
            _smoothedXRot -= (_settings.InvertLookY ? -1 : 1) * _settings.Sensitivity.y * _input.RawLook.y;
            _smoothedXRot = Mathf.Clamp(_smoothedXRot, _settings.YLookLimit.x, _settings.YLookLimit.y);
            Quaternion headRot = Quaternion.Euler(_smoothedXRot, 0f, 0f);
            _head.localRotation = headRot;

            // Body Rotation
            _smoothedYRot += (_settings.InvertLookX ? -1 : 1) * _settings.Sensitivity.x * _input.RawLook.x;
            Quaternion playerRot = Quaternion.Euler(0f, _smoothedYRot, 0f);
            transform.localRotation = playerRot;
        }

        private void Awake()
        {
            if (!_controller) _controller = GetComponent<CharacterController>();
            _input = GameManager.GetMonoSystem<IInputMonoSystem>();

            _input.JumpAction.AddListener(Jump);
        }

        private void Update()
        {
            ProcessLook();
            ProcessMovement();
            ProcessGravity();

            _controller.Move(transform.TransformDirection(_movementSpeed * Time.deltaTime));
        }
    }
}