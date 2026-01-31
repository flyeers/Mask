using System;
using UnityEngine;

namespace AI.Perception
{
    public class Perceivable : MonoBehaviour
    {
        [Flags]
        public enum EEntityType: int
        {
            None = 0,
            Player = 1 << 0,
            Human = 1 << 1,
            Cat = 1 << 2
        }
        [SerializeField] 
        private EEntityType finalTypeMask;
        public EEntityType TypeMask => finalTypeMask;
    }
}
