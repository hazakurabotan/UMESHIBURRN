using UnityEngine;

// -----------------------------------------------------------
// FireWavyLine
// LineRenderer���g���āu���v��u�r�[���v�̂悤��
// ���˂��˗h������`�悷��X�N���v�g
// -----------------------------------------------------------
public class FireWavyLine : MonoBehaviour
{
    // ���̌����ڂɂȂ����`��LineRenderer
    public LineRenderer lineRenderer;

    // �����\�����钸�_�̐��i�����قǍׂ������炩�ɂȂ�j
    public int pointCount = 20;

    // ���̒����i�P�ʂ̓��[���h���W�j
    public float length = 2f;

    // �h��i���˂�j�̍ő卂��
    public float waveHeight = 0.2f;

    // �h��i���˂�j�̑���
    public float waveSpeed = 2f;

    // ������
    void Start()
    {
        // LineRenderer�����ݒ�Ȃ玩������擾
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
        // ���_���iPosition Count�j��ݒ�
        lineRenderer.positionCount = pointCount;
    }

    // ���t���[���Ă΂��
    void Update()
    {
        float time = Time.time * waveSpeed;  // ���Ԃɂ��h��
        Vector3 basePos = transform.position; // �I�u�W�F�N�g�̌��݈ʒu�����

        for (int i = 0; i < pointCount; i++)
        {
            // 0�`length�܂ŋϓ��ɕ�������x���W
            float x = (float)i / (pointCount - 1) * length;

            // y���W�F�T�C���g�Łu���˂��ˁv�{PerlinNoise�Ŕ����Ȃ�炬��ǉ�
            float y = Mathf.Sin(x * 10f + time + i) * waveHeight * Mathf.PerlinNoise(time, x);

            // ���_i�̈ʒu���Z�b�g�i����W�{x, y�j
            lineRenderer.SetPosition(i, basePos + new Vector3(x, y, 0));
        }
    }
}
