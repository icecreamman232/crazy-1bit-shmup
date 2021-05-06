using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBarController : MonoBehaviour
{
    public List<Animator> list_hp_bar_animator;
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HealthBarSetup()
    {
        index = list_hp_bar_animator.Count - 1;
        StartCoroutine(play_anim());
    }
    public void UpdateHealthBarUI()
    {
        list_hp_bar_animator[index].Play("hp_bar_explode");
        index--;
    }
    IEnumerator play_anim()
    {
        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.1f);
            list_hp_bar_animator[i].Play("hp_bar_idle");
        }
    }
}
