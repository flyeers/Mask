using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public GameObject auxPanel;
    public void OpenScene(int sceneIndex)
    {
        GeneralManager.Instance.SceneController.LoadLevel(sceneIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SetAuxPanelEnable( bool active)
    {
        auxPanel.SetActive(active);
    }
}
