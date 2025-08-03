using System.Collections.Generic;
using System.Linq;
using PlazmaGames.Core;
using PlazmaGames.Core.Utils;
using Recursive.Player;
using UnityEngine;

namespace Recursive.MonoSystem
{
    public class GameplayMonoSystem : MonoBehaviour, IGameplayMonoSystem
    {
        private Replayer _clonePrefab;

        private List<Replayer> _clones = new();
        
        private Level _level = null;
        
        private Player.Controller _player = null;

        private void Start()
        {
            _clonePrefab = Resources.Load<Replayer>("Prefabs/Clone");
            GameManager.GetMonoSystem<IInputMonoSystem>().RestartAction.AddListener(Restart);
            LoadLevel(FindAnyObjectByType<Level>());
            _player = FindAnyObjectByType<Player.Controller>();
            
            GameManager.GetMonoSystem<IInputMonoSystem>().RightMouseAction.AddListener(DoRecord);
        }

        private void DoRecord()
        {
            Recorder recorder = _player.GetComponent<Recorder>();
            if (recorder.IsRecording())
            {
                recorder.EndRecord();
            }
            else
            {
                RestartAndRecord();
            }
        }

        public void LoadLevel(Level level)
        {
            _level = level;
        }

        public void RestartAndRecord()
        {
            Restart();
            _player.GetComponent<Recorder>().StartRecord();
        }

        public void Restart()
        {
            Debug.Log("Restart Level!");
            _level.Components.ForEach(c => c.ResetState());
            _player.SetTransform(_level.StartPosition);
            Recording[] recordings = GameManager.GetMonoSystem<IRecorderMonoSystem>().ActiveRecordings();
            
            _clones.Where(c => c).ForEach(c => Destroy(c.gameObject));
            _clones.Clear();

            foreach (Recording rec in recordings)
            {
                Replayer rp = GameObject.Instantiate(_clonePrefab);
                rp.SetRecording(rec);
                rp.Play();
                _clones.Add(rp);
            }
            
        }
    }
}
