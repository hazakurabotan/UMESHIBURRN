using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{
    public Image barImage;  // ← ここはImageにして、Sprite差し替え用
    private Sprite[] hpSprites;

    void Awake()
    {
        // 画像を一括ロード（Resources/HpBars/HP0.png ... HP15.png）
        hpSprites = new Sprite[16];
        for (int i = 0; i <= 15; i++)
        {
            hpSprites[i] = Resources.Load<Sprite>($"HpBars/HP{i}");
        }
    }

    /// <summary>
    /// HPバーを更新（currentHp: 0〜15, maxHp: 15）
    /// </summary>
    public void SetHp(int currentHp, int maxHp = 15)
    {
        // 範囲外チェック
        int idx = Mathf.Clamp(currentHp, 0, 15);

        if (hpSprites != null && idx < hpSprites.Length && hpSprites[idx] != null)
            barImage.sprite = hpSprites[idx];
    }
}