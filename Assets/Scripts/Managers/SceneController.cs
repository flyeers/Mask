using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneController : MonoBehaviour
    {
        private bool fading = false;
        private bool loading = false;
        
        public void ReloadCurrentScene()
        {
            LoadLevel(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadLevel(int sceneIndex)
        {
            if (loading)
            {
                return;
            }
            
            loading = true;
            IrisFadeManager.Instance.FadeOutCompleted += InstanceOnFadeOutCompleted;
            IrisFadeManager.Instance.StartFadeOut();
            StartCoroutine(LoadLevel_CO(sceneIndex));
        }

        private IEnumerator LoadLevel_CO(int sceneIndex)
        {
            fading = true;
            yield return new WaitUntil(() => !fading);
            SceneManager.LoadScene(sceneIndex);
        }

        private void InstanceOnFadeOutCompleted()
        {
            fading = false;
            IrisFadeManager.Instance.FadeOutCompleted -= InstanceOnFadeOutCompleted;
        }
    }
}
