using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �e��V�F���̎����폜���Ǘ�����X�N���v�g
public class ShellController : MonoBehaviour
{
    public float deleteTime = 3.0f; // ���b��Ɏ����ŏ�����

    // ====== �ŏ���1�񂾂��Ă΂�� ======
    void Start()
    {
        // deleteTime�b��ɂ��̃I�u�W�F�N�g�������폜
        Destroy(gameObject, deleteTime);
    }

    // ====== ���t���[���Ă΂��i����͎g���Ă��܂���j======
    void Update()
    {
        // �����Ȃ�
    }

    // ====== �����ƂԂ��������iTrigger���m�̔���j ======
    void OnTriggerEnter2D(Collider2D collision)
    {
        // �����ɐڐG�����炷������
        Destroy(gameObject);
    }
}
