using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    public string sceneToLoad = "Shop"; // 行きたいシーン名をInspectorで指定

    private bool isPlayerInTrigger = false;

    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.UpArrow))
        {
            // シーン遷移
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }
}
