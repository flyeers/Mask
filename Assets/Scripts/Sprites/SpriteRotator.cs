using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    public Sprite[] sprites;          
    public float speed = 0.2f;     

    private SpriteRenderer sr;
    private int index = 0;
    private float timer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sprites.Length > 0)
            sr.sprite = sprites[0];
    }

    void Update()
    {
        if (sprites.Length == 0) return;

        timer += Time.deltaTime;

        if (timer >= speed)
        {
            timer = 0f;
            index++;

            if (index >= sprites.Length)
                index = 0;

            sr.sprite = sprites[index];
        }
    }
}
