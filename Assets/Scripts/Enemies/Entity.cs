using UnityEngine;

namespace RPG
{
    public class Entity : MonoBehaviour
    {
        public int Strength = 10;
        public int Dexterity = 10;
        public int Constitution = 10;
        public int Spellpower = 10;
        public int Intelligence = 10;

        public Faction Faction = Faction.Monster;
        public bool Indestructible = false;
        public float MaxHealth = 1;
        public float Health = 1;
        public float MaxMana = 0;
        public float Mana = 0;
        public GameObject[] DeathPrefabs;

        protected virtual float AdjustIncomingDamage(float baseDamage)
        {
            return baseDamage;
        }

        public virtual void OnDamage(float damage, Entity attacker)
        {
            if (!Indestructible)
            {
                Health -= AdjustIncomingDamage(damage);
                if (Health <= 0)
                    Die();
            }
        }

        protected virtual void Die()
        {
            // Death prefabs
            foreach (GameObject go in DeathPrefabs)
                Instantiate(go, transform.position, transform.rotation, transform.parent);

            // Destroy self
            Destroy(gameObject);
        }

        public virtual void AddHealth(float amount)
        {
            Health = Mathf.Min(Health + amount, MaxHealth);
        }

        public virtual void AddMana(float amount)
        {
            Mana = Mathf.Min(Mana + amount, MaxMana);
        }

        public virtual void AddHealthPercent(float percent)
        {
            Health = Mathf.Min(Health + MaxHealth * percent, MaxHealth);
        }

        public virtual void AddManaPercent(float percent)
        {
            Mana = Mathf.Min(Mana + MaxMana * percent, MaxMana);
        }
    }
}