using UnityEngine;

namespace RPG
{
    [System.Serializable]
    public abstract class CooldownAbility : MonoBehaviour
    {
        private float lastShoot;

        protected abstract float Cooldown();
        protected abstract void FireInternal(Vector3 casterPosition, Vector3 attackDirection, Entity attacker);

        public float GetCooldownPercent()
        {
            return Mathf.Clamp01((Time.time - lastShoot) / Cooldown());
        }

        public bool CanFire()
        {
            return ((Time.time - lastShoot) > Cooldown());
        }

        public bool Fire(Vector3 casterPosition, Vector3 attackDirection, Entity caster)
        {
            if ((Time.time - lastShoot) > Cooldown())
            {
                // TODO: Effects and animations and shit

                lastShoot = Time.time;

                FireInternal(casterPosition, attackDirection, caster);
                
                return true;
            }

            return false;
        }
    }
}