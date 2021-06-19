using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;

public class MeteorMonsterController : MonsterWithCustomPath
{
    public EnemyGunController gun;


    private int curForm;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public override void Setup()
    {
        base.Setup();
        curForm = 1;
        gun.SetupGun();
    }
    public override void Spawn()
    {
        base.Spawn();
        Move();
    }
    public override void Move()
    {
        base.Move();
        //Remember to unsubribe event before destroy something
        moveController.movementEndEvent -= OnMoveEnd;
        moveController.movementEndEvent += OnMoveEnd;
        moveController.pathContainer = introPath;
        moveController.moveToPath = false;
        moveController.loopType = splineMove.LoopType.none;
        moveController.StartMove();
        currentMovementState = MovementState.INTRO;
    }
    private void TransformIntoAngryForm(int form)
    {
        FXManager.Instance.CreateFX(2, transform.position);
        if (form == 2)
        {
            animator.Play("meteor_monster_form2_idle");
        }
        else if(form == 3)
        {
            animator.Play("meteor_monster_form2_idle");
        }
        gun.Shoot();
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Environment"))
        {
            if (collision.GetComponent<BaseEnvironment>().id == 2)
            {
                if(curForm < 4)
                {
                    curForm++;
                    //if meteor hit this monster, it would turn into angry one
                    TransformIntoAngryForm(curForm);
                    
                }
                else
                {
                    //if transformed then get hit and lose HP
                    UpdateHP(collision.GetComponent<BaseEnvironment>().impactDamage);
                    if (!gameObject.transform.GetChild(0).gameObject.activeSelf)
                    {
                        gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
            }          
        }
    }
}
