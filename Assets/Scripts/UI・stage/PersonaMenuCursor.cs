using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonaMenuCursor : MonoBehaviour
{
    public RectTransform cursor;   // �u�I���J�[�\���vImage��RectTransform
    public List<RectTransform> menuItems; // ���j���[����Image��RectTransform���X�g

    public float moveSpeed = 0.2f; // �J�[�\���ړ��̂Ȃ߂炩��
    public PersonaMenuDetail detailPanelScript;

    private int currentIndex = 0;

    void Start()
    {
        if (menuItems != null && menuItems.Count > 0 && cursor != null)
            cursor.anchoredPosition = menuItems[0].anchoredPosition;
        else
            Debug.LogWarning("PersonaMenuCursor: menuItems����ł��I");
    }

    void Update()
    {
        if (menuItems == null || menuItems.Count == 0 || cursor == null)
            return; // �����Ȃ���΃X���[

        // ���͎擾
        if (Input.GetKeyDown(KeyCode.RightArrow)) Next();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Prev();

        // �J�[�\���ړ��A�j��
        cursor.anchoredPosition = Vector2.Lerp(
            cursor.anchoredPosition,
            menuItems[currentIndex].anchoredPosition,
            moveSpeed
        );

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (detailPanelScript != null)
                detailPanelScript.ShowDetail(currentIndex); // ���ݑI�𒆂�index�ŏڍו\��
        }
    }

    void Next()
    {
        if (menuItems.Count == 0) return;
        currentIndex = (currentIndex + 1) % menuItems.Count;
    }

    void Prev()
    {
        if (menuItems.Count == 0) return;
        currentIndex = (currentIndex - 1 + menuItems.Count) % menuItems.Count;
    }
}
