using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RPG.UI
{
	/// <summary>
	/// GameOverPanel
	/// </summary>
	public class GameOverPanel : MonoBehaviour
	{
		public void QuitToMenu()
        {
            Game.Instance.QuitToMenu();
        }
        public void Quit()
        {
            Game.Instance.QuitGame();
        }
	}
}