using UnityEngine;

namespace RPG
{
    public class Spell : ScriptableObject
    {
        public bool IsAttack = true;
        public Inventory.WeaponData WeaponData;

        public virtual void Cast() { }
    }
}