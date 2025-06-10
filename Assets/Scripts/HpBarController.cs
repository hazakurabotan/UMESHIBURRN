using UnityEngine;
using UnityEngine.UI;

// HP�ɉ����ă����^���̉��A�C�R����؂�ւ���UI�X�N���v�g
public class HpBarController : MonoBehaviour
{
    public Image lanternImage; // �\���p��Image�R���|�[�l���g�iInspector�ŃZ�b�g�j
    public Sprite fullFire;    // HP���^���i�����傫���j
    public Sprite middleFire;  // HP�����炢�i���������炢�j
    public Sprite smallFire;   // HP���Ȃ߁i�����������j
    public Sprite noFire;      // HP�[���i���Ȃ��j

    // �v���C���[����Ă΂��֐�
    // hp�̒l�ɉ����ĉ摜��؂�ւ��܂��i��Fhp=3�Ȃ�fullFire�j
    public void SetHp(int hp)
    {
        switch (hp)
        {
            case 3:
                lanternImage.sprite = fullFire;    // HP3�̂Ƃ�
                break;
            case 2:
                lanternImage.sprite = middleFire;  // HP2�̂Ƃ�
                break;
            case 1:
                lanternImage.sprite = smallFire;   // HP1�̂Ƃ�
                break;
            default:
                lanternImage.sprite = noFire;      // ����ȊO�i0��}�C�i�X�j
                break;
        }
    }
}
