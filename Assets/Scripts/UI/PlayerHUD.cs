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

        [Header("Ability bar")]
        public Image WeaponImage;
        public Image WeaponCooldown;
        public Image PotionImage;
        public Image PotionCooldown;

        [Header("Panels")]
        public CharacterSheet CharSheet;
        public GameOverPanel GameOver;

        [Header("Item switching")]
        public GameObject ItemPrompt;
        public ItemComparePanel ItemComparePanel;
        public WeaponComparePanel SwitchWeaponDialog;

        [Header("Messages")]
        public GameObject LevelUpButton;

        List<Pickup> pickupPrompts = new List<Pickup>();
        Pickup pickupPromptDisplayed = null;

        public bool IsGUIOpen()
        {
            return (CharSheet.gameObject.activeSelf || ItemComparePanel.gameObject.activeSelf);
        }

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
            if (Player.Potion.Data != null)
            {
                PotionImage.sprite = Player.Potion.Data.Icon;
                PotionCooldown.fillAmount = 1f - Player.Potion.GetCooldownPercent();
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

        public void LevelUp()
        {
            LevelUpButton.SetActive(true);
        }

        public void ToggleCharSheet()
        {
            CharSheet.Toggle();
            if (CharSheet.gameObject.activeSelf)
                LevelUpButton.SetActive(false);
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

                PotionData potion = pickupPromptDisplayed.Item as PotionData;
                if (potion != null)
                {
                    ShowPotionComparison(potion);
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
        private void ShowPotionComparison(PotionData potion)
        {
            ItemComparePanel.gameObject.SetActive(true);

            if (Player.Potion.Data == null)
            {
                ItemComparePanel.SetItems(potion, Player.Potion.Data,
                    new ItemComparePanel.StatBlock("HP", potion.HealthAdd, ""),
                    new ItemComparePanel.StatBlock("HP Percent", Mathf.RoundToInt(potion.HealthPercent * 100) + "%", ""),
                    new ItemComparePanel.StatBlock("Mana", potion.ManaAdd, ""),
                    new ItemComparePanel.StatBlock("Mana Percent", Mathf.RoundToInt(potion.ManaPercent * 100) + "%", ""),
                    new ItemComparePanel.StatBlock("Cooldown", potion.Cooldown, "")
                );
            }
            else
            {
                ItemComparePanel.SetItems(potion, Player.Potion.Data,
                    new ItemComparePanel.StatBlock("HP", potion.HealthAdd, Player.Potion.Data.HealthAdd),
                    new ItemComparePanel.StatBlock("HP Percent", Mathf.RoundToInt(potion.HealthPercent * 100) + "%", Mathf.RoundToInt(Player.Potion.Data.HealthPercent * 100) + "%"),
                    new ItemComparePanel.StatBlock("Mana", potion.ManaAdd, Player.Potion.Data.ManaAdd),
                    new ItemComparePanel.StatBlock("Mana Percent", Mathf.RoundToInt(potion.ManaPercent * 100) + "%", Mathf.RoundToInt(Player.Potion.Data.ManaPercent * 100) + "%"),
                    new ItemComparePanel.StatBlock("Cooldown", potion.Cooldown, Player.Potion.Data.Cooldown)
                );
            }
        }

        public void AcceptPickup()
        {
            HideComparison();

            WeaponData weapon = pickupPromptDisplayed.Item as WeaponData;
            if (weapon != null)
            {
                Player.Weapon.Data = (WeaponData)ExchangePickupItem(Player.Weapon.Data);
                
                return;
            }

            ArmorData armor = pickupPromptDisplayed.Item as ArmorData;
            if (armor != null)
            {
                Player.SetArmor((ArmorData)ExchangePickupItem(Player.Armor));
            }

            PotionData potion = pickupPromptDisplayed.Item as PotionData;
            if (potion != null)
            {
                Player.SetPotion((PotionData)ExchangePickupItem(Player.Potion.Data));
            }
        }
        public void CancelPickup()
        {
            HideComparison();
        }

        private Item ExchangePickupItem(Item oldItem)
        {
            Item result = pickupPromptDisplayed.Item;

            if (oldItem != null)
            {
                // TODO: Also change mesh!
                pickupPromptDisplayed.Item = oldItem;
            }
            else
            {
                Destroy(pickupPromptDisplayed.gameObject);
            }

            return result;
        }
    }
}