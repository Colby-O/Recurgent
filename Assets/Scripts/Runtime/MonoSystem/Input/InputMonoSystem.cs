using PlazmaGames.Core;
using PlazmaGames.UI;
using PlazmaGames.UI.Views;
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
        private InputAction _slot1Action;
        private InputAction _slot2Action;
        private InputAction _slot3Action;
        private InputAction _slot4Action;
        private InputAction _restartAction;

        public UnityEvent JumpAction { get; private set; }
        public UnityEvent InteractCallback { get; private set; }
        public UnityEvent LeftMouseAction { get; private set; }
        public UnityEvent RightMouseAction { get; private set; }
        public UnityEvent RestartAction { get; private set; }

        public UnityEvent Slot1Action { get; private set; }
        public UnityEvent Slot2Action { get; private set; }
        public UnityEvent Slot3Action { get; private set; }
        public UnityEvent Slot4Action { get; private set; }

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

        private void HandleSlot1Action(InputAction.CallbackContext e)
        {
            if (RecursiveGameManager.IsPaused || RecursiveGameManager.Recorder.IsRecording()) return;
            Slot1Action.Invoke();
        }

        private void HandleSlot2Action(InputAction.CallbackContext e)
        {
            if (RecursiveGameManager.IsPaused || RecursiveGameManager.Recorder.IsRecording()) return;
            Slot2Action.Invoke();
        }

        private void HandleSlot3Action(InputAction.CallbackContext e)
        {
            if (RecursiveGameManager.IsPaused || RecursiveGameManager.Recorder.IsRecording()) return;
            Slot3Action.Invoke();
        }
        private void HandleSlot4Action(InputAction.CallbackContext e)
        {
            if (RecursiveGameManager.IsPaused || RecursiveGameManager.Recorder.IsRecording()) return;
            Slot4Action.Invoke();
        }


        private void Awake()
        {
            if (!_input) _input = GetComponent<PlayerInput>();

            JumpAction       = new UnityEvent();
            InteractCallback = new UnityEvent();
            LeftMouseAction  = new UnityEvent();
            RightMouseAction = new UnityEvent();
            Slot1Action      = new UnityEvent();
            Slot2Action      = new UnityEvent();
            Slot3Action      = new UnityEvent();
            Slot4Action      = new UnityEvent();
            RestartAction = new UnityEvent();

            _moveAction       = _input.actions["Move"];
            _lookAction       = _input.actions["Look"];
            _jumpAction       = _input.actions["Jump"];
            _interactAction   = _input.actions["Interact"];
            _leftMouseAction  = _input.actions["LMouse"];
            _rightMouseAction = _input.actions["RMouse"];
            _slot1Action      = _input.actions["Slot1"];
            _slot2Action      = _input.actions["Slot2"];
            _slot3Action      = _input.actions["Slot3"];
            _slot4Action      = _input.actions["Slot4"];
            _restartAction    = _input.actions["Restart"];

            _moveAction.performed       += HandleMoveAction;
            _lookAction.performed       += HandleLookAction;
            _jumpAction.performed       += HandleJumpAction;
            _interactAction.performed   += HandleInteractAction;
            _leftMouseAction.performed  += HandleLeftMouseAction;
            _rightMouseAction.performed += HandleRightMouseAction;
            _slot1Action.performed      += HandleSlot1Action;
            _slot2Action.performed      += HandleSlot2Action;
            _slot3Action.performed      += HandleSlot3Action;
            _slot4Action.performed      += HandleSlot4Action;

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
            _slot1Action.performed      -= HandleSlot1Action;
            _slot2Action.performed      -= HandleSlot2Action;
            _slot3Action.performed      -= HandleSlot3Action;
            _slot4Action.performed      -= HandleSlot4Action;
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame)
            {
                if (
                    !GameManager.GetMonoSystem<IUIMonoSystem>().GetCurrentViewIs<PausedView>() &&
                    !GameManager.GetMonoSystem<IUIMonoSystem>().GetCurrentViewIs<SettingsView>() &&
                    !GameManager.GetMonoSystem<IUIMonoSystem>().GetCurrentViewIs<MainMenuView>()
                ) GameManager.GetMonoSystem<IUIMonoSystem>().Show<PausedView>();
                else if (
                    !GameManager.GetMonoSystem<IUIMonoSystem>().GetCurrentViewIs<MainMenuView>() &&
                    RecursiveGameManager.HasStarted
                ) GameManager.GetMonoSystem<IUIMonoSystem>().GetView<PausedView>().Resume();
            }
        }
    }
}
