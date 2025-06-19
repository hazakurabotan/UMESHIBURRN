using UnityEngine;

public class TrapPlatform : MonoBehaviour
{
    public GameObject spikeObject; // 出現・消去させるトゲ（needleオブジェクトなど）
    public float spikeShowTime = 1.0f; // トゲが表示される秒数
    public float activateDistance = 1.5f; // プレイヤーが近づく距離

    private bool isActive = false; // トゲが表示中か
    private float timer = 0f;
    private Transform player;

    void Start()
    {
        if (spikeObject != null)
            spikeObject.SetActive(false); // 最初は非表示

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(player.position, transform.position);

        // プレイヤーが近づいたらトゲ出現＆タイマー起動
        if (!isActive && dist <= activateDistance)
        {
            ActivateSpike();
        }

        // 表示中はカウント
        if (isActive)
        {
            timer += Time.deltaTime;
            if (timer >= spikeShowTime)
            {
                DeactivateSpike();
            }
        }
    }

    void ActivateSpike()
    {
        if (spikeObject != null)
            spikeObject.SetActive(true);
        isActive = true;
        timer = 0f;
    }

    void DeactivateSpike()
    {
        if (spikeObject != null)
            spikeObject.SetActive(false);
        isActive = false;
        timer = 0f;
    }
}
