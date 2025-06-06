using UnityEngine;
using UnityEngine.UI;

public class BossHpBarController : MonoBehaviour
{
    public Image hpImage;
    public Sprite[] hpSprites;   // �̗͒i�K���Ƃ̉摜
    public int maxHp = 10;
    public int currentHp = 10;

    public void SetHp(int hp)
    {
        currentHp = Mathf.Clamp(hp, 0, maxHp);
        if (hpSprites != null && hpSprites.Length > currentHp)
        {
            hpImage.sprite = hpSprites[currentHp];
        }
    }
}
