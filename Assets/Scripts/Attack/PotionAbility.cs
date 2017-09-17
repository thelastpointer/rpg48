using UnityEngine;

namespace RPG
{
    public class PotionAbility : CooldownAbility
    {
        public Inventory.PotionData Data;

        protected override float Cooldown()
        {
            if (Data == null)
                return 0;

            return Data.Cooldown;
        }

        protected override void FireInternal(Vector3 casterPosition, Vector3 attackDirection, Entity attacker)
        {
            if (Data != null)
            {
                attacker.AddHealth(Data.HealthAdd);
                attacker.AddHealthPercent(Data.HealthPercent);
                attacker.AddMana(Data.ManaAdd);
                attacker.AddManaPercent(Data.ManaPercent);
            }
        }
    }
}