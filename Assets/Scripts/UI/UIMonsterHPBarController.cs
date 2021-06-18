using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMonsterHPBarController : MonoBehaviour
{
    
    public GameObject hpBar;
    public void Setup()
    {           
        hpBar.transform.localScale = new Vector3(1, 0.5f, 1);
        //Turn of HP Bar, need to take hit before show the HP Bar
        gameObject.SetActive(false);
    }
    public void UpdateHPBar(float hpPercent)
    {
        var lastScale = hpBar.transform.localScale;
        lastScale.x = hpPercent;
        hpBar.transform.localScale = lastScale;
    }

}
