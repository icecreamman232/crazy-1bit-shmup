using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBarController : BaseUIController
{
    public List<Animator> list_hp_bar_animator;
    private int index;

    private void UpdateHealthBarUI()
    {
        list_hp_bar_animator[index].Play("hp_bar_explode");
        index--;
        if(index < 0)
        {
            index = 0;
        }
    }
    IEnumerator play_anim()
    {
        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.1f);
            list_hp_bar_animator[i].Play("hp_bar_idle");
        }
    }

    public override void Show()
    {      
        GameManager.Instance.spaceShip.TakeDamageAction += UpdateHealthBarUI;
        index = list_hp_bar_animator.Count - 1;
        StartCoroutine(play_anim());
        isShow = true;
    }

    public override void Hide()
    {
        
        GameManager.Instance.spaceShip.TakeDamageAction -= UpdateHealthBarUI;
        isShow = false;
    }
}
