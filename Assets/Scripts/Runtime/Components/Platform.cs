using System;
using PlazmaGames.Attribute;
using System.Collections.Generic;
using Recursive.Components;
using UnityEngine;

namespace Recursive
{
    public abstract class Platform : MonoBehaviour, Components.IComponent
    {
        [SerializeField] protected float _speed;
        [SerializeField] protected List<Transform> _path;
        [SerializeField] protected GameObject _actuator;
        [SerializeField] protected bool _initialState;

        [SerializeField, ReadOnly] protected Transform _currentTarget;
        [SerializeField, ReadOnly] protected Transform _nextTarget;
        [SerializeField, ReadOnly] protected int _index;
        [SerializeField, ReadOnly] protected float _t;
        [SerializeField, ReadOnly] protected int _direction = 1;
        [SerializeField, ReadOnly] protected bool _isActivated;
        [SerializeField, ReadOnly] protected Vector3 _initialPosition;

        [SerializeField, InspectorButton("Activate")] protected bool _activate;
        [SerializeField, InspectorButton("Deactivate")] protected bool _deactivate;
        
        private void OnEnable() => _actuator.GetComponent<IActuator>().Bind(SetState);
        private void OnDisable() => _actuator.GetComponent<IActuator>().Unbind(SetState);

        protected virtual void Start()
        {
            _initialPosition = transform.position;
        }

        private void SetState(bool state)
        {
            if (state) Activate();
            else Deactivate();
        }
        
        public virtual void ResetState()
        {
            _currentTarget = null;
            _nextTarget = null;
            _index = 0;
            _t = 0;
            _direction = 1;
            _isActivated = _initialState;
            transform.position = _initialPosition;
            if (_initialState) Activate();
        }

        protected abstract void NextTarget();
        protected abstract void RefreshTarget();

        public abstract void Activate();
        public abstract void Deactivate();

        public abstract bool IsActivated();

        private Player.Controller _player = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Interactor") && other.TryGetComponent(out Player.Controller player))
            {
                _player = player;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Interactor") && other.TryGetComponent(out Player.Controller player))
            {
                _player = null;
            }
        }

        protected void Update()
        {
            if (!IsActivated()) return;
            Vector3 dir = Vector3.Normalize(_nextTarget.position - _currentTarget.position);
            if (_player)
            {
                _player.Move(dir * (_speed * Time.deltaTime));
            }
            if (_currentTarget && _nextTarget)
            {
                _t += Time.deltaTime * _speed / (_nextTarget.position - _currentTarget.position).magnitude;
                transform.position = Vector3.Lerp(_currentTarget.position, _nextTarget.position, _t);

                if (_t >= 1)
                {
                    transform.position = _nextTarget.position;
                    _t = 0f;
                    NextTarget();
                }
            }
        }

    }
}
