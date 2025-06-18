using UnityEngine;

public class ExtendPlatform : MonoBehaviour
{
    public Transform player;        // Player（インスペクターでアサイン）
    public float triggerDistance = 2.5f; // 近づいたと判定する距離
    public float extendDistance = 2.0f;  // どれくらい横に出すか
    public float extendSpeed = 3.0f;     // 張り出す速さ

    private Vector3 originalPos;
    private bool isExtended = false;

    void Start()
    {
        originalPos = transform.position;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(player.position, transform.position);

        if (!isExtended && dist < triggerDistance)
        {
            isExtended = true;
        }

        // 張り出すアニメーション
        if (isExtended)
        {
            Vector3 target = originalPos + new Vector3(extendDistance, 0, 0);
            transform.position = Vector3.MoveTowards(transform.position, target, extendSpeed * Time.deltaTime);
        }
    }
}
