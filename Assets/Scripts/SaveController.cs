using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----------------------------------------------
// SaveController
// �V�[�����܂����ł������Ȃ��u�f�[�^�ێ��I�u�W�F�N�g�v
// ��F�C�x���g����󋵂Ȃǂ�HashSet�ŊǗ�
// ----------------------------------------------
public class SaveController : MonoBehaviour
{
    // --- �V���O���g�����i�ǂ̃V�[���ł�1�������݂���d�g�݁j---
    public static SaveController instance;

    // --- �i��j�u�^�O�{ID�v�ň�x������C�x���g���L�^����Z�b�g ---
    public HashSet<(string tag, int arrangeId)>
        consumedEvent = new HashSet<(string tag, int arrangeId)>();

    private void Awake()
    {
        // �܂��B��̃C���X�^���X�������ꍇ�A�������u�B��́vinstance�Ƃ��ĕۑ�
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �V�[���؂�ւ��Ă������Ȃ�
        }
        else
        {
            // ���łɑ���1����ꍇ�́A�����͔j���i�d���h�~�j
            Destroy(gameObject);
        }
    }

    // --- ��F�C�x���g������L�^ ---
    void ConsumedEvent(string tag, int arrangeId)
    {
        consumedEvent.Add((tag, arrangeId));
    }

    // --- ��F�C�x���g�����łɏ���ς݂����ׂ� ---
    bool IsConsumed(string tag, int arrangeId)
    {
        return consumedEvent.Contains((tag, arrangeId));
    }

    // Start, Update �͌���g���Ă��Ȃ�
}
