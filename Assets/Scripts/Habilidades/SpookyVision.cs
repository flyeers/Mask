using UnityEngine;

public class SpookyVision : MonoBehaviour
{
    [SerializeField] string tagName = "spooky";

    private GameObject[] spookyObjects = { };

    private void OnEnable()
    {
        if (spookyObjects.Length == 0) 
        {
            spookyObjects = GameObject.FindGameObjectsWithTag(tagName);
        }
        SetRender(true);
    }

    private void OnDisable()
    {
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
