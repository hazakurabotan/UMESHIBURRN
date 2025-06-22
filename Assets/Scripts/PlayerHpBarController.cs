using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBarController : MonoBehaviour
{
    public Image hpImage;
    public Sprite[] hpSprites; // HP0`HP3‚Ì‰æ‘œ‚ð‚±‚±‚ÉƒZƒbƒg
    public int maxHp = 3;

    public void SetHp(int hp)
    {
        int clamped = Mathf.Clamp(hp, 0, maxHp);
        if (hpSprites != null && hpSprites.Length > clamped)
        {
            hpImage.sprite = hpSprites[clamped];
        }
    }
}
