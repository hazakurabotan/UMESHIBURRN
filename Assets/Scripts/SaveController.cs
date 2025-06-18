using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    //�����n�k���X�e�b�N�����Č������邱�ƂŎ����̒��̕ϐ��Ȃǂ𑼂̃V�[���Ɏ����z������
    public static SaveController instance;

    //
    public HashSet<(string tag,int arrangeId)> 
        consumedEvent = new HashSet<(string tag,int arrangeId)>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; //�v���O�������g�̏���static�ϐ��Ɋi�[
            DontDestroyOnLoad(gameObject); //scene���؂�ւ���Ă��I�u�W�F�N�g���Ђ���
        }
        else
        {
            //2�߈ڍs��sceneController�ł��邱�Ƃ������Ă�����
            Destroy(gameObject); //�ЂƂ�scene�ŋ������Ȃ��悤�Ɏ��Ȕj��
        }


    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ConsumedEvent(string tag,int arrangeId)
    {
        consumedEvent.Add((tag,arrangeId));
    }

    bool IsConsumed(string tag, int arrangeId)

    {
        return consumedEvent.Contains((tag, arrangeId));
    }

}
