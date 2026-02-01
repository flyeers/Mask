using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class IrisFadeManager : MonoBehaviour
    {
        public static IrisFadeManager Instance { get; private set; }
    
        [Header("Referencias UI")]
        [SerializeField] private Canvas fadeCanvas;
        [SerializeField] private Image fadeImage;
        [SerializeField] private Material fadeMaterial;
        
        [Header("Configuración")]
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private float maxRadius = 10f;

        private Camera mainCamera;
        private Transform player;
        private Material materialInstance;
        
        private static readonly int FadeAmountID = Shader.PropertyToID("_FadeAmount");
        private static readonly int CenterID = Shader.PropertyToID("_Center");

        public event Action FadeOutCompleted;
        public event Action FadeInCompleted;
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
                // Configurar el canvas para que esté siempre encima
                fadeCanvas.sortingOrder = 9999;
                
                // Crear instancia del material
                materialInstance = new Material(fadeMaterial);
                fadeImage.material = materialInstance;
                
                // Suscribirse al evento de carga de escena
                SceneManager.sceneLoaded += OnSceneLoaded;
                
                // Iniciar con fade desde el jugador (opcional)
                StartCoroutine(InitialFadeIn());
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponentInChildren<Camera>();
            
            // Hacer fade in automáticamente al cargar escena
            if (mode == LoadSceneMode.Single)
            {
                StartCoroutine(FadeIn());
            }
        }

        private IEnumerator InitialFadeIn()
        {
            // Esperar un frame para que todo se inicialice
            yield return null;
            
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponentInChildren<Camera>();
            
            yield return FadeIn();
        }

        public void StartFadeOut()
        {
            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
        {
            UpdatePlayerPosition();
            materialInstance.SetFloat(FadeAmountID, maxRadius);
            
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / fadeDuration;
                materialInstance.SetFloat(FadeAmountID, Mathf.Lerp(maxRadius,0f, progress));
                yield return null;
            }
            
            materialInstance.SetFloat(FadeAmountID, 0f);
            FadeOutCompleted?.Invoke();
        }

        private IEnumerator FadeIn()
        {
            UpdatePlayerPosition();
            materialInstance.SetFloat(FadeAmountID, 0f);
            
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / fadeDuration;
                materialInstance.SetFloat(FadeAmountID, Mathf.Lerp(0f,maxRadius, progress));
                yield return null;
            }
            
            materialInstance.SetFloat(FadeAmountID, maxRadius);
            FadeInCompleted?.Invoke();
        }

        private void UpdatePlayerPosition()
        {
            if (player == null || mainCamera == null)
            {
                // Centro de pantalla por defecto
                materialInstance.SetVector(CenterID, new Vector2(0.5f, 0.5f));
                return;
            }

            Vector3 screenPos = mainCamera.WorldToScreenPoint(player.position);
            Vector2 shaderCoords = new Vector2(
                screenPos.x / Screen.width,
                screenPos.y / Screen.height
            );
            
            materialInstance.SetVector(CenterID, shaderCoords);
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
