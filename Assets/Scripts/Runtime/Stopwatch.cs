using UnityEngine;

namespace Recursive
{
    public class Stopwatch
    {
        private bool _isPaused = true;
        private float _startTime;
        private float _pauseStartTime;
        private float _pauseDuration;

        private float GetRawTime()
        {
            return Time.time - _startTime;
        }
        
        public float Pause()
        {
            if (_isPaused) return -1;
            _isPaused = true;
            _pauseStartTime = GetRawTime();
            return Now();
        }

        public float Unpause()
        {
            if (!_isPaused) return -1;
            float cTime = GetRawTime();
            _isPaused = false;
            _pauseDuration += cTime - _pauseStartTime;
            return Now();
        }

        public float Now()
        {
            if (_isPaused) {
                return _pauseStartTime - _pauseDuration;
            } else {
                return GetRawTime() - _pauseDuration;
            }
        }

        public void Reset()
        {
            _startTime = Time.time;
            _isPaused = true;
            _pauseStartTime = 0;
            _pauseDuration = 0;
        }

        public void Set(float time) {
            _pauseDuration = (Now() + _pauseDuration) - time;
        }

        public void Skip(float duration) {
            _pauseDuration -= duration;
        }

        public bool IsPaused() => _isPaused;
    }
}
