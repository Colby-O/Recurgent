using UnityEngine;

namespace Recursive.Player
{
    public class Replayer : MonoBehaviour
    {
        private Recording _recording;
        private Stopwatch _stopwatch = new();

        private Frame _prev, _next;
        private int _ptr = 0;
        private bool _done = false;

        public void SetRecording(Recording rec) => _recording = rec;

        public void Play()
        {
            _stopwatch.Reset();
            _stopwatch.Unpause();
            _ptr = 0;
            _done = false;
            _prev = _recording.GetFrame(_ptr);
            _next = _recording.GetFrame(_ptr + 1);
        }

        public void Pause() => _stopwatch.Pause();
        public void Unpause() => _stopwatch.Unpause();

        private void FixedUpdate()
        {
            if (_done || _stopwatch.IsPaused()) return;
            _ptr += 1;
            if (_ptr + 1 >= _recording.FrameCount())
            {
                _done = true;
                return;
            }
            _prev = _recording.GetFrame(_ptr);
            _next = _recording.GetFrame(_ptr + 1);
        }

        private void Update()
        {
            if (_done) return;
            float t = (_stopwatch.Now() - _prev.Time) / (_next.Time - _prev.Time);
            transform.position = Vector3.Lerp(_prev.Position, _next.Position, t);
            transform.rotation = Quaternion.Lerp(_prev.Rotation, _next.Rotation, t);
        }
    }
}
