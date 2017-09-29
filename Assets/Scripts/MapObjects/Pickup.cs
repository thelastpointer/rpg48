using UnityEngine;
using RPG.Inventory;

namespace RPG
{
    public class Pickup : MonoBehaviour
    {
        public Item Item;

        //public WeaponData Weapon;
        // Armor
        // Mask
        // Spell scroll

        void OnTriggerEnter(Collider other)
        {
            PlayerCharacter player = other.GetComponent<PlayerCharacter>();
            if (player != null)
                player.HUD.DisplayPrompt(this);
        }

        void OnTriggerExit(Collider other)
        {
            PlayerCharacter player = other.GetComponent<PlayerCharacter>();
            if (player != null)
                player.HUD.RemovePrompt(this);
        }
    }
}