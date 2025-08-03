using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Recursive.MonoSystem
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputMonoSystem : MonoBehaviour, IInputMonoSystem
    {
        [SerializeField] private PlayerInput _input;

        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _jumpAction;
        private InputAction _interactAction;
        private InputAction _leftMouseAction;
        private InputAction _rightMouseAction;
        private InputAction _restartAction;

        public UnityEvent JumpAction { get; private set; }
        public UnityEvent InteractCallback { get; private set; }
        public UnityEvent LeftMouseAction { get; private set; }
        public UnityEvent RightMouseAction { get; private set; }
        public UnityEvent RestartAction { get; private set; }

        public Vector2 RawMovement { get; private set; }
        public Vector2 RawLook { get; private set; }

        private void HandleMoveAction(InputAction.CallbackContext e)
        {
            RawMovement = e.ReadValue<Vector2>();
        }

        private void HandleLookAction(InputAction.CallbackContext e)
        {
            RawLook = e.ReadValue<Vector2>();
        }

        private void HandleInteractAction(InputAction.CallbackContext e)
        {
            InteractCallback.Invoke();
        }

        private void HandleLeftMouseAction(InputAction.CallbackContext e)
        {
            LeftMouseAction.Invoke();
        }

        private void HandleRightMouseAction(InputAction.CallbackContext e)
        {
            RightMouseAction.Invoke();
        }
        
        private void HandleRestartAction(InputAction.CallbackContext e)
        {
            RestartAction.Invoke();
        }

        private void HandleJumpAction(InputAction.CallbackContext e)
        {
            JumpAction.Invoke();
        }

        private void Awake()
        {
            if (!_input) _input = GetComponent<PlayerInput>();

            JumpAction       = new UnityEvent();
            InteractCallback = new UnityEvent();
            LeftMouseAction  = new UnityEvent();
            RightMouseAction = new UnityEvent();
            RestartAction = new UnityEvent();

            _moveAction       = _input.actions["Move"];
            _lookAction       = _input.actions["Look"];
            _jumpAction       = _input.actions["Jump"];
            _interactAction   = _input.actions["Interact"];
            _leftMouseAction  = _input.actions["LMouse"];
            _rightMouseAction = _input.actions["RMouse"];
            _restartAction    = _input.actions["Restart"];

            _moveAction.performed       += HandleMoveAction;
            _lookAction.performed       += HandleLookAction;
            _jumpAction.performed       += HandleJumpAction;
            _interactAction.performed   += HandleInteractAction;
            _leftMouseAction.performed  += HandleLeftMouseAction;
            _rightMouseAction.performed += HandleRightMouseAction;
            _restartAction.performed    += HandleRestartAction;
        }

        private void OnDestroy()
        {
            _moveAction.performed       -= HandleMoveAction;
            _lookAction.performed       -= HandleLookAction;
            _jumpAction.performed       -= HandleJumpAction;
            _interactAction.performed   -= HandleInteractAction;
            _leftMouseAction.performed  -= HandleLeftMouseAction;
            _rightMouseAction.performed -= HandleRightMouseAction;
        }
    }
}
