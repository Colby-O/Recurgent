using NUnit.Framework;
using PlazmaGames.Attribute;
using PlazmaGames.Core;
using PlazmaGames.UI;
using Recursive.MonoSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEditor.FilePathAttribute;

namespace Recursive.UI
{
    public class GameView : View
    {
        [SerializeField] private AudioSource _as;
        [SerializeField] private GameObject _recordIcon;
        [SerializeField] private List<RecorderUISlot> _recorderSlots;
        [SerializeField] private GameObject _timer;
        [SerializeField] private TMP_Text _timerDisplay;

        [Header("Dialogue")]
        [SerializeField] private GameObject _dialogueHolder;
        [SerializeField] private TMP_Text _dialogueAvatarName;
        [SerializeField] private TMP_Text _dialogue;
        [SerializeField] private GameObject _dialogueHint;
        [SerializeField] private float _typeSpeed = 0.1f;
        [SerializeField] private float _timeout = 4f;

        [SerializeField, ReadOnly] private bool _isWriting = false;
        [SerializeField, ReadOnly] private bool _isWritingDialogue = false;
        [SerializeField, ReadOnly] private float _timeSinceWriteStart = 0f;
        [SerializeField, ReadOnly] private string _currentMessage;
        [SerializeField, ReadOnly] private bool _showedMessage = false;

        IEnumerator TypeDialogue(string msg, float typeSpeed, TMP_Text target, UnityAction onFinished = null)
        {
            _isWriting = true;
            _currentMessage = msg;
            _showedMessage = false;

            _isWritingDialogue = true;

            _timeSinceWriteStart = 0f;

            if (_as) _as.Play();

            target.text = string.Empty;

            for (int i = 0; i < msg.Length; i++)
            {
                while (RecursiveGameManager.IsPaused)
                {
                    if (_as) _as.Stop();
                    yield return null;
                }

                if (_as && !_as.isPlaying) _as.Play();

                if (msg[i] == '<')
                {
                    int endIndex = msg.IndexOf('>', i);
                    if (endIndex != -1)
                    {
                        string fullTag = msg.Substring(i, endIndex - i + 1);
                        target.text += fullTag;
                        i = endIndex;
                        continue;
                    }
                }

                target.text += msg[i];
                yield return new WaitForSeconds(typeSpeed);
            }

            if (_as) _as.Stop();

            _showedMessage = true;
            _dialogueHint.SetActive(true);

            while (!Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                yield return null; 
            }

            _isWritingDialogue = false;

            _currentMessage = string.Empty;
            _showedMessage = false;

            if (onFinished != null) onFinished.Invoke();
        }

        IEnumerator DialogueWait(UnityAction onFinished = null)
        {

            _timeSinceWriteStart = 0f;

            _showedMessage = true;

            while (!Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                yield return null;
            }

            _isWritingDialogue = false;

            _currentMessage = string.Empty;
            _showedMessage = false;

            if (onFinished != null) onFinished.Invoke();
        }

        public bool IsShowingDialogue()
        {
            return _isWritingDialogue;
        }

        public bool IsDialogueWaiting()
        {
            return _isWritingDialogue && _showedMessage;
        }

        public void ShowDialogue()
        {
            _dialogueHolder.SetActive(true);
            _dialogueHint.SetActive(false);
        }

        public void DisplayDialogue(Dialogue dialogue)
        {
            _dialogueHint.SetActive(false);

            _dialogueAvatarName.text = dialogue.avatarName;
            StartCoroutine(TypeDialogue(
                dialogue.msg[Language.EN],
                ((dialogue.typeSpeedOverride <= 0) ? _typeSpeed : dialogue.typeSpeedOverride),
                _dialogue,
                Next)
            );
        }

        public void HideDialogue()
        {
            _dialogueAvatarName.text = string.Empty;
            _dialogue.text = string.Empty;
            _dialogueHolder.SetActive(false);
            _dialogueHint.SetActive(false);
        }

        public void ForceStopDialogue()
        {
            if (_isWritingDialogue)
            {
                StopAllCoroutines();
                _isWriting = false;
                _showedMessage = false;
                _isWritingDialogue = false;
                _dialogue.text = string.Empty;
                _as?.Stop();
                Next();
            }
        }

        public void SkipDialogue()
        {
            if (!_showedMessage && _isWritingDialogue)
            {
                StopAllCoroutines();
                _dialogue.text = _currentMessage;
                _as?.Stop();
                _showedMessage = true;
                StartCoroutine(DialogueWait(Next));
            }
            else if (_isWritingDialogue)
            {
                StopAllCoroutines();
                _isWriting = false;
                _showedMessage = false;
                _isWritingDialogue = false;
                _dialogue.text = string.Empty;
                _as?.Stop();
                Next();
            }
        }

        private void Next()
        {
            _isWriting = false;
            GameManager.GetMonoSystem<IDialogueMonoSystem>().CloseDialogue();
        }

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

        private void HandleTimeout()
        {
            if (RecursiveGameManager.IsPaused) return;

            if (_isWriting) _timeSinceWriteStart += Time.deltaTime;

            if (_timeSinceWriteStart > _timeout && !_showedMessage || (Keyboard.current.spaceKey.wasPressedThisFrame && !_showedMessage))
            {
                StopAllCoroutines();
                _as?.Stop();
                _showedMessage = true;
                if (_isWritingDialogue) _dialogue.text = _currentMessage;
                _dialogueHint.SetActive(true);
            }
            else if (_showedMessage)
            {
                if (Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    _isWriting = false;

                    if (_isWritingDialogue)
                    {
                        Next();
                    }

                    _showedMessage = false;
                    _isWritingDialogue = false;
                    _as?.Stop();
                }
            }
        }

        public override void Init()
        {
            RecorderOff();
            DisableTimer();
            HideDialogue();
        }

        private void Update()
        {
            HandleTimeout();
        }
    }
}
