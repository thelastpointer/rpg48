using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class EnemyNotifier : MonoBehaviour
    {
        public Entity Owner;

        public void OnTriggerStay(Collider other)
        {
            Monster m = other.GetComponent<Monster>();
            if (m != null)
                m.TargetApproach(Owner);
        }
    }
}