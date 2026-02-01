using System;
using UnityEngine;

namespace Damage
{
    public class Damageable : MonoBehaviour,IDamageable
    {

        public void Die()
        {
            OnDeath?.Invoke();
            OnDeath = null;
        }

        public event Action OnDeath;
    }
}
