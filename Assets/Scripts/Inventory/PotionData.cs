using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    public class PotionData : Item
    {
        public float Cooldown = 1;
        public float HealthAdd = 10;
        public float ManaAdd = 0;
    }
}