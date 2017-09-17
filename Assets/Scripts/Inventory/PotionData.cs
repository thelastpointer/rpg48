using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(fileName = "potion.asset", menuName = "RPG/Potion data")]
    public class PotionData : Item
    {
        public float Cooldown = 1;
        public float HealthAdd = 10;
        public float ManaAdd = 0;
        public float HealthPercent = 0;
        public float ManaPercent = 0;
    }
}