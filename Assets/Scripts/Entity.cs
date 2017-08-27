using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class Entity : MonoBehaviour
    {
        public int Strength = 10;
        public int Dexterity = 10;
        public int Willpower = 10;

        public Faction Faction = Faction.Monster;
        public bool Indestructible = false;
        public float MaxHealth = 1;
        public float Health = 1;
        public GameObject[] DeathPrefabs;

        public virtual void OnDamage(float damage, Entity attacker)
        {
            if (!Indestructible)
            {
                Health -= damage;
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
    }
}