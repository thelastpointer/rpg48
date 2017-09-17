using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RPG.UI
{
	/// <summary>
	/// MainMenu
	/// </summary>
	public class MainMenu : MonoBehaviour
	{
        public GameObject MainMenuPanel;
        //...
        public GameObject OptionsPanel;
        public GameObject CreditsPanel;

        void Start()
        {
            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            MainMenuPanel.SetActive(true);
            OptionsPanel.SetActive(false);
            CreditsPanel.SetActive(false);
        }

		public void NewGame()
        {
            // TODO: Launch character generator instead of this
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            /*
            MainMenuPanel.SetActive(false);
            OptionsPanel.SetActive(false);
            CreditsPanel.SetActive(false);
            */
        }
        public void Options()
        {
            MainMenuPanel.SetActive(false);
            OptionsPanel.SetActive(true);
            CreditsPanel.SetActive(false);
        }
        public void Credits()
        {
            MainMenuPanel.SetActive(false);
            OptionsPanel.SetActive(false);
            CreditsPanel.SetActive(true);
        }
        public void Exit()
        {
            Application.Quit();
        }

        public void OpenBlog()
        {
            Application.OpenURL("https://thelastpointer.wordpress.com/48-hour-rpg/");
        }
        public void OpenTwitter()
        {
            // I wonder if this UTM stuff works...
            Application.OpenURL("https://twitter.com/thelastpointer?utm_source=rpg48");
        }
    }
}