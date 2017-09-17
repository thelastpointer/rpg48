using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RPG.UI
{
	/// <summary>
	/// PauseMenu
	/// </summary>
	public class PauseMenu : MonoBehaviour
	{
        // TODO: Maybe all these should go into Game.cs
		public void PauseGame()
        {
            Time.timeScale = 0f;
            gameObject.SetActive(true);
        }
        public void Unpause()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }

        public void QuitToMenu()
        {
            Game.Instance.QuitToMenu();
        }
        public void QuitGame()
        {
            Game.Instance.QuitGame();
        }
	}
}