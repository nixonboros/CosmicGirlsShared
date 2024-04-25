using UnityEngine;

public class EffectObject : MonoBehaviour
{
    public float lifetime = 1f; //1 second default

    private SpriteRenderer spriteRenderer;
    private float elapsedTime = 0f;
    private Color initialColor;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= lifetime)
        {
            Destroy(gameObject);
            return;
        }

        // Decrease the alpha value of the color gradually
        Color color = spriteRenderer.color;
        color.a = Mathf.Lerp(initialColor.a, 0f, elapsedTime / lifetime);
        spriteRenderer.color = color;
    }
}
