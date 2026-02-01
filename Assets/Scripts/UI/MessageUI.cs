using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MessageUI : MonoBehaviour
    {
        TextMeshProUGUI popupText;
        CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            popupText = GetComponentInChildren<TextMeshProUGUI>();
            CollectMask.OnMaskCollected += CollectMaskOnOnMaskCollected;
            Show(false);
        }

        private void OnDestroy()
        {
            CollectMask.OnMaskCollected -= CollectMaskOnOnMaskCollected;
        }

        private void CollectMaskOnOnMaskCollected(string messageText)
        {
            popupText.text = messageText;
            Show(true);
        }

        public void Show(bool show)
        {
            canvasGroup.alpha = show ? 1 : 0;
            canvasGroup.blocksRaycasts = show;
            canvasGroup.interactable = show;
            if (show)
            {
                StopAllCoroutines();
                StartCoroutine(StartCountDown());
            }
        }
        [SerializeField] float timeToDestro = 5f;

        IEnumerator StartCountDown() 
        { 
            yield return new WaitForSeconds(timeToDestro);
            Show(false);
        }
    }
}
