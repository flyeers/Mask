using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class BackgroundMusicManager : MonoBehaviour
{
    private static BackgroundMusicManager _instance;
    public static BackgroundMusicManager Instance => _instance;

    private EventInstance _musicInstance;
    private EventReference _currentReference;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(EventReference eventReference)
    {
        // 1. Check if the event is valid
        if (eventReference.IsNull) return;

        // 2. Avoid restarting the same music if it's already playing
        if (_currentReference.Guid == eventReference.Guid) return;

        // 3. Stop previous music if it exists
        StopMusic();

        // 4. Create and start the new instance
        _musicInstance = RuntimeManager.CreateInstance(eventReference);
        _musicInstance.start();
        
        // Store reference to check against in step 2
        _currentReference = eventReference;
    }

    public void StopMusic()
    {
        if (_musicInstance.isValid())
        {
            // ALLOWFADEOUT respects the AHDSR modulation you set in FMOD Studio
            _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            
            // Release clears the instance from memory once it stops
            _musicInstance.release();
            
            _currentReference = default;
        }
    }

    // Optional: Clean up if the manager is ever destroyed
    private void OnDestroy()
    {
        if (_instance == this)
        {
            StopMusic();
        }
    }
}