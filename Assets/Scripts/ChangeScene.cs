using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string sceneName = "Stage1"; // Å© ÇÌÇ©ÇËÇ‚Ç∑Ç≠ÉVÅ[ÉìñºÇ∆ñΩñºÅI

    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
