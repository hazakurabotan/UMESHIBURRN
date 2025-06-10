using UnityEngine;
using UnityEngine.SceneManagement;

// このスクリプトは「ワープドア」や「ゴール」などのトリガーにアタッチします
// プレイヤーが触れると次のシーンへ移動します
public class SceneChanger : MonoBehaviour
{
    public string nextSceneName = "Stage2"; // 遷移先シーンの名前（Inspectorで指定）

    // 他のCollider2Dがこのトリガーに入った時に呼ばれる
    private void OnTriggerEnter2D(Collider2D other)
    {
        // プレイヤー（Playerタグ）が触れた場合のみ
        if (other.CompareTag("Player"))
        {
            // 指定したシーンに切り替える
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
