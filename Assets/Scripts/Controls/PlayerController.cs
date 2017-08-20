/*
    PlayerController.cs
*/

using System;
using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : Entity
    {
        [Header("Movement properties")]
        public float MovementSpeed = 5f;

        [Header("Controls")]
        public PlayerControls Controls;

        [Header("UI")]
        public PlayerHUD HUD;

        [Header("Inventory")]
        //public ArmorItem Armor;
        //public WeaponItem Weapon;
        //public Item Artifact;

        // Spellbook
        //unlocked spells
        //selected spell

        public WeaponInstance Weapon;
        
        private Transform tr;
        private CharacterController controller;
        private Vector3 move;

        void Awake()
        {
            tr = GetComponent<Transform>();
            controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            Controls.Update(tr.position, MovementSpeed, 0);
            move = Controls.GetMovement().normalized * MovementSpeed;

            if (Controls.Attack1())
                Weapon.Fire(tr.position, Controls.GetDirection(), this);

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