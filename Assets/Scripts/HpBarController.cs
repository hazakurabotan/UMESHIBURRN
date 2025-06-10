using UnityEngine;
using UnityEngine.UI;

// HPに応じてランタンの炎アイコンを切り替えるUIスクリプト
public class HpBarController : MonoBehaviour
{
    public Image lanternImage; // 表示用のImageコンポーネント（Inspectorでセット）
    public Sprite fullFire;    // HP満タン（炎が大きい）
    public Sprite middleFire;  // HP中くらい（炎が中くらい）
    public Sprite smallFire;   // HP少なめ（炎が小さい）
    public Sprite noFire;      // HPゼロ（炎なし）

    // プレイヤーから呼ばれる関数
    // hpの値に応じて画像を切り替えます（例：hp=3ならfullFire）
    public void SetHp(int hp)
    {
        switch (hp)
        {
            case 3:
                lanternImage.sprite = fullFire;    // HP3のとき
                break;
            case 2:
                lanternImage.sprite = middleFire;  // HP2のとき
                break;
            case 1:
                lanternImage.sprite = smallFire;   // HP1のとき
                break;
            default:
                lanternImage.sprite = noFire;      // それ以外（0やマイナス）
                break;
        }
    }
}
