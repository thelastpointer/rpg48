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

        [Header("Panels")]
        public CharacterSheet CharSheet;

        [Header("Item switching")]
        public GameObject ItemPrompt;
        public ItemComparePanel ItemComparePanel;
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
            {
                WeaponImage.sprite = Player.Weapon.Data.Icon;
                WeaponCooldown.fillAmount = 1f - Player.Weapon.GetCooldownPercent();
            }

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

        public void ToggleCharSheet()
        {
            CharSheet.Toggle();
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
                {
                    ShowWeaponComparison(weapon);
                    return;
                }

                ArmorData armor = pickupPromptDisplayed.Item as ArmorData;
                if (armor != null)
                {
                    ShowArmorComparison(armor);
                    return;
                }
            }
        }

        void HideComparison()
        {
            ItemComparePanel.gameObject.SetActive(false);

            //...
        }

        private void ShowWeaponComparison(WeaponData weapon)
        {
            ItemComparePanel.gameObject.SetActive(true);
            ItemComparePanel.SetItems(weapon, Player.Weapon.Data,
                new ItemComparePanel.StatBlock("DPS", (weapon.Damage / weapon.Cooldown).ToString("0"), (Player.Weapon.Data.Damage / Player.Weapon.Data.Cooldown).ToString("0")),
                new ItemComparePanel.StatBlock("Damage", weapon.Damage, Player.Weapon.Data.Damage),
                new ItemComparePanel.StatBlock("Speed", weapon.Cooldown * 100, Player.Weapon.Data.Cooldown * 100),
                new ItemComparePanel.StatBlock("Range", weapon.GetRange(), Player.Weapon.Data.GetRange())
            );
        }

        private void ShowArmorComparison(ArmorData armor)
        {
            // TODO: This will throw a big fat NullRef because player won't have armor at the beginning
            ItemComparePanel.gameObject.SetActive(true);
            ItemComparePanel.SetItems(armor, Player.Armor,
                new ItemComparePanel.StatBlock("Defense", armor.NegativeDamagePercentile.ToString() + "%", Player.Armor.NegativeDamagePercentile.ToString() + "%"),
                new ItemComparePanel.StatBlock("DR", armor.DR, Player.Armor.DR)
            );
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

                // If player's default weapon, destroy the pickup
                if (tmp == Player.DefaultWeapon)
                {
                    Destroy(pickupPromptDisplayed.gameObject);
                }
                // Change contents otherwise
                else
                {
                    // TODO: Also change mesh!
                    pickupPromptDisplayed.Item = tmp;
                }

                return;
            }

            ArmorData armor = pickupPromptDisplayed.Item as ArmorData;
            if (armor != null)
            {
                ArmorData tmp = Player.Armor;

                Player.SetArmor(armor);

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