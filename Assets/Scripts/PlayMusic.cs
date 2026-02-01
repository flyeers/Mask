using FMODUnity;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    [SerializeField] private EventReference _eventReference;
    
    private void Start()
    {
        BackgroundMusicManager.Instance.PlayMusic(_eventReference);
    }
}
 