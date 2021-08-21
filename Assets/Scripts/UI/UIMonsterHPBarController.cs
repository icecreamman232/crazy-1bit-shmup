using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMonsterHPBarController : BaseUIController
{
    
    public GameObject hpBar;
    public void Setup()
    {
        hpBar.transform.localScale = new Vector3(1, 0.5f, 1);
        //Turn of HP Bar, need to take hit before show the HP Bar
        Hide();

    }
    public void UpdateHPBar(float hpPercent)
    {
        var lastScale = hpBar.transform.localScale;
        lastScale.x = hpPercent;
        hpBar.transform.localScale = lastScale;
    }

    public override void Show()
    {
        isShow = true;
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        isShow = false;
        gameObject.SetActive(false);
    }
}
