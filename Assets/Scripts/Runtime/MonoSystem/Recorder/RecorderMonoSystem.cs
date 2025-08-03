using PlazmaGames.Core;
using PlazmaGames.UI;
using Recursive.Player;
using Recursive.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Recursive.MonoSystem
{
    public class RecorderMonoSystem : MonoBehaviour, IRecorderMonoSystem
    {
        private const int NUMBER_OF_SLOTS = 4;

        [SerializeField] private Recording[] _recordings;
        [SerializeField] private int _selectedSlot = 0;

        private GameView _gameView;

        public void SelectSlot(int id)
        {
            if (id >= 4) return;

            _selectedSlot = id;
            _gameView.SelectSlot(id);
        }

        public void SetSelectedRecording(Recording recording)
        {
            _recordings[_selectedSlot] = recording;
            _gameView.SetSlotSaveState(_selectedSlot, recording != null);
        }

        public Recording GetSelectedRecording()
        {
            return _recordings[_selectedSlot];
        }

        public void ClearAllSlots()
        {
            for (int i = 0; i < NUMBER_OF_SLOTS; i++)
            {
                _recordings[i] = null;
                _gameView.SetSlotSaveState(_selectedSlot, false);
            }
            SelectSlot(0);
        }

        private void Awake()
        {
            _recordings = new Recording[NUMBER_OF_SLOTS];
        }

        private void Start()
        {
            _gameView = GameManager.GetMonoSystem<IUIMonoSystem>().GetView<GameView>();
            ClearAllSlots();

            GameManager.GetMonoSystem<IInputMonoSystem>().Slot1Action.AddListener(() => SelectSlot(0));
            GameManager.GetMonoSystem<IInputMonoSystem>().Slot2Action.AddListener(() => SelectSlot(1));
            GameManager.GetMonoSystem<IInputMonoSystem>().Slot3Action.AddListener(() => SelectSlot(2));
            GameManager.GetMonoSystem<IInputMonoSystem>().Slot4Action.AddListener(() => SelectSlot(3));
        }
    }
}
