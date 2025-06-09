using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public Transform defaultSpawn;         // 通常開始位置
    public Transform shopSpawnPoint;       // 店から戻った時の位置

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            if (SceneTransitionInfo.cameFromShop && shopSpawnPoint != null)
            {
                player.transform.position = shopSpawnPoint.position;
                SceneTransitionInfo.cameFromShop = false; // 一度使ったらリセット
            }
            else if (defaultSpawn != null)
            {
                player.transform.position = defaultSpawn.position;
            }
        }
    }
}
