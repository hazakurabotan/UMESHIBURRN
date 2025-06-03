using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAction : MonoBehaviour
{
    public GameObject targetMoveBlock;
    public Sprite imageOn;
    public Sprite imageOff;
    public bool on = false; //�X�C�b�`�̏�ԁitrue�F������Ă���@false�F������Ă��Ȃ�)

    // Start is called before the first frame update
    void Start()
    {
        if (on)
        {
            GetComponent<SpriteRenderer>().sprite = imageOn;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = imageOff;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //�ڐG�J�n
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (on)
            {
                on = false;
                GetComponent<SpriteRenderer>().sprite = imageOff;
                MovingBloc movingBloc = targetMoveBlock.GetComponent<MovingBloc>();
                movingBloc.Stop();
            }
            else
            {
                on = true;
                GetComponent<SpriteRenderer>().sprite = imageOn;
                MovingBloc movingBloc = targetMoveBlock.GetComponent<MovingBloc>();
                movingBloc.Move();
            }

        }
    }
}

