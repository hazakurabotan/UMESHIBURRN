using UnityEngine;
using UnityEngine.UI;

// �v���C���[�������Ă���A�C�e���ꗗ��UI�ŕ\���E�I������N���X
public class ItemDisplayManager : MonoBehaviour
{
    public Sprite[] itemSprites;        // �A�C�e���摜���X�g�i��: 0=�����S, 1=�|�[�V�����c�j
    public RectTransform parent;        // ���ׂ�e�I�u�W�F�N�g�iUI��Panel�Ȃǂ��w��j
    public GameObject itemImagePrefab;  // �A�C�e���摜�p�̃v���n�u�iImage�����Ă�Prefab�j

    int selectIndex = 0;                // ���ݑI�𒆂̃C���f�b�N�X
    GameObject[] displayedItems;        // ��ʏ�ɐ������ꂽ�A�C�e���I�u�W�F�N�g�z��

    void Start()
    {
        // �v���C���[���擾�����A�C�e�������擾
        int count = PlayerInventory.obtainedItems.Count;
        displayedItems = new GameObject[count];

        // �A�C�e�����Ƃɉ摜����ׂĐ���
        for (int i = 0; i < count; i++)
        {
            int itemId = PlayerInventory.obtainedItems[i];   // �A�C�e��ID�i0,1,...�j

            // Image�v���n�u��e�p�l���̎q�Ƃ��Đ���
            GameObject go = Instantiate(itemImagePrefab, parent);

            // �f�o�b�O�F�ʒu��T�C�Y���m�F�������ꍇ
            RectTransform rt = go.GetComponent<RectTransform>();
            Debug.Log($"��������Image: {go.name}, anchoredPos: {rt.anchoredPosition}, size: {rt.sizeDelta}, scale: {rt.localScale}");

            // �摜���A�C�e��ID�ɉ����č����ւ�
            go.GetComponent<Image>().sprite = itemSprites[itemId];

            // ���ɕ��ׂ�i�����ł�100�s�N�Z�����Y������j
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(60 + 100 * i, 0);

            displayedItems[i] = go; // �z��ɓo�^
        }

        UpdateHighlight(); // �ŏ��̑I����Ԃ𔽉f
    }

    void Update()
    {
        // �����\�����Ă��Ȃ��ꍇ�͉������Ȃ�
        if (displayedItems == null || displayedItems.Length == 0) return;

        // �E�L�[�Ŏ��̃A�C�e���A���L�[�őO�̃A�C�e��
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectIndex = (selectIndex + 1) % displayedItems.Length;
            UpdateHighlight();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectIndex = (selectIndex - 1 + displayedItems.Length) % displayedItems.Length;
            UpdateHighlight();
        }

        // Z�L�[�ŃA�C�e���g�p�i��FID=1���񕜃A�C�e���j
        if (Input.GetKeyDown(KeyCode.Z))
        {
            int itemId = PlayerInventory.obtainedItems[selectIndex];
            if (itemId == 1)
            {
                // �v���C���[��T���ĉ񕜏���
                var player = FindObjectOfType<PlayerController>();
                if (player != null)
                {
                    player.Heal(10); // �̗͂�10��
                    Debug.Log("�̗͉񕜃A�C�e�����g�����I");
                }
            }
            else
            {
                Debug.Log("���̃A�C�e���͎g���܂���I");
            }
        }
    }

    // �I�𒆃A�C�e�������傫���\���i�����j
    void UpdateHighlight()
    {
        for (int i = 0; i < displayedItems.Length; i++)
        {
            displayedItems[i].transform.localScale = (i == selectIndex) ? Vector3.one * 1.2f : Vector3.one;
        }
    }
}
