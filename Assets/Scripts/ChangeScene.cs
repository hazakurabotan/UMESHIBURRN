using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string sceneName = "Stage1"; // �� �킩��₷���V�[�����Ɩ����I

    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
