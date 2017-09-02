/*
    PlayerController.cs
*/

using System;
using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerCharacter : Entity
    {
        [Header("Movement properties")]
        public float MovementSpeed = 5f;

        [Header("Controls")]
        public PlayerControls Controls;

        [Header("UI")]
        public PlayerHUD HUD;

        [Header("Inventory")]
        //public ArmorItem Armor;
        public Inventory.WeaponData DefaultWeapon;
        //public Item Artifact;

        // Spellbook
        //unlocked spells
        //selected spell

        public WeaponInstance Weapon { get { return weaponInstance; } }

        private Transform tr;
        private CharacterController controller;
        private Vector3 move;
        private WeaponInstance weaponInstance;
        private WeaponInstance selectedSpell;

        protected override float AdjustIncomingDamage(float baseDamage)
        {
            // DR
            // Percentile

            return base.AdjustIncomingDamage(baseDamage);
        }

        void Awake()
        {
            tr = GetComponent<Transform>();
            controller = GetComponent<CharacterController>();

            weaponInstance = GetComponentInChildren<WeaponInstance>();
            weaponInstance.Data = DefaultWeapon;
        }

        void Update()
        {
            Controls.Update(tr.position, MovementSpeed, 0);
            move = Controls.GetMovement().normalized * MovementSpeed;

            if (weaponInstance != null)
            {
                if (Controls.Attack1())
                    weaponInstance.Fire(tr.position, Controls.GetDirection(), this);
            }

            if (Input.GetKeyDown(KeyCode.F))
                HUD.OpenItemPrompt();
        }

        void FixedUpdate()
        {
            tr.rotation = Controls.GetRotation();

            Vector3 fullMove = move + Physics.gravity;
            controller.Move(fullMove * Time.deltaTime);
        }
    }
}