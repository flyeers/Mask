using System;
using UnityEngine;

namespace AI.Perception
{
    public interface ISensor
    {
        public int GetPriority();
        public GameObject GetCurrentTarget();
        public event Action<ISensor> Sensed;
        public event Action<ISensor> Forgotten;
    }
}
