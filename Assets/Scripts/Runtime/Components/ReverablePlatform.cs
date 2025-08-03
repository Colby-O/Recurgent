using UnityEngine;

namespace Recursive
{
    public class ReversablePlatform : Platform
    {
        public override void Activate()
        {
            if (_isActivated) return;

            _isActivated = true;
            _direction = 1;

            _index -= 1;

            if (_t != 0f)
            {
                _t = 1f - _t;
                SwapTargets();
            }

            if (_currentTarget == null || _nextTarget == null)
                RefreshTarget();
        }

        public override void Deactivate()
        {
            if (!_isActivated) return;

            _isActivated = false;
            _direction = -1;

            _index += 1;

            if (_t != 0f)
            {
                _t = 1f - _t;
                SwapTargets();
            }

            if (_currentTarget == null || _nextTarget == null) RefreshTarget();
        }

        public override bool IsActivated()
        {
            return _currentTarget != null && _nextTarget != null;
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
                _index = _path.Count - 1;
                if (_isActivated)
                {
                    _currentTarget = null;
                    _nextTarget = null;
                    return;
                }
            }
            else if (_index <= 0)
            {
                _index = 0;
                if (!_isActivated)
                {
                    _currentTarget = null;
                    _nextTarget = null;
                    return;
                }
            }

            _currentTarget = _path[_index];
            _nextTarget = _path[_index + _direction];
        }

        private void SwapTargets()
        {
            Transform temp = _currentTarget;
            _currentTarget = _nextTarget;
            _nextTarget = temp;
        }
    }
}
