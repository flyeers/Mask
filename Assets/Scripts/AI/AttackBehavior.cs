using Damage;
using UnityEngine;

public class AttackBehavior : MonoBehaviour
{
    public void AttackTarget(IDamageable target)
    {
        if (target == null) return;
        
        target.Die();
    }
}
