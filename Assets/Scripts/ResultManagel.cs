using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        // GameManager �� static �� totalScore ���Q�Ƃ��ĕ\��
        scoreText.text = GameManager.totalScore.ToString();
        Debug.Log("���U���g��ʂɕ\�������X�R�A: " + GameManager.totalScore);
    }
}
