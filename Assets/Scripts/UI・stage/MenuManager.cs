using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;    // MenuPanel�iInspector�Ŏw��j
    public PersonaMenuDetail detail; // PersonaMenuDetail�X�N���v�g�iInspector�Ŏw��j

    public enum MenuState { Main, Detail }
    private MenuState currentState = MenuState.Main;

    private bool isMenuOpen = false;
    private int currentMenuIndex = 0; // �J�[�\���I��Index�i�K�v�ɉ����āj

    void Start()
    {
        menuPanel.SetActive(false); // �Q�[���J�n���͔�\��
        if (detail != null) detail.HideDetail(); // �ڍ׃p�l������\���ŊJ�n
    }

    void Update()
    {
        // ���j���[���̂̊J�iSpace�j
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isMenuOpen)
            {
                OpenMenu();
            }
            else if (currentState == MenuState.Main)
            {
                CloseMenu();
            }
            // �ڍו\�����iDetail�j��ESC�ŕ���̂�Space�͖���
        }

        if (!isMenuOpen) return; // ���j���[��\�����͂����őł��؂�

        // --- ���C�����j���[���쎞 ---
        if (currentState == MenuState.Main)
        {
            // �J�[�\�����E�ړ��i�����͏ȗ� or �ʃX�N���v�g�ɈϏ��j
            // if (Input.GetKeyDown(KeyCode.RightArrow)) currentMenuIndex++;
            // if (Input.GetKeyDown(KeyCode.LeftArrow))  currentMenuIndex--;

            // Z�ŏڍׂ��J��
            if (Input.GetKeyDown(KeyCode.Z))
            {
                detail.ShowDetail(currentMenuIndex);
                currentState = MenuState.Detail;
            }
        }
        // --- �ڍו\���� ---
        else if (currentState == MenuState.Detail)
        {
            // ESC�ŏڍׂ���ă��j���[�ɖ߂�
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                detail.HideDetail();
                currentState = MenuState.Main;
            }
        }
    }

    void OpenMenu()
    {
        menuPanel.SetActive(true);
        isMenuOpen = true;
        Time.timeScale = 0f; // �Q�[���S�̂��~
    }

    void CloseMenu()
    {
        if (detail != null) detail.HideDetail();
        menuPanel.SetActive(false);
        isMenuOpen = false;
        currentState = MenuState.Main;
        Time.timeScale = 1f; // �Q�[���ĊJ
    }
}
