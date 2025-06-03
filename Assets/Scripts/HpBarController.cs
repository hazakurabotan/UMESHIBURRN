using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{
    public Image lanternImage; // Inspectorにセット
    public Sprite fullFire;
    public Sprite middleFire;
    public Sprite smallFire;
    public Sprite noFire;

    // プレイヤーからアクセスする（またはイベントで）
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
