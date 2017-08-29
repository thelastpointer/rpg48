using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(fileName = "armor.asset", menuName = "RPG/Armor data")]
    public class ArmorData : Item
    {
        // TODO: Graphics

        // DR is subtracted from all incoming damage
        public float DR;

        // Incoming damage is reduced by this percent
        public float NegativeDamagePercentile;

        public float AdjustDamage(float baseDamage)
        {
            baseDamage -= DR;
            baseDamage *= ((100f - NegativeDamagePercentile) / 100f);

            return baseDamage;
        }
    }
}