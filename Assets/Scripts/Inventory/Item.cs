using UnityEngine;

namespace RPG.Inventory
{
    public abstract class Item : ScriptableObject
    {
        [Header("Description")]
        public string Name;
        [Multiline]
        public string Description;
        public Sprite Icon;
    }
}