using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class Switch : MonoBehaviour
    {
        public bool PlayerTrigger = true;
        public bool MonsterTrigger = false;
        public bool OneShot = true;

        public GameObject[] TriggeredOjects;

        bool alreadyUsed = false;

        public void OnTriggerEnter(Collider other)
        {
            Entity e = other.GetComponent<Entity>();
            if (e != null)
            {
                if (PlayerTrigger && (e.Faction == Faction.Player))
                {
                    Trigger();
                }

                if (MonsterTrigger && ((e.Faction == Faction.Monster) || (e.Faction == Faction.RecklessMonster)))
                {
                    Trigger();
                }
            }
        }

        void Trigger()
        {
            if (OneShot && alreadyUsed)
                return;

            alreadyUsed = true;

            foreach (GameObject go in TriggeredOjects)
                go.SendMessage("OnTrigger", gameObject);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (GameObject go in TriggeredOjects)
                Gizmos.DrawLine(transform.position, go.transform.position);
        }
    }
}