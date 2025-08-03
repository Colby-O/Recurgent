using PlazmaGames.Audio;
using PlazmaGames.Core;
using PlazmaGames.UI;
using Recursive.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Recursive
{
    public class SettingsView : View
    {
        [SerializeField] private Slider _volume;
        [SerializeField] private Slider _sensitivity;
        [SerializeField] private EventButton _back;

        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private GameObject _level1;

        private bool _hideLevel = false;

        private void OnVolumeChanged(float val)
        {
            GameManager.GetMonoSystem<IAudioMonoSystem>().SetOverallVolume(val);
        }
        private float GetSensitivityAdjustedValue(float input, float exp = 2f)
        {
            return Mathf.Pow(input, exp);
        }
        private void OnSensitivityChanged(float val)
        {
            float sens = Mathf.Lerp(0.01f, 1f, GetSensitivityAdjustedValue(val));
            _playerSettings.Sensitivity = new Vector2(sens, sens);
        }

        private void Back()
        {
            GameManager.GetMonoSystem<IUIMonoSystem>().ShowLast();
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
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        public override void Init()
        {
            _back.onPointerDown.AddListener(Back);
            _volume.onValueChanged.AddListener(OnVolumeChanged);
            _sensitivity.onValueChanged.AddListener(OnSensitivityChanged);

            _sensitivity.value = 0.3f;
            _volume.value = 0.5f;

            OnSensitivityChanged(0.3f);
            OnVolumeChanged(0.5f);
        }
    }
}
