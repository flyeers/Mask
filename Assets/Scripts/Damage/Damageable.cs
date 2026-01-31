using System;
using UnityEngine;

namespace Damage
{
    public class Damageable : MonoBehaviour,IDamageable
    {

        public void Die()
        {
            Destroy(gameObject);
            OnDeath?.Invoke();
        }

        public event Action OnDeath;
    }
}
