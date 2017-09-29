using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(fileName = "weapon.asset", menuName = "RPG/Weapon data")]
    public class WeaponData : Item
    {
        // TODO: Graphics
        // TODO: Type (one or two-handed)
        // TODO: Hit effects and stuff

        [Header("Shared data")]
        public float Damage;
        public float Cooldown;
        public bool IsSpell = false;

        public WeaponType Type = WeaponType.Melee;
        
        [Header("Melee")]
        public float MeleeRange;
        public float MeleeAngle;

        [Header("Projectile")]
        public float ProjectileSpeed;
        public Projectile Projectile;
        public float ProjectileRange;
        public int ProjectilePassThrough = 1;

        [Header("Raycast")]
        public float RaycastRange;
        public GameObject RaycastEffect;
        public int RaycastPassThrough = 1;
        public bool ContinousRaycast = false;
        public float RaycastDuration;

        public float GetRange()
        {
            if (Type == WeaponType.Melee)
                return MeleeRange;

            return ProjectileRange;
        }

        public float DamagePerSec()
        {
            return Damage * Cooldown;
        }
    }

    public enum WeaponType
    {
        Melee,
        Projectile,
        Raycast
    }
}