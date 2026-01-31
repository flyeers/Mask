using UnityEngine;

namespace Managers
{
    public class GeneralManager : MonoBehaviour
    {
        public static GeneralManager Instance => _instance;
        private static GeneralManager _instance;

        [SerializeField]
        private GameManager gameManager;
        [SerializeField]
        private SceneController sceneController;
        
        public GameManager GameManager => gameManager;
        public SceneController SceneController => sceneController;
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }

            _instance = this;
        }
    }
}
