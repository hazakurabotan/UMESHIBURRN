using UnityEngine;

// -----------------------------------------------
// AfterImageFade
// �L�����N�^�[�̓����ɍ��킹�Ĕ�������u�c���v���A
// ���Ԃ��o���Ƃɏ����������Ă����X�N���v�g�ł��B
// -----------------------------------------------
public class AfterImageFade : MonoBehaviour
{
    // ���̎c����������܂ł̍��v���ԁi�b�j
    public float fadeTime = 1.0f;

    // SpriteRenderer�F�X�v���C�g�摜��\�����邽�߂̃R���|�[�l���g
    private SpriteRenderer sr;

    // �c�莞�Ԃ��L�^����^�C�}�[
    private float timer;

    // �X�^�[�g���Ɉ�x�������s�����֐�
    void Start()
    {
        // ���̃I�u�W�F�N�g�ɂ��Ă���SpriteRenderer�R���|�[�l���g���擾
        sr = GetComponent<SpriteRenderer>();
        // �^�C�}�[���ő�l�ifadeTime�j�ŏ�����
        timer = fadeTime;
    }

    // ���t���[���Ă΂��֐��i1�b�Ԃɕ�������s�����j
    void Update()
    {
        // 1�t���[�����ƂɁA�o�߂����������^�C�}�[�����炷
        timer -= Time.deltaTime;

        // �c�莞�Ԃɉ����ē����x�i�A���t�@�l�j���v�Z
        // timer��fadeTime�̂Ƃ���1�i�ŏ��͊��S�ȐF�j
        // timer��0�̂Ƃ���0�i���S�ɓ����j
        float alpha = Mathf.Clamp01(timer / fadeTime);

        // SpriteRenderer���������擾�ł��Ă���ꍇ�̂�
        if (sr != null)
        {
            // ���݂̐F�iR,G,B,A�̏��j���擾
            var c = sr.color;
            // �����x�iA�j���u�v�Z����alpha �~ 0.5�v�ɕύX
            // �� 0.5�{���邱�ƂŁA�ŏ����炿����Ɣ������Ȏc���ɂ���
            c.a = alpha * 0.5f;
            // �ύX�����F��SpriteRenderer�ɔ��f
            sr.color = c;
        }

        // �^�C�}�[��0�ȉ��i�c���̎������s�����j�ɂȂ�����c
        if (timer <= 0)
            // ���̃Q�[���I�u�W�F�N�g���̂������i�c������ʂ��������j
            Destroy(gameObject);
    }
}
