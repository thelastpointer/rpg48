using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class TriggerAnimation : MonoBehaviour
    {
        public string ParameterName;
        public ParameterType Type = ParameterType.Trigger;
        public Animator Target;

        public float FloatValue;
        public int IntValue;
        public bool BoolValue;
        public bool IsTrigger;

        public enum ParameterType
        {
            Float,
            Int,
            Bool,
            Trigger
        }

        public void OnTrigger(GameObject sender)
        {
            UnityEngine.Assertions.Assert.IsNotNull(Target, "TriggerAnimation has no target!");

            switch (Type)
            {
                case ParameterType.Float:
                    Target.SetFloat(ParameterName, FloatValue);
                    break;
                case ParameterType.Int:
                    Target.SetInteger(ParameterName, IntValue);
                    break;
                case ParameterType.Bool:
                    Target.SetBool(ParameterName, BoolValue);
                    break;
                case ParameterType.Trigger:
                    Target.SetTrigger(ParameterName);
                    break;
                default:
                    break;
            }
        }
    }
}