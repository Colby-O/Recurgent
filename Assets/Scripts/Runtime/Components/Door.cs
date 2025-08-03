using PlazmaGames.Audio;
using PlazmaGames.Core;
using Recursive.Components;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Recursive
{
    public class Door : MonoBehaviour, Components.IComponent
    {
        [SerializeField] private GameObject _actuator;
        [SerializeField] private GameObject _model;
        [SerializeField] private bool _defaultState;
        [SerializeField] private AudioClip _onSolveClip;

        private void OnEnable() => _actuator.GetComponent<IActuator>().Bind(SetState);
        private void OnDisable() => _actuator.GetComponent<IActuator>().Unbind(SetState);

        private void SetState(bool state)
        {
            if (state) Open();
            else Close();
        }
        
        public void ResetState()
        {
            _model.SetActive(!_defaultState);
        }

        private void Open()
        {
            if (_onSolveClip) GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio(_onSolveClip, PlazmaGames.Audio.AudioType.Sfx, false, true);
            _model.SetActive(false);
        }

        private void Close()
        {
            _model.SetActive(true);
        }

    }
}
