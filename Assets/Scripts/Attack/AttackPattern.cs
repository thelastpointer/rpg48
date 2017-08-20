using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [Serializable]
    public class AttackPattern
    {
        public Type Pattern = Type.Cone;
        public float Range;
        public float Angle;

        const int MAX_HITS = 20;

        private Collider[] results = new Collider[MAX_HITS];
        private List<Entity> entities = new List<Entity>();

        public Entity[] Resolve(Vector3 position, Vector3 direction)
        {
            entities.Clear();
            for (int i = 0; i < MAX_HITS; ++i)
                results[i] = null;

            switch (Pattern)
            {
                case Type.Circle:
                    return ResolveCircle(position, direction);
                case Type.Cone:
                    return ResolveCone(position, direction);
                case Type.Raycast:
                    return ResolveRaycast(position, direction);
                default:
                    break;
            }

            return entities.ToArray();
        }

        public void DrawGizmos(Vector3 position, Vector3 direction)
        {
            Gizmos.color = Color.yellow;

            if (Pattern == Type.Cone)
            {
                Vector3 p1 = position + Quaternion.Euler(0, Angle / 2f, 0) * direction * Range;
                Vector3 p2 = position + Quaternion.Euler(0, -Angle / 2f, 0) * direction * Range;
                Vector3 p3 = position + direction * Range;

                Gizmos.DrawLine(position, p1);
                Gizmos.DrawLine(position, p2);
                Gizmos.DrawLine(p1, p3);
                Gizmos.DrawLine(p2, p3);
            }
            else if (Pattern == Type.Circle)
            {

            }
            else if (Pattern == Type.Raycast)
            {

            }
        }

        private Entity[] ResolveCircle(Vector3 position, Vector3 direction)
        {
            int count = Physics.OverlapSphereNonAlloc(position, Range, results);

            //results = Physics.OverlapSphere(position, Range);
            //int count = results.Length;

            if (count > 0)
            {
                foreach (Collider c in results)
                {
                    if (c != null)
                    {
                        // Filter for entities
                        Entity e = c.GetComponent<Entity>();
                        if (e != null)
                            entities.Add(e);
                    }
                }
            }

            return entities.ToArray();
        }

        private Entity[] ResolveCone(Vector3 position, Vector3 direction)
        {
            int count = Physics.OverlapSphereNonAlloc(position, Range, results);

            //results = Physics.OverlapSphere(position, Range);
            //int count = results.Length;

            if (count > 0)
            {
                // Filter cone
                foreach (Collider c in results)
                {
                    if (c != null)
                    {
                        // Filter for entities
                        Entity e = c.GetComponent<Entity>();
                        if (e != null)
                        {
                            // Filter cone
                            Vector3 diff = e.transform.position - position;
                            diff = new Vector3(diff.x, 0, diff.z);
                            if (Vector3.Angle(diff, direction) < (Angle / 2f))
                                entities.Add(e);
                        }
                    }
                }
            }

            return entities.ToArray();
        }

        private Entity[] ResolveRaycast(Vector3 position, Vector3 direction)
        {
            throw new NotImplementedException();
        }

        public enum Type
        {
            Circle,
            Cone,
            Raycast
        }
    }
}