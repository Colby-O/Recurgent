using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Recursive.Components;
using UnityEngine;

namespace Recursive
{
    public class Lever : MonoBehaviour, IActuator, Components.IComponent
    {
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
            _callbacks.ForEach(c => c.Invoke(_state));
        }
        
        public void ResetState()
        {
            _state = false;
            _interactorsInside = 0;
            _t = 0;
            _model.rotation = _offRotation;
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
