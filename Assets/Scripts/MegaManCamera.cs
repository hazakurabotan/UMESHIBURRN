using UnityEngine;

public class MegaManCamera : MonoBehaviour
{
    public Transform player;
    public float screenWidth = 16f; // 1画面ぶんの横幅（例：16ユニット）
    public float screenHeight = 9f; // 1画面ぶんの高さ
    public float yOffset = 0f; // 追加！
    private Vector2 currentScreenOrigin;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        UpdateScreenOrigin();
    }

    void Update()
    {
        Vector2 playerPos = player.position;

        // プレイヤーが現在の画面範囲外に出たらカメラ移動
        if (playerPos.x < currentScreenOrigin.x)
        {
            currentScreenOrigin.x -= screenWidth;
            MoveCamera();
        }
        else if (playerPos.x > currentScreenOrigin.x + screenWidth)
        {
            currentScreenOrigin.x += screenWidth;
            MoveCamera();
        }
        // ↑縦スクロールもしたい場合は同じノリでyも判定

        // 追加：上下方向のパネル切り替えもしたい場合
        // if (playerPos.y < currentScreenOrigin.y) { ... }
        // if (playerPos.y > currentScreenOrigin.y + screenHeight) { ... }
    }

    void MoveCamera()
    {
        // カメラの位置をパネルの左下基準の中央に
        float camX = currentScreenOrigin.x + screenWidth / 2f;
        float camY = currentScreenOrigin.y + screenHeight / 2f + yOffset; // ←ここ修正
        transform.position = new Vector3(camX, camY, transform.position.z);
    }

    void UpdateScreenOrigin()
    {
        // プレイヤーがいるパネル（画面区画）の左下座標
        float originX = Mathf.Floor(player.position.x / screenWidth) * screenWidth;
        float originY = Mathf.Floor(player.position.y / screenHeight) * screenHeight;
        currentScreenOrigin = new Vector2(originX, originY);

        MoveCamera();
    }
}
