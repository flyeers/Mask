using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Perception
{
    public class PerceptionBrain : MonoBehaviour
    {
        [SerializeField] private BaseSensor[] sensors;

        private ISensor currentSensor = null;
        public event Action PerceptionChanged;
        private void Awake()
        {
            Array.Sort(sensors,(a, b) => a.GetPriority().CompareTo(b.GetPriority()));
            foreach (var sensor in sensors)
            {
                sensor.Sensed += SensorOnSensed;
            }
        }

        private void SensorOnSensed(ISensor sensor)
        {
            if (currentSensor == null || sensor.GetPriority() > currentSensor.GetPriority())
            {
                ChangeCurrentSensor(sensor);
            }
        }

        private void ChangeCurrentSensor(ISensor sensor)
        {
            if (currentSensor != null)
            {
                currentSensor.Forgotten -= SensorOnForgotten;
            }
            currentSensor = sensor;
            if (currentSensor != null)
            {
                currentSensor.Forgotten += SensorOnForgotten;
            }
            PerceptionChanged?.Invoke();
        }

        private void SensorOnForgotten(ISensor sensor)
        {
            if (sensor == currentSensor)
            {
                SearchNextTarget();
            }
        }

        private void SearchNextTarget()
        {
            foreach (var sensor in sensors)
            {
                if (sensor.GetCurrentTarget() != null)
                {
                    ChangeCurrentSensor(sensor);
                    return;
                }
            }
            ChangeCurrentSensor(null);
        }

        public GameObject GetCurrentTarget()
        {
            if (currentSensor == null) return null;
            return currentSensor.GetCurrentTarget();
        }
    }
}
