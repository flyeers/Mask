using UnityEngine;
using System.Collections;

public class SpriteReproducer : MonoBehaviour
{
    public Sprite[] frames;
    public float frameTime = 0.1f;

    [SerializeField] SpriteRenderer rightSprite;
    [SerializeField] SpriteRenderer leftSprite;

    private void OnEnable()
    {
        //sr = GetComponent<SpriteRenderer>();
    }

    public void ReproduceSpriteSequence(bool right) 
    {
        if (right)
        {
            rightSprite.enabled = true;
            StartCoroutine(RightPlay());
        }
        else
        {
            leftSprite.enabled = true;
            StartCoroutine(LeftPlay());
        }
    }

    IEnumerator RightPlay()
    {
        foreach (var frame in frames)
        {
            rightSprite.sprite = frame;
            yield return new WaitForSeconds(frameTime);
        }
        rightSprite.enabled = false;
    }

    IEnumerator LeftPlay()
    {
        foreach (var frame in frames)
        {
            leftSprite.sprite = frame;
            yield return new WaitForSeconds(frameTime);
        }
        leftSprite.enabled = false;
    }
}
