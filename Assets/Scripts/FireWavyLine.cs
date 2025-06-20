using UnityEngine;

public class FireWavyLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int pointCount = 20;       // ü‚Ì×‚©‚³
    public float length = 2f;         // ‰Š‚Ì’·‚³
    public float waveHeight = 0.2f;   // —h‚ê•
    public float waveSpeed = 2f;      // —h‚ê‘¬“x

    void Start()
    {
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = pointCount;
    }

    void Update()
    {
        float time = Time.time * waveSpeed;
        for (int i = 0; i < pointCount; i++)
        {
            float x = (float)i / (pointCount - 1) * length;
            float y = Mathf.Sin(x * 10f + time + i) * waveHeight * Mathf.PerlinNoise(time, x);
            // «ã•ûŒü‚ÉL‚Ñ‚é‰ŠiÀ•W‚ÍD‚«‚É’²®‚µ‚Ä‚Ëj
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}
