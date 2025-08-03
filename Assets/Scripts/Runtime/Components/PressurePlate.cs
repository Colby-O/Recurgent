using System;
using System.Collections.Generic;
using PlazmaGames.Audio;
using PlazmaGames.Core;
using Recursive.Components;
using UnityEngine;

namespace Recursive
{
    public class PressurePlate : MonoBehaviour, IActuator, Components.IComponent
    {
        [SerializeField] private List<MeshRenderer> _wires;
        [SerializeField] private List<MeshRenderer> _wiresLive;
        [SerializeField] private AudioClip _onClip;
        [SerializeField] private AudioClip _offClip;

        private List<Action<bool>> _callbacks = new();
        private int _interactorsInside = 0;
        
        public void Bind(Action<bool> callback) => _callbacks.Add(callback);
        public void Unbind(Action<bool> callback) => _callbacks.Remove(callback);
        
        private void SetOn()
        {
            if (_onClip) GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio(_onClip, PlazmaGames.Audio.AudioType.Sfx, false, true);
            if (_wires != null) foreach (var w in _wires) w.material.SetColor("_BaseColor", Color.cyan);
            if (_wiresLive != null) foreach (var w in _wiresLive) w.material.SetColor("_BaseColor", Color.red);
            _callbacks.ForEach(c => c.Invoke(true));
        }
        private void SetOff()
        {
            if (_offClip) GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio(_offClip, PlazmaGames.Audio.AudioType.Sfx, false, true);
            if (_wires != null) foreach (var w in _wires) w.material.SetColor("_BaseColor", Color.red);
            if (_wiresLive != null) foreach (var w in _wiresLive) w.material.SetColor("_BaseColor", Color.cyan);
            _callbacks.ForEach(c => c.Invoke(false));
        }
        
        public void ResetState()
        {
            _interactorsInside = 0;
            if (_wires != null) foreach (var w in _wires) w.material.SetColor("_BaseColor", Color.red);
            if (_wiresLive != null) foreach (var w in _wiresLive) w.material.SetColor("_BaseColor", Color.cyan);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Interactor"))
            {
                if (_interactorsInside == 0) SetOn();
                _interactorsInside += 1;
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Interactor") && _interactorsInside > 0)
            {
                _interactorsInside -= 1;
                if (_interactorsInside == 0) SetOff();
            }
        }
        private void Awake()
        {
            if (_wires != null) foreach (var w in _wires) w.material.SetColor("_BaseColor", Color.red);
            if (_wiresLive != null) foreach (var w in _wiresLive) w.material.SetColor("_BaseColor", Color.cyan);
        }
    }
}
