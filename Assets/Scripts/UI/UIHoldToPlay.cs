using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHoldToPlay : BaseUIController
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Finished()
    {
        gameObject.SetActive(false);
    }
    public override void Show()
    {
        gameObject.SetActive(true);
        animator.Play("holdtoplay_intro_anim");
        
    }
    public void OnCompleteShow()
    {
        Debug.Log("Show complete");
        isShow = true;
    }

    public override void Hide()
    {
        animator.Play("holdtoplay_outtro_anim");
        isShow = false;
    }
}
