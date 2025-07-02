using UnityEngine;

// -----------------------------------------------
// AnimatorTestSwitcher
// R�L�[�������ƁAAnimatorOverrideController�i�A�j���[�V�����㏑���p�j��؂�ւ���
// �w�肵���A�j���[�V�����𓪂���Đ�����e�X�g�p�X�N���v�g�ł�
// -----------------------------------------------
public class AnimatorTestSwitcher : MonoBehaviour
{
    // Animator�^�̕ϐ��B�L������I�u�W�F�N�g��Animator�i�A�j���[�^�[�j���i�[����
    // public�ɂ��Ȃ���Inspector��ɂ͏o�Ȃ����A�X�N���v�g���Ŏg����
    private Animator animator;

    // Inspector�Őݒ�ł���AnimatorOverrideController
    // �؂�ւ������u�㏑���p�A�j���[�V�����R���g���[���v���w�肷��
    //Inspector�ŃA�j���[�^�[�㏑���p�����蓖�ĉ\
    public AnimatorOverrideController revivedOverrideController;

    // �Q�[���J�n���iAwake�j�͂��߂Ɉ�x�������s�����֐�
    void Awake()
    {
        // ���̃I�u�W�F�N�g�ɂ��Ă���Animator�R���|�[�l���g���擾
        animator = GetComponent<Animator>();
    }

    // ���t���[���Ă΂��֐�
    void Update()
    {
        // R�L�[���������u�Ԃɏ������s���i���t���[�����肳���j
        if (Input.GetKeyDown(KeyCode.R))
        {
            // ��x�A�j���[�V�����R���g���[����null�i�����Ȃ��j�ɂ��Ă���c
            // ��x�u�Ȃ��v�ɂ��Ă���Z�b�g���������ƂŁA��������؂�ւ��
            animator.runtimeAnimatorController = null;
            // �V����AnimatorOverrideController���Z�b�g
            //���C���[0�ŁA"RePlayerStop"�Ƃ����A�j���[�V�����𓪂���Đ�
            animator.runtimeAnimatorController = revivedOverrideController;

            // "RePlayerStop"�Ƃ������O�̃A�j���[�V�������A���C���[0�Ő擪����Đ�
            animator.Play("RePlayerStop", 0, 0);

            // �f�o�b�O�p���O�iUnity��Console�ɕ\���j
            Debug.Log("�؂�ւ��e�X�g�����I");
        }
    }
}
