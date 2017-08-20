using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [System.Serializable]
    public class WeaponInstance
    {
        public WeaponData Data;

        private float lastShoot;
        private List<Entity> entities = new List<Entity>();
        private Collider[] hitResults = new Collider[20];

        public float GetCooldownPercent()
        {
            return Mathf.Clamp01((Time.time - lastShoot) / Data.Cooldown);
        }

        public bool CanFire()
        {
            return ((Time.time - lastShoot) > Data.Cooldown);
        }

        public bool Fire(Vector3 position, Vector3 direction, Entity attacker)
        {
            UnityEngine.Assertions.Assert.IsNotNull(Data, "Weapon data is null");

            if ((Time.time - lastShoot) > Data.Cooldown)
            {
                // TODO: Effects and anim and shit
                lastShoot = Time.time;

                switch (Data.Type)
                {
                    case WeaponType.Melee:
                        MeleeAttack(position, direction, attacker);
                        break;
                    case WeaponType.Projectile:
                        ProjectileAttack(position, direction, attacker);
                        break;
                    case WeaponType.Raycast:
                        RaycastAttack(position, direction, attacker);
                        break;
                    default:
                        break;
                }

                return true;
            }

            return false;
        }

        private void MeleeAttack(Vector3 position, Vector3 direction, Entity attacker)
        {
            // Resolve cone
            Entity[] targets = ResolveCone(position, direction, attacker.Faction);

            // Damage every entity
            foreach (Entity e in targets)
                e.OnDamage(Data.Damage, attacker);
        }

        private void ProjectileAttack(Vector3 position, Vector3 direction, Entity attacker)
        {
            // Create projectile
            Projectile projectile = GameObject.Instantiate(Data.Projectile);
            projectile.Initialize(position, direction, Data, attacker);
        }

        private void RaycastAttack(Vector3 position, Vector3 direction, Entity attacker)
        {
            // Resolve raycast
            Entity[] targets = ResolveRaycast(position, direction, attacker.Faction, Data.RaycastPassThrough);

            // Damage every entity
            foreach (Entity e in targets)
                e.OnDamage(Data.Damage, attacker);
        }

        private Entity[] ResolveCone(Vector3 position, Vector3 direction, Faction attacker)
        {
            entities.Clear();
            for (int i = 0; i < hitResults.Length; ++i)
                hitResults[i] = null;

            int count = Physics.OverlapSphereNonAlloc(position, Data.MeleeRange, hitResults);

            //results = Physics.OverlapSphere(position, Range);
            //int count = results.Length;
            
            if (count > 0)
            {
                // Filter cone
                foreach (Collider c in hitResults)
                {
                    if (c != null)
                    {
                        // Filter for entities
                        Entity e = c.GetComponent<Entity>();
                        if ((e != null) && attacker.CanAttack(e.Faction))
                        {
                            // Filter cone
                            Vector3 diff = e.transform.position - position;
                            diff = new Vector3(diff.x, 0, diff.z);
                            if (Vector3.Angle(diff, direction) < (Data.MeleeAngle / 2f))
                                entities.Add(e);
                        }
                    }
                }
            }

            return entities.ToArray();
        }

        private Entity[] ResolveRaycast(Vector3 position, Vector3 direction, Faction attacker, int resultCount)
        {
            entities.Clear();

            // We need position to be higher than floor level here
            Vector3 midPos = new Vector3(position.x, 1, position.z);
            RaycastHit[] hits = Physics.SphereCastAll(midPos, 0.1f, direction, Data.RaycastRange);
            
            foreach (RaycastHit hit in hits)
            {
                // Filter for entities
                Entity e = hit.collider.GetComponent<Entity>();
                if ((e != null) && attacker.CanAttack(e.Faction))
                    entities.Add(e);
            }

            // Sort by distance; we might only need the first few of them
            entities.Sort((e1, e2) =>
            {
                float d1 = (e1.transform.position - position).sqrMagnitude;
                float d2 = (e2.transform.position - position).sqrMagnitude;

                return d1.CompareTo(d2);
            });

            if ((resultCount > 0) && (entities.Count > resultCount))
            {
                Debug.LogFormat("{0} entities; {1}... {2}", entities.Count, resultCount, entities.Count - resultCount);
                entities.RemoveRange(resultCount, entities.Count - resultCount);
            }
            
            return entities.ToArray();
        }
    }
}