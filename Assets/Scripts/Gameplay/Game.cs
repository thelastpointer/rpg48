using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RPG
{
	/// <summary>
	/// Main game controller.
	/// </summary>
	public class Game : MonoBehaviour
	{
        static Game instance;
        public static Game Instance { get { return instance; } }

		void Awake()
		{
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
		}

        public void OnPlayerKilled(PlayerCharacter pc)
        {
            // Game over screen
            pc.HUD.GameOver.gameObject.SetActive(true);
        }

        public void OnMonsterKilled(Monster monster)
        {
            // Calculate XP
            float xp = monster.MaxHealth;
            if (monster.Weapon != null)
                xp *= monster.Weapon.DamagePerSec();

            Debug.Log("Got " + xp + " xp");

            PlayerCharacter pc = FindObjectOfType<PlayerCharacter>();
            pc.XP += Mathf.RoundToInt(xp);
            
            // Level up
            if (pc.XP >= XPTable.GetMaxXPForLevel(pc.Level))
            {
                ++pc.Level;

                pc.Health = pc.MaxHealth;
                pc.Mana = pc.MaxMana;

                pc.HUD.LevelUp();
            }
        }

        public void QuitToMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
	}
}