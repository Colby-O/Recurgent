using PlazmaGames.Attribute;
using PlazmaGames.Core;
using Recursive.MonoSystem;
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

		private void Record()
		{
			if (!_isRecording) StartRecord();
			else EndRecord();
		}

		private void StartRecord()
        {
            _recording.Clear();

			_recordStartTime = Time.time;
			_isRecording = true;
		}

		private void EndRecord()
		{
			_isRecording = false;
            Replayer rp = GameObject.Instantiate(_clonePrefab).transform.GetComponent<Replayer>();
            rp.SetRecording(_recording.Clone());
            rp.Play();
        }

		private void Awake()
		{
			GameManager.GetMonoSystem<IInputMonoSystem>().LeftMouseAction.AddListener(Record);

//            _frames = new Frame[_bufferSize];
//            for (int i = 0; i < _bufferSize; i++) _frames[i] = new Frame();
		}

		private void FixedUpdate()
		{
			if (_isRecording)
			{
				_recording.AddFrame(new Frame(Time.time - _recordStartTime, transform.position, transform.rotation));
			}
		}
	}
}
