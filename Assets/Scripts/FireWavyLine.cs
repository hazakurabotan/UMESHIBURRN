using UnityEngine;

public class FireWavyLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int pointCount = 20;       // 線の細かさ
    public float length = 2f;         // 炎の長さ
    public float waveHeight = 0.2f;   // 揺れ幅
    public float waveSpeed = 2f;      // 揺れ速度

    void Start()
    {
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = pointCount;
    }

    void Update()
    {
        float time = Time.time * waveSpeed;
        Vector3 basePos = transform.position; // オブジェクトの位置を基準に

        for (int i = 0; i < pointCount; i++)
        {
            float x = (float)i / (pointCount - 1) * length;
            float y = Mathf.Sin(x * 10f + time + i) * waveHeight * Mathf.PerlinNoise(time, x);
            // ここが違う！
            lineRenderer.SetPosition(i, basePos + new Vector3(x, y, 0));
        }
    }
}
