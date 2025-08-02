using PlazmaGames.Attribute;
using PlazmaGames.Core;
using Recursive.MonoSystem;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace Recursive.Player
{
    public class Recorder : MonoBehaviour
    {
        [SerializeField] private float _timeStep = 0.1f;
        [SerializeField] private float _timeLimit = 20f;

        [SerializeField] private int _bufferSize = 1200; 
        [SerializeField, ReadOnly] private Frame[] _frames;
        [SerializeField, ReadOnly] private int _currentIndex = 0;

        [SerializeField, ReadOnly] float _recordStartTime;

        [SerializeField, ReadOnly] private bool _isRecording;

        private void Record()
        {
            if (!_isRecording) StartRecord();
            else EndRecord();
        }

        private void StartRecord()
        {
            for (int i = 0; i < _bufferSize; i++) _frames[i] = new Frame();
            _currentIndex = 0;

            _recordStartTime = Time.time;
            _isRecording = true;
        }

        private void EndRecord()
        {
            _isRecording = false;
        }

        private void Awake()
        {
            GameManager.GetMonoSystem<IInputMonoSystem>().LeftMouseAction.AddListener(Record);

            _bufferSize = Mathf.CeilToInt(_timeLimit / _timeStep);

            _frames = new Frame[_bufferSize];
            for (int i = 0; i < _bufferSize; i++) _frames[i] = new Frame();
        }

        private void FixedUpdate()
        {
            if (_isRecording && _currentIndex < _bufferSize)
            {

                _frames[_currentIndex++] = new Frame(Time.time - _recordStartTime, transform.position, transform.rotation, false, false);
            }
        }
    }
}
