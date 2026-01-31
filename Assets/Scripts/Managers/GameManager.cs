using System;
using System.Collections;
using System.Security.Cryptography;
using Damage;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Damageable playerDamageable;

        private Coroutine killCoroutine;

        private void Awake()
        {
            playerDamageable.OnDeath += PlayerDamageableOnDeath;
        }

        private void PlayerDamageableOnDeath()
        {
            GeneralManager.Instance.SceneController.ReloadCurrentScene();
        }

        public void StartKillCountdown(float seconds)
        {
            if(killCoroutine  != null) 
                StopCoroutine(killCoroutine);

            killCoroutine = StartCoroutine(KillCountdownRoutine(seconds));
        }

        public void CancelkillCountdown()
        {
            if(killCoroutine != null)
            {
                StopCoroutine(killCoroutine);
                killCoroutine = null;
            }
        }

        private IEnumerator KillCountdownRoutine(float seconds)
        {
            float t = seconds;

            while(t > 0)
            {
                t -= Time.deltaTime;
                yield return null;
            }

            killCoroutine = null;

            playerDamageable.Die();
        }
        
    }
}
