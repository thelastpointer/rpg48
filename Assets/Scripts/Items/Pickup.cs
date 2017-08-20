using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class Pickup : MonoBehaviour
    {
        public WeaponData Weapon;
        // Armor
        // Mask
        // Spell scroll

        void OnTriggerEnter(Collider other)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
                player.HUD.DisplayPrompt(this);
        }

        void OnTriggerExit(Collider other)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
                player.HUD.RemovePrompt(this);
        }
    }
}