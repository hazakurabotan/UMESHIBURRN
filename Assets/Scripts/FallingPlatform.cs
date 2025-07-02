using UnityEngine;

// ----------------------------------------------------
// FallingPlatform
// �v���C���[�����Ƃ��΂炭���ė������A�Ō�͏����鑫��̃X�N���v�g
// ----------------------------------------------------
public class FallingPlatform : MonoBehaviour
{
    // �v���C���[������Ă��痎����܂ł̑҂����ԁi�b�j
    public float fallDelay = 2f;

    // ���̑����Rigidbody2D�i���������p�j
    private Rigidbody2D rb;

    // ���łɗ����J�n���Ă��邩�̃t���O
    private bool isFalling = false;

    // �Q�[���J�n���Ɉ�x�������s
    void Start()
    {
        // ���̃I�u�W�F�N�g�ɕt���Ă���Rigidbody2D���擾
        rb = GetComponent<Rigidbody2D>();
    }

    // �����ƂԂ��������ɌĂ΂��i2D�����p�j
    void OnCollisionEnter2D(Collision2D collision)
    {
        // ���肪�uPlayer�v�^�O�̃I�u�W�F�N�g�A���܂��������Ă��Ȃ����
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            isFalling = true; // ��x��������
            // fallDelay�b���DropPlatform()���Ăԁi�^�C�}�[�Z�b�g�j
            Invoke("DropPlatform", fallDelay);
        }
    }

    // ����𗎉������鏈��
    void DropPlatform()
    {
        // Rigidbody2D��Dynamic�i�d�͂ɏ]���j�ɕύX�������J�n
        rb.bodyType = RigidbodyType2D.Dynamic;
        // ����I�u�W�F�N�g���̂�2�b��ɏ����i������̌�n���j
        Destroy(gameObject, 2f);
    }
}
