using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellDamage : MonoBehaviour
{
    public int damage = 1; // 何ダメージ与えるか

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // PlayerController を取得してダメージを与える
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            // 弾自身は消える
            Destroy(gameObject);
        }
        // 壁や他のものに当たったら消す（オプション）
        else if (other.CompareTag("Ground") || other.CompareTag("Block"))
        {
            Destroy(gameObject);
        }
    }
}
