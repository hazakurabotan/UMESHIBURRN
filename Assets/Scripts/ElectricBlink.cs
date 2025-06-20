using UnityEngine;

public class ElectricWipe : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float wipeDuration = 1f; // �S�̂̃A�j������
    private float timer = 0f;

    void Start()
    {
        // �K�v�Ȃ�O���f�[�V������������
        SetAlpha(0f, 0f); // �����ŊJ�n
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = Mathf.Repeat(timer / wipeDuration, 1f); // 0�`1����

        // "�擪����" t �̊�������������
        SetAlpha(t, 0.1f); // head: t, head��: 0.1�i����OK�j
    }

    void SetAlpha(float head, float width)
    {
        Gradient gradient = new Gradient();

        // �O���f�[�V�����̃|�C���g�ݒ�
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.white, 0.0f),
                new GradientColorKey(Color.white, 1.0f),
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(0f, 0.0f),                      // ���S����
                new GradientAlphaKey(1f, Mathf.Clamp01(head)),        // �s�����ihead�j
                new GradientAlphaKey(1f, Mathf.Clamp01(head + width)),// �s�����itail�j
                new GradientAlphaKey(0f, 1.0f)                       // ���S����
            }
        );
        lineRenderer.colorGradient = gradient;
    }
}
