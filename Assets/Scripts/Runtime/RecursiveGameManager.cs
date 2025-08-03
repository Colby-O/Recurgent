using PlazmaGames.Animation;
using PlazmaGames.Audio;
using PlazmaGames.Core;
using PlazmaGames.UI;
using Recursive.MonoSystem;
using Recursive.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Recursive
{
    public class RecursiveGameManager : GameManager
    {
        [SerializeField] private GameObject _monoSystemHolder;

        [Header("MonoSystems")]
        [SerializeField] private UIMonoSystem _uiSystem;
        [SerializeField] private AnimationMonoSystem _animSystem;
        [SerializeField] private AudioMonoSystem _audioSystem;
        [SerializeField] private InputMonoSystem _inputSystem;
        [SerializeField] private RecorderMonoSystem _recorderSystem;
        [SerializeField] private GameplayMonoSystem _gameplaySystem;
        [SerializeField] private DialogueMonoSystem _dialogueSystem;

        [SerializeField] private Texture2D _cursor;

        public static bool IsPaused = true;
        public static bool HasStarted = false;

        public static Recorder Recorder { get; set; }

        public static void UseCustomCursor()
        {
            if (Instance) Cursor.SetCursor(((RecursiveGameManager)Instance)._cursor, Vector2.zero, CursorMode.Auto);
        }

        private void AttachMonoSystems()
        {
            AddMonoSystem<UIMonoSystem, IUIMonoSystem>(_uiSystem);
            AddMonoSystem<AnimationMonoSystem, IAnimationMonoSystem>(_animSystem);
            AddMonoSystem<AudioMonoSystem, IAudioMonoSystem>(_audioSystem);
            AddMonoSystem<InputMonoSystem, IInputMonoSystem>(_inputSystem);
            AddMonoSystem<RecorderMonoSystem, IRecorderMonoSystem>(_recorderSystem);
            AddMonoSystem<GameplayMonoSystem, IGameplayMonoSystem>(_gameplaySystem);
            AddMonoSystem<DialogueMonoSystem, IDialogueMonoSystem>(_dialogueSystem);
        }

        public override string GetApplicationName()
        {
            return nameof(RecursiveGameManager);
        }

        public override string GetApplicationVersion()
        {
            return "v0.0.1";
        }

        protected override void OnInitalized()
        {
            AttachMonoSystems();

            _monoSystemHolder.SetActive(true);
        }

        private void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {

        }

        private void OnSceneUnload(Scene scene)
        {
            RemoveAllEventListeners();
        }

        private void Awake()
        {
            Application.runInBackground = true;
        }

        private void Start()
        {
            Recorder = FindAnyObjectByType<Recorder>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoad;
            SceneManager.sceneUnloaded += OnSceneUnload;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
            SceneManager.sceneUnloaded -= OnSceneUnload;
        }
    }
}
