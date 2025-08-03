using PlazmaGames.Audio;
using PlazmaGames.Core;
using Recursive.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Recursive
{
    public class Lever : MonoBehaviour, IActuator, Components.IComponent
    {
        [SerializeField] private List<MeshRenderer> _wires;
        [SerializeField] private List<MeshRenderer> _wiresLive;

        [SerializeField] private AudioClip _onClip;
        [SerializeField] private AudioClip _offClip;

        private List<Action<bool>> _callbacks = new();
        private int _interactorsInside = 0;
        private bool _state = false;
        private float _t = 0;

        [SerializeField] private Transform _model;
        [SerializeField] private Transform _modelOnRotation;
        
        [SerializeField] private float _flipTime = 0.6f;

        private Quaternion _offRotation;
        private Quaternion _onRotation;

        private void Start()
        {
            _offRotation = _model.rotation;
            _onRotation = _modelOnRotation.rotation;
        }

        private void Update()
        {
            if (_state) _t += Time.deltaTime / _flipTime;
            else _t -= Time.deltaTime / _flipTime;
            if (_t < 0) _t = 0;
            if (_t > 1) _t = 1;
            if (_t > 0 && _t < 1)
            {
                _model.rotation = Quaternion.Lerp(_offRotation, _onRotation, _t);
            }
        }
        
        public void Bind(Action<bool> callback) => _callbacks.Add(callback);
        public void Unbind(Action<bool> callback) => _callbacks.Remove(callback);
        
        private void Toggle()
        {
            _state = !_state;

            if (_state)
            {
                if (_onClip) GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio(_onClip, PlazmaGames.Audio.AudioType.Sfx, false, true);
                if (_wires != null) foreach (var w in _wires) w.material.SetColor("_BaseColor", Color.cyan);
                if (_wiresLive != null) foreach (var w in _wiresLive) w.material.SetColor("_BaseColor", Color.red);
            }
            else
            {
                if (_offClip) GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio(_offClip, PlazmaGames.Audio.AudioType.Sfx, false, true);
                if (_wires != null) foreach (var w in _wires) w.material.SetColor("_BaseColor", Color.red);
                if (_wiresLive != null) foreach (var w in _wiresLive) w.material.SetColor("_BaseColor", Color.cyan);
            }

                _callbacks.ForEach(c => c.Invoke(_state));
        }
        
        public void ResetState()
        {
            _state = false;
            _interactorsInside = 0;
            _t = 0;
            _model.rotation = _offRotation;

            if (_wires != null) foreach (var w in _wires) w.material.SetColor("_BaseColor", Color.red);
            if (_wiresLive != null) foreach (var w in _wiresLive) w.material.SetColor("_BaseColor", Color.cyan);
        }

        private void Awake()
        {
            if (_wires != null) foreach (var w in _wires) w.material.SetColor("_BaseColor", Color.red);
            if (_wiresLive != null) foreach (var w in _wiresLive) w.material.SetColor("_BaseColor", Color.cyan);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Interactor"))
            {
                if (_interactorsInside == 0) Toggle();
                _interactorsInside += 1;
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Interactor"))
            {
                if (_interactorsInside > 0) _interactorsInside -= 1;
            }
        }

    }
}
