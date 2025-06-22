using UnityEngine;

public class FireWavyLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int pointCount = 20;       // ���ׂ̍���
    public float length = 2f;         // ���̒���
    public float waveHeight = 0.2f;   // �h�ꕝ
    public float waveSpeed = 2f;      // �h�ꑬ�x

    void Start()
    {
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = pointCount;
    }

    void Update()
    {
        float time = Time.time * waveSpeed;
        Vector3 basePos = transform.position; // �I�u�W�F�N�g�̈ʒu�����

        for (int i = 0; i < pointCount; i++)
        {
            float x = (float)i / (pointCount - 1) * length;
            float y = Mathf.Sin(x * 10f + time + i) * waveHeight * Mathf.PerlinNoise(time, x);
            // �������Ⴄ�I
            lineRenderer.SetPosition(i, basePos + new Vector3(x, y, 0));
        }
    }
}
