using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public BossSimpleJump boss;
    public DialogManager dialogManager;
    public string[] dialogLines; // Inspector‚ÅƒZƒŠƒt“o˜^

    bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            boss.isActive = false;
            FindObjectOfType<PlayerController>().enabled = false;

            dialogManager.StartDialog(dialogLines);

            Destroy(gameObject); // ƒgƒŠƒK[Á‹
        }
    }
}
