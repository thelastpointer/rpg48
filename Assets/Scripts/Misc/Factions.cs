namespace RPG
{
    public enum Faction
    {
        Player,
        Monster,
        RecklessMonster,
        Level
    }

    public static class FactionExtensions
    {
        public static bool CanAttack(this Faction faction, Faction other)
        {
            // Level can attack anyone
            if (faction == Faction.Level)
                return true;

            // Level can only be "attacked" by players
            if (other == Faction.Level)
                return (faction == Faction.Player);

            // Reckless monsters can attack anyone
            if (faction == Faction.RecklessMonster)
                return true;

            // Monsters only attack players
            if (faction == Faction.Monster)
                return (other == Faction.Player);

            // Players can attack any monster
            if (faction == Faction.Player)
                return ((other == Faction.Monster) || (other == Faction.RecklessMonster));

            return false;
        }
    }
}