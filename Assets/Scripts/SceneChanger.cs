using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string nextSceneName = "Stage2"; // Inspectorで遷移先シーン名を指定

    private bool playerInRange = false;
    public string sceneName = "Stage1";


    void Update()
    {
        // プレイヤーが範囲内で上キー押した時のみ遷移
        if (playerInRange && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            SceneManager.LoadScene(nextSceneName); // ← Inspectorで指定できる！
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

}
