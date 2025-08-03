using System;
using PlazmaGames.Attribute;
using PlazmaGames.Core;
using PlazmaGames.UI;
using Recursive.MonoSystem;
using Recursive.UI;
using UnityEngine;

namespace Recursive.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Controller : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CharacterController _controller;
        [SerializeField] private AudioSource _as;
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

        private Vector3 _groundPoint;
        
        private bool IsGrounded()
        {
            bool isGrounded = Physics.CheckSphere(_feet.position, _groundedCheckRadius, ~_groundedCheckIgnoreLayer);
            return isGrounded;
        }

        private void Jump()
        {
            if (GameManager.GetMonoSystem<IUIMonoSystem>().GetView<GameView>().IsShowingDialogue()) return;

            if (IsGrounded()) _velY = _settings.JumpForce;
        }

        private void ProcessGravity()
        {
            bool isGrounded = IsGrounded();
            if (isGrounded && _velY < 0.0f) _velY = 0.0f;
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
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (!_controller) _controller = GetComponent<CharacterController>();
            _input = GameManager.GetMonoSystem<IInputMonoSystem>();

            _input.JumpAction.AddListener(Jump);
        }

        private void Update()
        {
            ProcessLook();
            ProcessMovement();
            ProcessGravity();

            Vector3 speed = _movementSpeed;
            speed.y = 0;
            if (speed.magnitude > 0.01f && IsGrounded())
            {
                if (!_as.isPlaying)
                {
                    _as.time = UnityEngine.Random.Range(0, _as.clip.length);
                    _as.Play();
                }
            }
            else
            {
                _as.Stop();
            }

            _controller.Move(transform.TransformDirection(_movementSpeed * Time.deltaTime));
        }

        public void Move(Vector3 vec)
        {
            _controller.Move(vec);
        }

        public void SetTransform(Transform trans)
        {
            _controller.enabled = false;
            transform.position = trans.position;
            transform.rotation = trans.rotation;
            _smoothedYRot = trans.eulerAngles.y;
            _smoothedXRot = trans.eulerAngles.x;
            _controller.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Exit"))
            {
                GameManager.GetMonoSystem<IGameplayMonoSystem>().NextLevel();
            }
        }
    }
}