using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneController : MonoBehaviour
    {
        public void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
