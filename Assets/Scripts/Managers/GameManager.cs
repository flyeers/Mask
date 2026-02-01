using System;
using System.Collections;
using System.Security.Cryptography;
using Damage;
using UnityEngine;
using TMPro;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Damageable playerDamageable;

        private Coroutine killCoroutine;
        private float timeLeft;
        public bool IsKillCountdownActive => killCoroutine != null;
        public float TimeLeft => timeLeft;

        [Header("UI")]
        [SerializeField] private TMP_Text deathCountdownText;
        [SerializeField] private string countdownPrefix = "Time to exit: ";

        private void Awake()
        {
            playerDamageable.OnDeath += PlayerDamageableOnDeath;
            SetCountdownUIVisible(false);
        }

        private void PlayerDamageableOnDeath()
        {
            CancelkillCountdown();
            GeneralManager.Instance.SceneController.ReloadCurrentScene();
        }

        public void StartKillCountdown(float seconds)
        {
            if (seconds <= 0f) seconds = 0.01f;

            if (killCoroutine != null)
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
            timeLeft = 0f;
            SetCountdownUIVisible(false);
        }

        private IEnumerator KillCountdownRoutine(float seconds)
        {
            timeLeft = seconds;
            SetCountdownUIVisible(true);

            while (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateCountdownUI(timeLeft);
                yield return null;
            }

            killCoroutine = null;
            SetCountdownUIVisible(false);
            playerDamageable.Die();
        }

        private void UpdateCountdownUI(float t)
        {
            if (deathCountdownText == null) return;

            // Mostrar en segundos enteros (3,2,1...)
            int secondsInt = Mathf.CeilToInt(Mathf.Max(t, 0f));
            deathCountdownText.text = countdownPrefix + secondsInt + "s";
        }

        private void SetCountdownUIVisible(bool visible)
        {
            if (deathCountdownText == null) return;
            deathCountdownText.gameObject.SetActive(visible);
        }

    }
}
