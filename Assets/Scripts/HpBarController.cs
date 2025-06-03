using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{
    public Image lanternImage; // Inspector�ɃZ�b�g
    public Sprite fullFire;
    public Sprite middleFire;
    public Sprite smallFire;
    public Sprite noFire;

    // �v���C���[����A�N�Z�X����i�܂��̓C�x���g�Łj
    public void SetHp(int hp)
    {
        switch (hp)
        {
            case 3:
                lanternImage.sprite = fullFire;
                break;
            case 2:
                lanternImage.sprite = middleFire;
                break;
            case 1:
                lanternImage.sprite = smallFire;
                break;
            default:
                lanternImage.sprite = noFire;
                break;
        }
    }
}
