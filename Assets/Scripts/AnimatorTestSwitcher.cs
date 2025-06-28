using UnityEngine;

public class AnimatorTestSwitcher : MonoBehaviour
{
    private Animator animator; // �� public����Ȃ���OK

    public AnimatorOverrideController revivedOverrideController; // �����Inspector�ɏo��

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.runtimeAnimatorController = null;
            animator.runtimeAnimatorController = revivedOverrideController;
            animator.Play("RePlayerStop", 0, 0); // ���C���[0�A�擪����Đ�
            Debug.Log("�؂�ւ��e�X�g�����I");
        }
    }
}