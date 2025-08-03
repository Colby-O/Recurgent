using PlazmaGames.Attribute;
using UnityEngine;
using UnityEngine.UI;

namespace Recursive
{
    public class RecorderUISlot : MonoBehaviour
    {
        [SerializeField] private Image _border;
        [SerializeField] private Image _icon;

        [SerializeField, ReadOnly] private bool _isSelected;
        [SerializeField, ReadOnly] private bool _hasSave;

        public bool IsSelected() => _isSelected;
        public void Select()
        {
            _isSelected = true;
            _border.color = Color.white;
        }

        public void Deselect()
        {
            _isSelected = false;
            _border.color = Color.black;
        }

        public void SetSaveState(bool hasSave)
        {
            _hasSave = hasSave;

            _icon.gameObject.SetActive(hasSave);
        }

        private void Awake()
        {
            Deselect();
            SetSaveState(false);
        }
    }
}
