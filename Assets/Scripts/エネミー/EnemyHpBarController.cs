using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBarController : MonoBehaviour
{
    public Image fillImage; // HP�o�[�pImage�iType: Filled�����j

    // HP���X�V
    public void SetHp(int currentHp, int maxHp)
    {
        float ratio = Mathf.Clamp01((float)currentHp / maxHp);
        fillImage.fillAmount = ratio;
    }
}
