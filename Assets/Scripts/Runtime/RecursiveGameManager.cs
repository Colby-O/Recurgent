using PlazmaGames.Animation;
using PlazmaGames.Audio;
using PlazmaGames.Core;
using PlazmaGames.UI;
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

        private void AttachMonoSystems()
        {
            AddMonoSystem<UIMonoSystem, IUIMonoSystem>(_uiSystem);
            AddMonoSystem<AnimationMonoSystem, IAnimationMonoSystem>(_animSystem);
            AddMonoSystem<AudioMonoSystem, IAudioMonoSystem>(_audioSystem);
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
