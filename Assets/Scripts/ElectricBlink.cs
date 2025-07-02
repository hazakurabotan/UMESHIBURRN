using UnityEngine;

// ---------------------------------------------------
// ElectricWipe
// LineRenderer���g���āu�V���b�v�Ɛ����L�т�G�t�F�N�g�����X�N���v�g
// ---------------------------------------------------
public class ElectricWipe : MonoBehaviour
{
    // �G�t�F�N�g�p��LineRenderer
    public LineRenderer lineRenderer;

    // �����L�т���܂ł̃A�j���[�V�������ԁi�b�j
    public float wipeDuration = 1f;

    // �o�ߎ��Ԃ��L�^����^�C�}�[
    private float timer = 0f;

    // �Q�[���J�n����1�x�������s�����
    void Start()
    {
        // �ŏ��͊��S�ɓ����ȏ�ԂŊJ�n
        SetAlpha(0f, 0f);
    }

    // ���t���[���Ă΂��i���̐L�т�A�j���[�V�����𐧌�j
    void Update()
    {
        // �o�ߎ��Ԃ����Z
        timer += Time.deltaTime;

        // t��0�`1�Ń��[�v�i0��1��0��1�c�j
        float t = Mathf.Repeat(timer / wipeDuration, 1f);

        // head�i�擪�j����width�̕������s�����Ō�����
        SetAlpha(t, 0.1f); // 0.1f�́u���̑����v�݂����ȃC���[�W�B����OK
    }

    // head����width�͈̔͂�������s�����ɂ��A����ȊO�͓����ɂ���
    void SetAlpha(float head, float width)
    {
        Gradient gradient = new Gradient();

        // �O���f�[�V�����̐F�ݒ�i�F�͔��ŌŒ�j
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.white, 0.0f), // �J�n�_
                new GradientColorKey(Color.white, 1.0f), // �I�_
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(0f, 0.0f),                            // �J�n�͓���
                new GradientAlphaKey(1f, Mathf.Clamp01(head)),             // head�ʒu�ŕs����
                new GradientAlphaKey(1f, Mathf.Clamp01(head + width)),     // head+width�ʒu���s����
                new GradientAlphaKey(0f, 1.0f)                             // �I���_�͓���
            }
        );
        // LineRenderer�ɃO���f�[�V������K�p
        lineRenderer.colorGradient = gradient;
    }
}
