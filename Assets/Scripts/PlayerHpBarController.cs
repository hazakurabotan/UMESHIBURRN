using UnityEngine;
using UnityEngine.UI;

// -----------------------------------------------------
// PlayerHpBarController
// �v���C���[�p�́u�n�[�g�^�v��u�A�C�R���^�vHP�o�[�𐧌䂷��N���X
// �iHP0�`HP3�܂őΉ��j
// -----------------------------------------------------
public class PlayerHpBarController : MonoBehaviour
{
    public Image hpImage;            // ���ۂɕ\������HP�o�[��Image
    public Sprite[] hpSprites;       // HP���Ƃ̉摜�i�C���X�y�N�^�[�ŕ��ׂăZ�b�g�j
    public int maxHp = 3;            // �ő�HP�i�摜�z��Ƒ����Ă����j

    // --- HP�̒l�ɂ��킹�ăo�[�摜��؂�ւ��� ---
    public void SetHp(int hp)
    {
        // �l���͈͊O�ɂȂ�Ȃ��悤0�`maxHp�ɐ���
        int clamped = Mathf.Clamp(hp, 0, maxHp);

        // �z��Ɖ摜�������Ɨp�ӂ���Ă��邩�`�F�b�N
        if (hpSprites != null && hpSprites.Length > clamped)
        {
            // HP�l�ɑΉ������摜�֍����ւ�
            hpImage.sprite = hpSprites[clamped];
        }
    }
}
