using UnityEngine;
using UnityEngine.UI;

// -----------------------------------------------
// HpBarController
// プレイヤーのHPバー（画像差し替え式）を管理するスクリプト
// -----------------------------------------------
public class HpBarController : MonoBehaviour
{
    public Image barImage;      // HPバー表示用のImageコンポーネント
    private Sprite[] hpSprites; // HP値ごとのバー画像配列（0～15まで16枚）

    void Awake()
    {
        // --- HPバー画像（Resources/HpBars/HP0.png ～ HP15.png）を一括ロード ---
        hpSprites = new Sprite[16]; // 配列を16個分用意（0～15HP用）
        for (int i = 0; i <= 15; i++)
        {
            // 各HP値に対応する画像ファイルを読み込む
            hpSprites[i] = Resources.Load<Sprite>($"HpBars/HP{i}");
        }
    }

    /// <summary>
    /// HPバーを現在値にあわせて更新（0～15）
    /// </summary>
    /// <param name="currentHp">現在のHP</param>
    /// <param name="maxHp">最大HP（デフォルト15）</param>
    public void SetHp(int currentHp, int maxHp = 15)
    {
        // HP値が範囲外にならないよう制限
        int idx = Mathf.Clamp(currentHp, 0, 15);

        // 画像が正しくロードされていれば、そのスプライトをセット
        if (hpSprites != null && idx < hpSprites.Length && hpSprites[idx] != null)
            barImage.sprite = hpSprites[idx];
    }
}
