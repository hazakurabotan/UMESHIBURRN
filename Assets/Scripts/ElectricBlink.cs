using UnityEngine;

public class ElectricWipe : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float wipeDuration = 1f; // 全体のアニメ時間
    private float timer = 0f;

    void Start()
    {
        // 必要ならグラデーションを初期化
        SetAlpha(0f, 0f); // 透明で開始
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = Mathf.Repeat(timer / wipeDuration, 1f); // 0～1周期

        // "先頭から" t の割合だけ見える
        SetAlpha(t, 0.1f); // head: t, head幅: 0.1（調整OK）
    }

    void SetAlpha(float head, float width)
    {
        Gradient gradient = new Gradient();

        // グラデーションのポイント設定
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.white, 0.0f),
                new GradientColorKey(Color.white, 1.0f),
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(0f, 0.0f),                      // 完全透明
                new GradientAlphaKey(1f, Mathf.Clamp01(head)),        // 不透明（head）
                new GradientAlphaKey(1f, Mathf.Clamp01(head + width)),// 不透明（tail）
                new GradientAlphaKey(0f, 1.0f)                       // 完全透明
            }
        );
        lineRenderer.colorGradient = gradient;
    }
}
