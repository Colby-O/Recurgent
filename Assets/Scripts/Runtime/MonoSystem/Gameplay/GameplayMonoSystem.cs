using System.Collections.Generic;
using System.Linq;
using PlazmaGames.Core;
using UnityEngine;

namespace Recursive.MonoSystem
{
    public class GameplayMonoSystem : MonoBehaviour, IGameplayMonoSystem
    {
        private Level _level = null;
        
        private Player.Controller _player = null;

        private void Start()
        {
            GameManager.GetMonoSystem<IInputMonoSystem>().RestartAction.AddListener(Restart);
            LoadLevel(FindAnyObjectByType<Level>());
            _player = FindAnyObjectByType<Player.Controller>();
        }

        public void LoadLevel(Level level)
        {
            _level = level;
        }

        public void Restart()
        {
            Debug.Log("Restart Level!");
            _level.Components.ForEach(c => c.ResetState());
            _player.SetTransform(_level.StartPosition);
        }
    }
}
