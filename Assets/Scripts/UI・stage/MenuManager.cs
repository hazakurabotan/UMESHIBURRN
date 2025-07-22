using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;    // MenuPanel（Inspectorで指定）
    public PersonaMenuDetail detail; // PersonaMenuDetailスクリプト（Inspectorで指定）

    public enum MenuState { Main, Detail }
    private MenuState currentState = MenuState.Main;

    private bool isMenuOpen = false;
    private int currentMenuIndex = 0; // カーソル選択中Index（必要に応じて）

    void Start()
    {
        menuPanel.SetActive(false); // ゲーム開始時は非表示
        if (detail != null) detail.HideDetail(); // 詳細パネルも非表示で開始
    }

    void Update()
    {
        // メニュー自体の開閉（Space）
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
            // 詳細表示中（Detail）はESCで閉じるのでSpaceは無効
        }

        if (!isMenuOpen) return; // メニュー非表示時はここで打ち切り

        // --- メインメニュー操作時 ---
        if (currentState == MenuState.Main)
        {
            // カーソル左右移動（ここは省略 or 別スクリプトに委譲）
            // if (Input.GetKeyDown(KeyCode.RightArrow)) currentMenuIndex++;
            // if (Input.GetKeyDown(KeyCode.LeftArrow))  currentMenuIndex--;

            // Zで詳細を開く
            if (Input.GetKeyDown(KeyCode.Z))
            {
                detail.ShowDetail(currentMenuIndex);
                currentState = MenuState.Detail;
            }
        }
        // --- 詳細表示時 ---
        else if (currentState == MenuState.Detail)
        {
            // ESCで詳細を閉じてメニューに戻る
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
        Time.timeScale = 0f; // ゲーム全体を停止
    }

    void CloseMenu()
    {
        if (detail != null) detail.HideDetail();
        menuPanel.SetActive(false);
        isMenuOpen = false;
        currentState = MenuState.Main;
        Time.timeScale = 1f; // ゲーム再開
    }
}
