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

        private Transform _levelDoors;

        private List<Replayer> _clones = new();

        private int _levelId = 0;
        private Level _level = null;
        private List<Level> _levels = new();
        
        private Player.Controller _player = null;

        private void Start()
        {
            _levelDoors = GameObject.Find("LevelDoors").transform;
            for (int i = 0; i < _levelDoors.childCount; i++) _levelDoors.GetChild(i).gameObject.SetActive(false);
            _player = FindAnyObjectByType<Player.Controller>();
            _clonePrefab = Resources.Load<Replayer>("Prefabs/Clone");
            GameManager.GetMonoSystem<IInputMonoSystem>().RestartAction.AddListener(Restart);
            _levels = FindObjectsByType<Level>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID).Reverse().ToList();
            _levels.ForEach(l => Debug.Log(l.name));
            _levels.ForEach(l => l.gameObject.SetActive(false));
            LoadLevel(_levels[0]);
            Restart(true);
            
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
            _level.gameObject.SetActive(true);
        }

        public void NextLevel()
        {
            _levelDoors.GetChild(_levelId).gameObject.SetActive(true);
            _levelId += 1;
            _level.gameObject.SetActive(false);
            if (_levelId >= _levels.Count) return;
            LoadLevel(_levels[_levelId]);
            RemoveClones();
            GameManager.GetMonoSystem<IRecorderMonoSystem>().ClearAllSlots();
            Restart(false);
        }

        public void RestartAndRecord()
        {
            GameManager.GetMonoSystem<IRecorderMonoSystem>().SetSelectedRecording(null);
            Restart(true);
            _player.GetComponent<Recorder>().StartRecord();
        }

        private void RemoveClones()
        {
            _clones.Where(c => c).ForEach(c => Destroy(c.gameObject));
            _clones.Clear();
        }

        public void Restart() => Restart(true);
        public void Restart(bool movePlayer)
        {
            Debug.Log("Restart Level!");
            _level.Components.ForEach(c => c.ResetState());
            if (movePlayer) _player.SetTransform(_level.StartPosition);
            Recording[] recordings = GameManager.GetMonoSystem<IRecorderMonoSystem>().ActiveRecordings();

            RemoveClones();

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
