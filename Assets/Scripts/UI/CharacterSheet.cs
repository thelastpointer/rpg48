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
        public Text XPLabel;
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
            XPSlider.minValue = XPTable.GetMinXPForLevel(Target.Level);
            XPSlider.maxValue = XPTable.GetMaxXPForLevel(Target.Level);
            XPSlider.value = Target.XP;
            XPLabel.text = string.Format("{0}/{1}", Target.XP, XPTable.GetMaxXPForLevel(Target.Level));

            // Attributes
            StrLabel.text = Target.Strength.ToString();
            DexLabel.text = Target.Dexterity.ToString();
            ConLabel.text = Target.Constitution.ToString();
            PowLabel.text = Target.Spellpower.ToString();
            IntLabel.text = Target.Intelligence.ToString();

            // Icons
            WeaponIcon.sprite = Target.Weapon.Data?.Icon;
            ArmorIcon.sprite = Target.Armor?.Icon;

            WeaponIcon.sprite = Target.Weapon.Data ? Target.Weapon.Data.Icon : null;
            ArmorIcon.sprite = Target.Armor ? Target.Armor.Icon : null;
            //MaskIcon.sprite = 
            //RingIcon.sprite = 
            PotionIcon.sprite = Target.Potion.Data ? Target.Potion.Data.Icon : null;

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