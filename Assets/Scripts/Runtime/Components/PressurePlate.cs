using System;
using System.Collections.Generic;
using Recursive.Components;
using UnityEngine;
using IComponent = System.ComponentModel.IComponent;

namespace Recursive
{
    public class PressurePlate : MonoBehaviour, IActuator, Components.IComponent
    {
        private List<Action<bool>> _callbacks = new();
        private int _interactorsInside = 0;
        
        public void Bind(Action<bool> callback) => _callbacks.Add(callback);
        public void Unbind(Action<bool> callback) => _callbacks.Remove(callback);
        
        private void SetOn()
        {
            _callbacks.ForEach(c => c.Invoke(true));
        }
        private void SetOff()
        {
            _callbacks.ForEach(c => c.Invoke(false));
        }
        
        public void ResetState()
        {
            _interactorsInside = 0;
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

    }
}
