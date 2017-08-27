using UnityEngine;

namespace RPG
{
    public class Spell : ScriptableObject
    {
        public bool IsAttack = true;
        public WeaponData WeaponData;

        public virtual void Cast() { }
    }
}