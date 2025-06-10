using UnityEngine;

// AfterImageFadeクラス
// キャラクターなどの「残像」用オブジェクトが徐々に透明になって消える動きを作るスクリプトです。
public class AfterImageFade : MonoBehaviour
{
    // 残像が完全に消えるまでの時間（秒）
    public float fadeTime = 1.0f;

    // SpriteRenderer（スプライト画像を描画するコンポーネント）への参照
    private SpriteRenderer sr;

    // 残りの時間を記録するタイマー
    private float timer;

    // スタート時に一度だけ呼ばれる関数
    void Start()
    {
        // このオブジェクトに付いているSpriteRendererを取得
        sr = GetComponent<SpriteRenderer>();

        // タイマーを消えるまでの時間で初期化
        timer = fadeTime;
    }

    // 毎フレーム呼ばれる関数
    void Update()
    {
        // 経過時間分だけタイマーを減らす
        timer -= Time.deltaTime;

        // 残り時間に応じて透明度（アルファ値）を計算（0～1の範囲に収める）
        float alpha = Mathf.Clamp01(timer / fadeTime);

        // sr（SpriteRenderer）がちゃんと取得できていれば…
        if (sr != null)
        {
            // 現在の色データを取得
            var c = sr.color;
            // アルファ値（透明度）だけ計算値に変更
            c.a = alpha * 0.5f; // 最初の半透明度に合わせて0.5倍
            // 変更した色を適用
            sr.color = c;
        }

        // タイマーが0以下になったら、このオブジェクトを削除（残像が消える）
        if (timer <= 0)
            Destroy(gameObject);
    }
}
