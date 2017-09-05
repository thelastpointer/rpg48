using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class CharacterSheet : MonoBehaviour
    {
        public PlayerCharacter Target;

        public Text LevelLabel;
        public Slider XPSlider;
        public Text StrLabel;
        public Text DexLabel;
        public Text ConLabel;
        public Text PowLabel;
        public Text IntLabel;

        public Image MaskIcon;
        public Image ArmorIcon;
        public Image WeaponIcon;
        public Image RingIcon;
        public Image PotionIcon;

        public void Show()
        {
            // Level & XP
            LevelLabel.text = "Level " + Target.Level;
            XPSlider.minValue = PlayerCharacter.GetMinXPForLevel(Target.Level);
            XPSlider.maxValue = PlayerCharacter.GetMaxXPForLevel(Target.Level);
            XPSlider.value = Target.XP;

            // Attributes
            StrLabel.text = Target.Strength.ToString();
            DexLabel.text = Target.Dexterity.ToString();
            ConLabel.text = Target.Constitution.ToString();
            PowLabel.text = Target.Spellpower.ToString();
            IntLabel.text = Target.Intelligence.ToString();

            // Icons
            WeaponIcon.sprite = Target.Weapon.Data ? Target.Weapon.Data.Icon : null;
            ArmorIcon.sprite = Target.Armor ? Target.Armor.Icon : null;
            //MaskIcon.sprite = 
            //RingIcon.sprite = 
            PotionIcon.sprite = Target.Potion ? Target.Potion.Icon : null;

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Toggle()
        {
            if (!gameObject.activeSelf)
                Show();
            else
                Hide();
        }
    }
}