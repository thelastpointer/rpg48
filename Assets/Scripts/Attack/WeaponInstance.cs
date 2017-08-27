using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [System.Serializable]
    public class WeaponInstance : CooldownAbility
    {
        public WeaponData Data;

        private float lastShoot;
        private List<Entity> entities = new List<Entity>();
        private Collider[] hitResults = new Collider[20];

        protected override float Cooldown()
        {
            return Data.Cooldown;
        }

        protected override void FireInternal(Vector3 casterPosition, Vector3 attackDirection, Entity attacker)
        {
            switch (Data.Type)
            {
                case WeaponType.Melee:
                    MeleeAttack(casterPosition, attackDirection, attacker);
                    break;
                case WeaponType.Projectile:
                    ProjectileAttack(casterPosition, attackDirection, attacker);
                    break;
                case WeaponType.Raycast:
                    RaycastAttack(casterPosition, attackDirection, attacker);
                    break;
                default:
                    break;
            }
        }
        
        private void DoDamage(Entity target, Entity attacker)
        {
            // Melee, magic, ranged
            float baseDamage = Data.Damage;

            if (Data.Type == WeaponType.Melee)
                baseDamage *= 1f;
            else if (Data.Type == WeaponType.Projectile)
                baseDamage *= 1f;
            //...
            
            target.OnDamage(Data.Damage, attacker);
        }
        
        private void MeleeAttack(Vector3 position, Vector3 direction, Entity attacker)
        {
            // Resolve cone
            Entity[] targets = ResolveCone(position, direction, attacker.Faction);

            // Damage every entity
            foreach (Entity e in targets)
                DoDamage(e, attacker);
        }

        private void ProjectileAttack(Vector3 position, Vector3 direction, Entity attacker)
        {
            // Create projectile
            Projectile projectile = GameObject.Instantiate(Data.Projectile);
            projectile.Initialize(position, direction, this, attacker, DoDamage);
        }

        private void RaycastAttack(Vector3 position, Vector3 direction, Entity attacker)
        {
            // Resolve raycast
            Entity[] targets = ResolveRaycast(position, direction, attacker.Faction, Data.RaycastPassThrough);

            // Damage every entity
            foreach (Entity e in targets)
                DoDamage(e, attacker);
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