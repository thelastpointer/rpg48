using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.UI;
using RPG.Inventory;

namespace RPG
{
    public class PlayerHUD : MonoBehaviour
    {
        public PlayerCharacter Player;

        [Header("Health")]
        public Slider Healthbar;
        public Text HealthText;

        [Header("Weapon")]
        public Image WeaponImage;
        public Image WeaponCooldown;

        [Header("Item switching")]
        public GameObject ItemPrompt;
        public WeaponComparePanel SwitchWeaponDialog;

        List<Pickup> pickupPrompts = new List<Pickup>();
        Pickup pickupPromptDisplayed = null;

        void Start()
        {
            Player.HUD = this;
        }

        // Update is called once per frame
        void Update()
        {
            // Health
            Healthbar.maxValue = Player.MaxHealth;
            Healthbar.value = Player.Health;
            HealthText.text = string.Format("{0}/{1}", Player.Health.ToString("0"), Player.MaxHealth.ToString("0"));

            // Cooldowns
            if (Player.Weapon != null)
                WeaponCooldown.fillAmount = 1f - Player.Weapon.GetCooldownPercent();

            // Displaying prompt
            if ((pickupPrompts.Count > 0) && (pickupPromptDisplayed == null))
                ItemPrompt.SetActive(true);
            else
                ItemPrompt.SetActive(false);

            // Displaying switch panel
            if (pickupPromptDisplayed != null)
            {
                ItemPrompt.SetActive(false);
            }
        }

        // Sent by an Item
        public void DisplayPrompt(Pickup i)
        {
            if (!pickupPrompts.Contains(i))
                pickupPrompts.Add(i);
        }
        // Sent by an Item
        public void RemovePrompt(Pickup i)
        {
            if (pickupPrompts.Contains(i))
                pickupPrompts.Remove(i);
            
            if (pickupPromptDisplayed == i)
            {
                HideComparison();

                pickupPromptDisplayed = null;
            }
        }

        // Sent by PlayerController after user action
        public void OpenItemPrompt()
        {
            if (pickupPrompts.Count > 0)
            {
                pickupPromptDisplayed = pickupPrompts[0];
                pickupPrompts.RemoveAt(0);

                WeaponData weapon = pickupPromptDisplayed.Item as WeaponData;
                if (weapon != null)
                    ShowWeaponComparison(weapon);
            }
        }

        void HideComparison()
        {
            SwitchWeaponDialog.gameObject.SetActive(false);

            //...
        }

        private void ShowWeaponComparison(WeaponData weapon)
        {
            SwitchWeaponDialog.gameObject.SetActive(true);
            SwitchWeaponDialog.SetItems(weapon, Player.Weapon.Data);
        }

        public void AcceptPickup()
        {
            HideComparison();

            WeaponData weapon = pickupPromptDisplayed.Item as WeaponData;
            if (weapon != null)
            {
                WeaponData tmp = Player.Weapon.Data;

                // TODO: Also change mesh!
                Player.Weapon.Data = weapon;

                // TODO: Also change mesh!
                pickupPromptDisplayed.Item = tmp;
            }
        }
        public void CancelPickup()
        {
            HideComparison();
        }
    }
}