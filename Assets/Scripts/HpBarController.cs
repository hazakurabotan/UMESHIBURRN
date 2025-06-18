using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // ©‚±‚ê‚ð–Y‚ê‚¸‚ÉI


public class HpBarController : MonoBehaviour
{
    public Image lanternImage;
    public Sprite fullFire;
    public Sprite middleFire;
    public Sprite smallFire;
    public Sprite noFire;

    public void SetHp(int hp, int maxHp = 3)
    {
        float ratio = (float)hp / maxHp;
        if (ratio >= 0.7f)
            lanternImage.sprite = fullFire;
        else if (ratio >= 0.4f)
            lanternImage.sprite = middleFire;
        else if (ratio >= 0.1f)
            lanternImage.sprite = smallFire;
        else
            lanternImage.sprite = noFire;
    }
}
