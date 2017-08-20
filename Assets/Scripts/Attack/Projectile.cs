using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class Projectile : MonoBehaviour
    {
        Transform tr;
        Entity owner;
        WeaponData data;
        float distanceMoved;
        int passThrough;

        public void Initialize(Vector3 position, Vector3 direction, WeaponData data, Entity attacker)
        {
            tr.position = position;
            tr.rotation = Quaternion.LookRotation(direction);

            this.data = data;
            owner = attacker;
            passThrough = 0;
        }

        void Awake()
        {
            tr = GetComponent<Transform>();
        }

        // TODO: It would be faster if a 'projectile manager' managed all updates. This could get slow
        void Update()
        {
            distanceMoved += data.ProjectileSpeed * Time.deltaTime;
            if (distanceMoved > data.ProjectileRange)
                Kill();
            else
                tr.Translate(Vector3.forward * data.ProjectileSpeed * Time.deltaTime);
        }

        void OnTriggerEnter(Collider other)
        {
            Entity e = other.GetComponent<Entity>();
            if ((e != null) && (owner != e) && owner.Faction.CanAttack(e.Faction))
            {
                // TODO: Impact effects

                e.OnDamage(data.Damage, owner);

                ++passThrough;
                if (passThrough >= data.ProjectilePassThrough)
                    Kill();
            }
            else if ((other.gameObject.layer == LayerMask.NameToLayer("Level")) && !other.CompareTag("ShootThrough"))
                Kill();
        }
        /*
        void OnCollisionEnter(Collision collision)
        {
            Debug.Log("COLLISION");
            if ((collision.gameObject.layer == LayerMask.NameToLayer("Level")) && !collision.gameObject.CompareTag("ShootThrough"))
                Kill();
        }
        */
        void Kill()
        {
            Destroy(gameObject);
        }
    }
}