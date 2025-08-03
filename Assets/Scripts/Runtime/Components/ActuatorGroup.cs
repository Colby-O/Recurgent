using System;
using System.Collections.Generic;
using PlazmaGames.Attribute;
using Recursive.Components;
using UnityEngine;

namespace Recursive
{
    public class ActuatorGroup : MonoBehaviour, IActuator
    {
        private enum Type
        {
            Or,
            Nor,
            Xor,
            And,
            Nand,
        }
        
        private List<Action<bool>> _callbacks = new();
        private bool _state = false;

        [SerializeField] private Type _type = Type.Or;
        [SerializeField] private List<GameObject> _actuators = new();
        
        public void Bind(Action<bool> callback) => _callbacks.Add(callback);
        public void Unbind(Action<bool> callback) => _callbacks.Remove(callback);

        [SerializeField, ReadOnly] private int _onCount = 0;

        private void OnEnable()
        {
            _actuators.ForEach(a => a.GetComponent<IActuator>().Bind(SetState));
        }
        
        private void OnDisable()
        {
            _actuators.ForEach(a => a.GetComponent<IActuator>().Unbind(SetState));
        }

        private void SetState(bool state)
        {
            if (state) _onCount += 1;
            else _onCount -= 1;
            
            bool res;

            switch (_type)
            {
                case Type.Or:
                    res = _onCount > 0;
                    break;
                case Type.Nor:
                    res = _onCount == 0;
                    break;
                case Type.Xor:
                    res = _onCount == 1;
                    break;
                case Type.And:
                    res = _onCount == _actuators.Count;
                    break;
                case Type.Nand:
                    res = _onCount != _actuators.Count;
                    break;
                default: goto case Type.Or;
            }

            if (res != _state)
            {
                _callbacks.ForEach(c => c.Invoke(res));
                _state = res;
            }
        }

    }
}
