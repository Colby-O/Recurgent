using PlazmaGames.Attribute;
using PlazmaGames.Core;
using PlazmaGames.UI;
using Recursive.MonoSystem;
using Recursive.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace Recursive.Player
{
	public class Recorder : MonoBehaviour
    {
        [SerializeField] private GameObject _clonePrefab;
        
		[SerializeField] private float _timeStep = 0.1f;
		[SerializeField] private float _timeLimit = 20f;

        private Recording _recording = new();

		[SerializeField, ReadOnly] float _recordStartTime;

		[SerializeField, ReadOnly] private bool _isRecording;

		private GameView _gameView;
		private IRecorderMonoSystem _recorderMS;

		private void Record()
		{
			if (!_isRecording) StartRecord();
			else EndRecord();
		}

		public void StartRecord()
        {
            _recording.Clear();

			_recordStartTime = Time.time;

			_gameView.RecorderOn();
			_gameView.EnableTimer();
			_gameView.SetTimer(0f);

			_isRecording = true;
		}

		public void EndRecord()
		{
			_isRecording = false;

			_recorderMS.SetSelectedRecording(_recording.Clone());
			_gameView.RecorderOff();
            _gameView.DisableTimer();
            _gameView.SetTimer(0f);

        }

		private void Awake()
		{
//            _frames = new Frame[_bufferSize];
//            for (int i = 0; i < _bufferSize; i++) _frames[i] = new Frame();
		}

        private void Start()
        {
            _recorderMS = GameManager.GetMonoSystem<IRecorderMonoSystem>();
            _gameView = GameManager.GetMonoSystem<IUIMonoSystem>().GetView<GameView>();
        }

        private void Update()
        {
            _gameView.SetTimer(Time.time - _recordStartTime);
        }

        private void FixedUpdate()
		{
			if (_isRecording)
			{
				_recording.AddFrame(new Frame(Time.time - _recordStartTime, transform.position, transform.rotation));
			}
		}

        public bool IsRecording() => _isRecording;
    }
}
