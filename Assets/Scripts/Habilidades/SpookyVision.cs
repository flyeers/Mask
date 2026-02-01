using UnityEngine;
using UnityEngine.Rendering;

public class SpookyVision : MonoBehaviour
{
    [SerializeField] string tagName = "spooky";
    [SerializeField] Volume spookyVolume;
    private GameObject[] spookyObjects = { };

    private void OnEnable()
    {
        if (spookyObjects.Length == 0) 
        {
            spookyObjects = GameObject.FindGameObjectsWithTag(tagName);
        }
        if (spookyVolume) spookyVolume.gameObject.SetActive(true);
        SetRender(true);
    }

    private void OnDisable()
    {
        if (spookyVolume) spookyVolume.gameObject.SetActive(false);
        SetRender(false);
    }

    private void SetRender(bool render) 
    {
        foreach (var obj in spookyObjects) 
        {
            if(obj.TryGetComponent<Renderer>(out Renderer rend)) 
            { 
                rend.enabled = render;
            }
        }

    }

}
