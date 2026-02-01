using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AutoCreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 400f; 
    public float endTime = 10f; 
    public int sceneIndex = 7; 

    private RectTransform rectTransform;
    private float timer = 0f;
    private bool endCredits = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (!endCredits)
        {
            timer += Time.deltaTime;

            if (timer >= endTime)
            {
                endCredits = true;
            }
        }


        if (Keyboard.current.enterKey.wasPressedThisFrame || endCredits)
        {
            GeneralManager.Instance.SceneController.LoadLevel(sceneIndex);
        }
    }


}
