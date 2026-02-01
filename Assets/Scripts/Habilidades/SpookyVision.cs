using UnityEngine;
using UnityEngine.Rendering;

public class SpookyVision : MonoBehaviour
{
    [SerializeField] string tagName = "spooky";
    [SerializeField] Volume normalVolume;
    [SerializeField] Volume spookyVolume;
    private GameObject[] spookyObjects = { };
    
    [SerializeField] private FMODUnity.StudioEventEmitter _spookyVisionSfx;

    private void OnEnable()
    {
        if (spookyObjects.Length == 0) 
        {
            spookyObjects = GameObject.FindGameObjectsWithTag(tagName);
        }
        if (normalVolume)
        {
            normalVolume.gameObject.SetActive(false);
        }
        if (spookyVolume)
        {
            spookyVolume.gameObject.SetActive(true);
        }
        SetRender(true);
    }

    private void OnDisable()
    {
        if (spookyVolume && normalVolume)
        {
            spookyVolume.gameObject.SetActive(false);
        }
        if (normalVolume)
        {
            normalVolume.gameObject.SetActive(true);
        }
        SetRender(false);
    }

    private void SetRender(bool render) 
    {
        if (render)
        {
            _spookyVisionSfx.Play();
        }
        
        foreach (var obj in spookyObjects) 
        {
            if(obj != null)
            {
                var rend = obj.GetComponentInChildren<Renderer>();
                if (rend)
                    rend.enabled = render;
            }
        }

    }

}
