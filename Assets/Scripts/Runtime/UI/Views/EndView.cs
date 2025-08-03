using PlazmaGames.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Recursive
{
    public class EndView : View
    {
        public override void Init()
        {

        }

        public override void Show()
        {
            base.Show();
            RecursiveGameManager.IsPaused = true;
        }

        private void Update()
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                Application.Quit();
            }
        }
    }
}
