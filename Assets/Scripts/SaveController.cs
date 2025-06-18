using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    //自分地震をステックかして顕現することで自分の中の変数などを他のシーンに持ち越す準備
    public static SaveController instance;

    //
    public HashSet<(string tag,int arrangeId)> 
        consumedEvent = new HashSet<(string tag,int arrangeId)>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; //プログラム自身の情報をstatic変数に格納
            DontDestroyOnLoad(gameObject); //sceneが切り替わってもオブジェクトをひきつぐ
        }
        else
        {
            //2つめ移行のsceneControllerであることがかくていする
            Destroy(gameObject); //ひとつのsceneで競合しないように自己破棄
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
