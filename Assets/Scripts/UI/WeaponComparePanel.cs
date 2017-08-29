using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using RPG.Inventory;

namespace RPG.UI
{
    public class WeaponComparePanel : MonoBehaviour
    {
        public Text TitleLabel;
        public Text DamageLabel1, DamageLabel2;
        public Text SpeedLabel1, SpeedLabel2;
        public Text RangeLabel1, RangeLabel2;
        public Text DPSLabel1, DPSLabel2;

        public void SetItems(WeaponData newWeapon, WeaponData oldWeapon)
        {
            TitleLabel.text = newWeapon.Name;

            // Damage
            DamageLabel1.text = newWeapon.Damage.ToString();
            DamageLabel2.text = oldWeapon.Damage.ToString();

            // Speed (cooldown)
            SpeedLabel1.text = (newWeapon.Cooldown * 100).ToString();
            SpeedLabel2.text = (oldWeapon.Cooldown * 100).ToString();

            // Range
            RangeLabel1.text = newWeapon.GetRange().ToString();
            RangeLabel2.text = oldWeapon.GetRange().ToString();

            // DPS
            DPSLabel1.text = (newWeapon.Damage / newWeapon.Cooldown).ToString();
            DPSLabel2.text = (oldWeapon.Damage / oldWeapon.Cooldown).ToString();
        }
    }
}