using UnityEngine;

// ---------------------------------------------------
// CrateBombSimple
// �v���C���[���G���ƈ�莞�Ԍ�ɔ������锠�i�N���[�g�j�̃T���v���X�N���v�g
// ---------------------------------------------------
public class CrateBombSimple : MonoBehaviour
{
    // �����̌����ځi�X�v���C�g�Ȃǁj�p��GameObject
    public GameObject explosionSpriteObj;

    // ��������܂ł̒x�����ԁi�b�j
    public float delay = 3f;

    // �J�E���g�_�E���J�n�ς݂��ǂ����𔻒肷��t���O
    private bool isCounting = false;

    // �c�莞�Ԃ̃^�C�}�[
    private float timer = 0f;

    // ���t���[���Ă΂��֐�
    void Update()
    {
        // �J�E���g�_�E�����Ȃ�c
        if (isCounting)
        {
            // 1�t���[�����ƂɃ^�C�}�[�����炷
            timer -= Time.deltaTime;

            // �^�C�}�[��0�ȉ��ɂȂ�����c
            if (timer <= 0f)
            {
                // �����G�t�F�N�g�p�I�u�W�F�N�g�����蓖�Ă��Ă����
                if (explosionSpriteObj != null)
                {
                    // ������\���iactive�ɂ���j
                    explosionSpriteObj.SetActive(true);
                    // �����𔠂Ɠ����ʒu�ɏo��
                    explosionSpriteObj.transform.position = transform.position;
                }

                // ���̔��i�N���[�g�j���̂�����
                Destroy(gameObject);
            }
        }
    }

    // 2D��Collider���u�����v�ɐG�ꂽ�Ƃ��Ă΂��֐�
    void OnTriggerEnter2D(Collider2D other)
    {
        // �܂��J�E���g�_�E���J�n���Ă��Ȃ��āA�Ԃ��������肪Player�Ȃ�c
        if (!isCounting && other.CompareTag("Player"))
        {
            // �J�E���g�_�E���J�n�I
            isCounting = true;
            timer = delay; // �w�肵���b�������҂�
        }
    }
}
