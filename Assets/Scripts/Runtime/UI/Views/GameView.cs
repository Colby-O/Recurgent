using PlazmaGames.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Recursive.UI
{
    public class GameView : View
    {
        [SerializeField] private GameObject _recordIcon;
        [SerializeField] private List<RecorderUISlot> _recorderSlots;
        [SerializeField] private GameObject _timer;
        [SerializeField] private TMP_Text _timerDisplay;

        public void RecorderOn()
        {
            _recordIcon.SetActive(true);
        }

        public void RecorderOff()
        {
            _recordIcon.SetActive(false);
        }

        public int GetSelectedSlot()
        {
            for (int i = 0; i < _recorderSlots.Count; i++) 
            {
                if (_recorderSlots[i].IsSelected()) return i;
            }

            return -1;
        }

        public void EnableTimer()
        {
            _timer.SetActive(true);
        }

        public void DisableTimer()
        {
            _timer.SetActive(false);
        }

        public void SetTimer(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);

            _timerDisplay.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        }

        public void SelectSlot(int slotIndex)
        {
            foreach (RecorderUISlot slot in  _recorderSlots) slot.Deselect();
            _recorderSlots[slotIndex].Select();
        }

        public void SetSlotSaveState(int slotIndex, bool hasSave)
        {
            _recorderSlots[slotIndex].SetSaveState(hasSave);
        }

        public override void Init()
        {
            RecorderOff();
        }
    }
}
