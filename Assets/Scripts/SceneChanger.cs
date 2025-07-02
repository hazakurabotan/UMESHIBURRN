using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string nextSceneName = "Stage2"; // Inspector�őJ�ڐ�V�[�������w��

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("LoadScene���s: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}