using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string sceneName = "Stage1"; // ← わかりやすくシーン名と命名！

    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
