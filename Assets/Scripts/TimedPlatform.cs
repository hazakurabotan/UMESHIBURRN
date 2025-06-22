using UnityEngine;

public class TimedPlatform : MonoBehaviour
{
    public float visibleTime = 2f;   // �\�������b��
    public float invisibleTime = 1f; // �����Ă�b��

    private float timer = 0f;
    private bool isVisible = true;
    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        SetVisible(true); // �ŏ��͕\�����
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isVisible && timer >= visibleTime)
        {
            SetVisible(false);
            timer = 0f;
        }
        else if (!isVisible && timer >= invisibleTime)
        {
            SetVisible(true);
            timer = 0f;
        }
    }

    void SetVisible(bool show)
    {
        isVisible = show;
        sr.enabled = show;
        if (col != null) col.enabled = show;
    }
}
