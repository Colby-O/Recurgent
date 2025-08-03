using PlazmaGames.Core;
using PlazmaGames.UI;
using Recursive.UI;
using UnityEngine;

namespace Recursive
{
    public class PausedView : View
    {
        [SerializeField] private EventButton _resume;
        [SerializeField] private EventButton _settings;
        [SerializeField] private EventButton _quit;

        [SerializeField] private GameObject _menuCamera;
        [SerializeField] private GameObject _level1;

        private bool _hideLevel = false;

        public void Resume()
        {
            GameManager.GetMonoSystem<IUIMonoSystem>().Show<GameView>();
            RecursiveGameManager.IsPaused = false;
            _menuCamera.SetActive(false);
        }

        private void Settings()
        {
            GameManager.GetMonoSystem<IUIMonoSystem>().Show<SettingsView>();
        }

        private void Quit()
        {
            Application.Quit();
        }

        public override void Init()
        {
            _resume.onPointerDown.AddListener(Resume);
            _settings.onPointerDown.AddListener(Settings);
            _quit.onPointerDown.AddListener(Quit);
        }

        public override void Hide()
        {
            base.Hide();
            _level1.SetActive(!_hideLevel);
        }

        public override void Show()
        {
            base.Show();
            _hideLevel = !_level1.activeSelf;
            if (_hideLevel) _level1.SetActive(_hideLevel);
            RecursiveGameManager.IsPaused = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            RecursiveGameManager.UseCustomCursor();
            _menuCamera.SetActive(true);
        }
    }
}
