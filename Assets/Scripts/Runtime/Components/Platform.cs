using PlazmaGames.Attribute;
using System.Collections.Generic;
using UnityEngine;

namespace Recursive
{
    public abstract class Platform : MonoBehaviour
    {
        [SerializeField] protected float _speed;
        [SerializeField] protected List<Transform> _path;
        [SerializeField] protected GameObject _activator;

        [SerializeField, ReadOnly] protected Transform _currentTarget;
        [SerializeField, ReadOnly] protected Transform _nextTarget;
        [SerializeField, ReadOnly] protected int _index;
        [SerializeField, ReadOnly] protected float _t;
        [SerializeField, ReadOnly] protected int _direction = 1;
        [SerializeField, ReadOnly] protected bool _isActivated;

        [SerializeField, InspectorButton("Activate")] protected bool _activate;
        [SerializeField, InspectorButton("Deactivate")] protected bool _deactivate;

        protected abstract void NextTarget();
        protected abstract void RefreshTarget();

        public abstract void Activate();
        public abstract void Deactivate();

        public abstract bool IsActivated();

        protected void Update()
        {
            if (_currentTarget && _nextTarget && IsActivated())
            {
                _t += Time.deltaTime * _speed;
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
