using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string nextSceneName = "Stage2"; // Inspector�őJ�ڐ�V�[�������w��

    private bool playerInRange = false;
    public string sceneName = "Stage1";


    void Update()
    {
        // �v���C���[���͈͓��ŏ�L�[���������̂ݑJ��
        if (playerInRange && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            SceneManager.LoadScene(nextSceneName); // �� Inspector�Ŏw��ł���I
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
