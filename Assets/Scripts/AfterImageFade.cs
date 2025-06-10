using UnityEngine;

// AfterImageFade�N���X
// �L�����N�^�[�Ȃǂ́u�c���v�p�I�u�W�F�N�g�����X�ɓ����ɂȂ��ď����铮�������X�N���v�g�ł��B
public class AfterImageFade : MonoBehaviour
{
    // �c�������S�ɏ�����܂ł̎��ԁi�b�j
    public float fadeTime = 1.0f;

    // SpriteRenderer�i�X�v���C�g�摜��`�悷��R���|�[�l���g�j�ւ̎Q��
    private SpriteRenderer sr;

    // �c��̎��Ԃ��L�^����^�C�}�[
    private float timer;

    // �X�^�[�g���Ɉ�x�����Ă΂��֐�
    void Start()
    {
        // ���̃I�u�W�F�N�g�ɕt���Ă���SpriteRenderer���擾
        sr = GetComponent<SpriteRenderer>();

        // �^�C�}�[��������܂ł̎��Ԃŏ�����
        timer = fadeTime;
    }

    // ���t���[���Ă΂��֐�
    void Update()
    {
        // �o�ߎ��ԕ������^�C�}�[�����炷
        timer -= Time.deltaTime;

        // �c�莞�Ԃɉ����ē����x�i�A���t�@�l�j���v�Z�i0�`1�͈̔͂Ɏ��߂�j
        float alpha = Mathf.Clamp01(timer / fadeTime);

        // sr�iSpriteRenderer�j�������Ǝ擾�ł��Ă���΁c
        if (sr != null)
        {
            // ���݂̐F�f�[�^���擾
            var c = sr.color;
            // �A���t�@�l�i�����x�j�����v�Z�l�ɕύX
            c.a = alpha * 0.5f; // �ŏ��̔������x�ɍ��킹��0.5�{
            // �ύX�����F��K�p
            sr.color = c;
        }

        // �^�C�}�[��0�ȉ��ɂȂ�����A���̃I�u�W�F�N�g���폜�i�c����������j
        if (timer <= 0)
            Destroy(gameObject);
    }
}
