using UnityEngine;

public class AnimatorTestSwitcher : MonoBehaviour
{
    private Animator animator; // ← publicじゃなくてOK

    public AnimatorOverrideController revivedOverrideController; // これはInspectorに出る

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.runtimeAnimatorController = null;
            animator.runtimeAnimatorController = revivedOverrideController;
            animator.Play("RePlayerStop", 0, 0); // レイヤー0、先頭から再生
            Debug.Log("切り替えテスト発動！");
        }
    }
}