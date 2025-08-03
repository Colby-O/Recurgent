using PlazmaGames.Attribute;
using UnityEngine;

namespace Recursive
{
    public class MovablePlatform : Platform
    {
        [SerializeField] private bool _canLoop;

        public override void Activate()
        {
            _isActivated = true;
        }

        public override void Deactivate()
        {
            _isActivated = false;
        }

        public override bool IsActivated()
        {
            return _isActivated;
        }

        protected override void NextTarget()
        {
            if (_path == null || _path.Count < 2) return;

            _index += _direction;

            RefreshTarget();
        }

        protected override void RefreshTarget()
        {
            if (_index >= _path.Count - 1)
            {
                if (!_canLoop)
                {
                    _currentTarget = null;
                    _nextTarget = null;
                    return;
                }
                _index = _path.Count - 1;
                _direction = -1;
            }
            else if (_index <= 0)
            {
                _index = 0;
                _direction = 1;
            }

            _currentTarget = _path[_index];
            _nextTarget = _path[_index + _direction];
        }

        private void Start()
        {
            _currentTarget = _path[0];
            _nextTarget = _path[1];
        }
    }
}
