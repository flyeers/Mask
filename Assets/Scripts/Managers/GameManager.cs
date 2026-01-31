using System;
using Damage;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Damageable playerDamageable;

        private void Awake()
        {
            playerDamageable.OnDeath += PlayerDamageableOnDeath;
        }

        private void PlayerDamageableOnDeath()
        {
            GeneralManager.Instance.SceneController.ReloadCurrentScene();
        }
    }
}
