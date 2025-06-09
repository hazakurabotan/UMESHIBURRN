using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public Transform defaultSpawn;         // �ʏ�J�n�ʒu
    public Transform shopSpawnPoint;       // �X����߂������̈ʒu

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            if (SceneTransitionInfo.cameFromShop && shopSpawnPoint != null)
            {
                player.transform.position = shopSpawnPoint.position;
                SceneTransitionInfo.cameFromShop = false; // ��x�g�����烊�Z�b�g
            }
            else if (defaultSpawn != null)
            {
                player.transform.position = defaultSpawn.position;
            }
        }
    }
}
