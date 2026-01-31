using System;

namespace Damage
{
    public interface IDamageable
    {
        public void Die();
        public event Action OnDeath;
    }
}
