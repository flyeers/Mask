using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Managers;

public class TitleScreen : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;          // Imagen UI (negra normalmente)
    public float delayBeforeFade = 0.5f;
    public float fadeDuration = 1.5f;

    [Header("Scene")]
    public string sceneToLoad;

    void Start()
    {
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        // Espera inicial
        yield return new WaitForSeconds(delayBeforeFade);

        // float t = 0f;
        // Color color = fadeImage.color;
        //
        // // Fade out (alpha 1 → 0)
        // while (t < fadeDuration)
        // {
        //     t += Time.deltaTime;
        //     color.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
        //     fadeImage.color = color;
        //     yield return null;
        // }
        //
        // color.a = 0f;
        // fadeImage.color = color;

        // Cambia de escena
        GeneralManager.Instance.SceneController.LoadLevel(0);
        //SceneManager.LoadScene(sceneToLoad);
    }
}