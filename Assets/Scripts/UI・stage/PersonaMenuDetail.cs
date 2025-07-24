using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening; // DOTween�g���ꍇ

public class PersonaMenuDetail : MonoBehaviour
{
    [System.Serializable]
    public class MenuDetail
    {
        public Sprite detailImage;
        public string detailTitle;
        [TextArea(2, 5)]
        public string detailText;
    }

    public GameObject detailPanel;   // �㕔�̏ڍ׃p�l��
    public Image detailImage;
    public TextMeshProUGUI detailTitle;
    public TextMeshProUGUI detailText;
    public GameObject WeaponListPanel;
    public GameObject AccessoryListPanel;
    public GameObject SkillListPanel;
    public GameObject MissionListPanel;
    public GameObject OptionListPanel;
    public GameObject SaveListPanel;
    public GameObject TitleListPanel;

    public MenuDetail[] details;     // ���j���[���ڂ��Ƃ̏ڍ�

    private int currentMenuIndex = 0;


    void Update()
    {
        if (detailPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            HideDetail();
        }
    }

    // �J�[�\�����ǂ���I��ł��邩�A�܂���Z�L�[�����ꂽ��Ă΂��
    public void ShowDetail(int menuIndex)
    {
        if (menuIndex < 0 || menuIndex >= details.Length) return;
        var data = details[menuIndex];

        detailPanel.SetActive(true); // �\��
        detailImage.sprite = data.detailImage;
        detailTitle.text = data.detailTitle;
        detailText.text = data.detailText;

        WeaponListPanel.SetActive(menuIndex == 0);
        AccessoryListPanel.SetActive(menuIndex == 1);
        SkillListPanel.SetActive(menuIndex == 2);
        MissionListPanel.SetActive(menuIndex == 3);
        OptionListPanel.SetActive(menuIndex == 4);
        SaveListPanel.SetActive(menuIndex == 5);
        TitleListPanel.SetActive(menuIndex == 6);



        // DOTween�ł�����Ɠ����������Ƃ�
        detailPanel.transform.localScale = Vector3.one * 0.8f;
        detailPanel.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
    }

    // ���j���[����Ƃ��Ă�
    public void HideDetail()
    {
        detailPanel.SetActive(false);
    }
}
