using UnityEngine;

public class AfterImageFade : MonoBehaviour
{
    public float fadeTime = 1.0f;
    private SpriteRenderer sr;
    private float timer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        timer = fadeTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        float alpha = Mathf.Clamp01(timer / fadeTime);
        if (sr != null)
        {
            var c = sr.color;
            c.a = alpha * 0.5f; // ç≈èâÇ∆çáÇÌÇπÇÈ
            sr.color = c;
        }
        if (timer <= 0) Destroy(gameObject);
    }
}
