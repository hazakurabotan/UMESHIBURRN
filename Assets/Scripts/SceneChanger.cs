using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string nextSceneName = "Stage2"; // Inspectorで遷移先シーン名を指定

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("LoadScene実行: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}