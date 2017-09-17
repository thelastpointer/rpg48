/*
    Monster.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// can flee when low on HP
// smarter monsters could move back when weapon is on cooldown

namespace RPG
{
    [RequireComponent(typeof(CharacterController))]
    public class Monster : Entity
    {
        public Inventory.WeaponData Weapon;
        public float Speed = 2;
        public Animator Anim;
        public float AttackAnimDelay;
        public float MinAttackRange = 0;
        public float MaxAttackRange = 2;

        Transform tr;
        CharacterController controller;
        Entity target;
        WeaponInstance weapon = new WeaponInstance();
        Vector3 movement;
        bool changedFactions = false;

        void Awake()
        {
            tr = GetComponent<Transform>();
            controller = GetComponent<CharacterController>();
            weapon.Data = Weapon;
        }

        void Update()
        {
            bool moving = false;

            if (target != null)
            {
                // Check distance
                Vector3 diff = target.transform.position - tr.position;
                float dst = diff.magnitude;

                tr.rotation = Quaternion.LookRotation(diff);

                // Move towards target
                if (dst > MaxAttackRange)
                {
                    controller.Move((diff.normalized * Speed) * Time.deltaTime);
                    moving = true;
                }
                // Attack
                else
                {
                    //bool attacking = Weapon.CanFire();

                    if (weapon.CanFire())
                    {
                        if (Anim != null)
                        {
                            Anim.SetTrigger("PlayAttack");
                            StartCoroutine(Delayed(AttackAnimDelay, () => { weapon.Fire(tr.position, diff, this); }));
                        }
                        else
                            weapon.Fire(tr.position, diff, this);
                    }
                }
            }
            else
            {
                // Player will notify monster if in range

                // Wander?
            }

            if (Anim != null)
                Anim.SetBool("IsMoving", moving);
            controller.Move(Physics.gravity);
        }
        /*
        void FixedUpdate()
        {
            controller.Move((movement + Physics.gravity) * Time.deltaTime);
        }
        */
        public override void OnDamage(float damage, Entity attacker)
        {
            base.OnDamage(damage, attacker);

            if (Anim != null)
                Anim.SetTrigger("PlayHit");

            // Reckless monsters attack other monsters too
            if ((Faction == Faction.RecklessMonster) && (attacker != target))
                TrySetTarget(attacker);
        }

        protected override void Die()
        {
            Game.Instance.OnMonsterKilled(this);

            base.Die();
        }

        void TrySetTarget(Entity e)
        {
            if (target == null)
            {
                changedFactions = false;
                target = e;
            }
            else if (e != target)
            {
                if ((e.Faction != target.Faction) && !changedFactions)
                {
                    changedFactions = true;
                    target = e;
                }
                else
                    target = e;
            }
        }

        IEnumerator Delayed(float delay, System.Action action)
        {
            yield return new WaitForSeconds(delay);

            if (Health > 0)
                action.Invoke();
        }

        public void TargetApproach(Entity e)
        {
            TrySetTarget(e);
        }
    }
}