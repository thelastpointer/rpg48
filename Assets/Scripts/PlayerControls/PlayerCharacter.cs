/*
    PlayerController.cs
*/

using RPG.Inventory;
using System;
using System.Collections.Generic;
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

        [Header("Starting inventory")]
        public WeaponData DefaultWeapon;
        public ArmorData DefaultArmor;
        //public Item Artifact;
        
        // Spellbook
        //unlocked spells
        //selected spell
        
        public WeaponUseAbility Weapon { get { return weaponInstance; } }
        public ArmorData Armor { get { return armor; } }
        public PotionUseAbility Potion { get { return potion; } }

        [HideInInspector]
        public int Level = 1;
        [HideInInspector]
        public int XP;

        int abilityPoints;
        
        private Transform tr;
        private CharacterController controller;
        private Vector3 move;
        private WeaponUseAbility weaponInstance = new WeaponUseAbility();
        private WeaponUseAbility selectedSpell = new WeaponUseAbility();
        private PotionUseAbility potion = new PotionUseAbility();
        private ArmorData armor;

        private List<Item> inventory = new List<Item>();

        protected override float AdjustIncomingDamage(float baseDamage)
        {
            float finalDamage = base.AdjustIncomingDamage(baseDamage);

            // Armor
            if (armor != null)
                finalDamage = armor.AdjustDamage(finalDamage);

            return base.AdjustIncomingDamage(baseDamage);
        }

        protected override void Die()
        {
            Game.Instance.OnPlayerKilled(this);

            base.Die();
        }

        void Awake()
        {
            tr = GetComponent<Transform>();
            controller = GetComponent<CharacterController>();

            weaponInstance.Data = DefaultWeapon;
            armor = DefaultArmor;
        }

        void Update()
        {
            if (!HUD.IsGUIOpen())
            {
                // TODO: Can we move when the GUI is open?
                Controls.Update(tr.position, MovementSpeed, 0);
                move = Controls.GetMovement().normalized * MovementSpeed;

                if (weaponInstance != null)
                {
                    if (Controls.Attack1())
                        weaponInstance.Fire(tr.position, Controls.GetDirection(), this);
                }

                // TODO: Move these to PlayerControls
                if (Input.GetKeyDown(KeyCode.F))
                    HUD.OpenItemPrompt();
                
                if (Input.GetKeyDown(KeyCode.Q))
                    Potion.Fire(tr.position, Controls.GetDirection(), this);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // CAST THAT MOTHERFUCKING SPELL
                    //Potion.Fire(tr.position, Controls.GetDirection(), this);
                }
            }

            if (Input.GetKeyDown(KeyCode.C))
                HUD.ToggleCharSheet(this);
        }

        void FixedUpdate()
        {
            tr.rotation = Controls.GetRotation();

            Vector3 fullMove = move + Physics.gravity;
            controller.Move(fullMove * Time.deltaTime);
        }

        public void SetArmor(ArmorData a)
        {
            armor = a;
        }

        public void SetPotion(PotionData p)
        {
            potion.Data = p;
        }

        public void LevelUp()
        {
            ++abilityPoints;
            ++Level;

            MaxHealth = 90 + Level * Constitution;

            Health = MaxHealth;
            Mana = MaxMana;

            HUD.LevelUp();
        }

        public int UseAbilityPoint()
        {
            --abilityPoints;

            if (abilityPoints < 0)
                abilityPoints = 0;

            return abilityPoints;
        }
    }
}