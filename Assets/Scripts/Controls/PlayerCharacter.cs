/*
    PlayerController.cs
*/

using RPG.Inventory;
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

        [Header("Starting inventory")]
        public WeaponData DefaultWeapon;
        public ArmorData DefaultArmor;
        //public Item Artifact;
        
        // Spellbook
        //unlocked spells
        //selected spell
        
        public WeaponInstance Weapon { get { return weaponInstance; } }
        public ArmorData Armor { get { return armor; } }
        public PotionData Potion { get { return potion.Data; } }

        [HideInInspector]
        public int Level = 1;
        [HideInInspector]
        public int XP;
        
        private Transform tr;
        private CharacterController controller;
        private Vector3 move;
        private WeaponInstance weaponInstance = new WeaponInstance();
        private WeaponInstance selectedSpell = new WeaponInstance();
        private PotionAbility potion = new PotionAbility();
        private ArmorData armor;

        protected override float AdjustIncomingDamage(float baseDamage)
        {
            float finalDamage = base.AdjustIncomingDamage(baseDamage);

            // Armor
            if (armor != null)
                finalDamage = armor.AdjustDamage(finalDamage);

            return base.AdjustIncomingDamage(baseDamage);
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

            if (Input.GetKeyDown(KeyCode.C))
                HUD.ToggleCharSheet();
        }

        public void SetArmor(ArmorData a)
        {
            armor = a;
        }

        void FixedUpdate()
        {
            tr.rotation = Controls.GetRotation();

            Vector3 fullMove = move + Physics.gravity;
            controller.Move(fullMove * Time.deltaTime);
        }
        
        #region XP table

        static int[] XPLevels = new int[] { 1000, 3000, 5000, 10000, 15000 };

        // Note: level is indexed from 1
        public static int GetMinXPForLevel(int level)
        {
            if (level <= 1)
                return 0;

            return XPLevels[level - 2];
        }
        // Note: level is indexed from 1
        public static int GetMaxXPForLevel(int level)
        {
            int idx = level - 1;

            if (XPLevels.Length >= idx)
                return XPLevels[XPLevels.Length - 1] * 2;

            return XPLevels[level - 1];
        }

        #endregion
    }
}