using Managers;
using UnityEngine;

public class StatueEnd : MonoBehaviour
{
    [SerializeField] private SceneController sceneController; 
    [SerializeField] private int sceneIndex = 6; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sceneController.LoadLevel(sceneIndex);
        }
    }
}
