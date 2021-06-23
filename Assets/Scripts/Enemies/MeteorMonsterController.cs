using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;

public class MeteorMonsterController : MonsterWithCustomPath
{
    public EnemyGunController gun;


    private int curForm;
    private Animator animator;
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }
    public override void Setup()
    {
        base.Setup();
        transform.localScale = Vector3.one * 0.8f;
        animator.Play("meteor_monster_idle");
        curForm = 1;
        gun.SetupGun();
    }
    public override void Spawn()
    {
        base.Spawn();
        Move();
    }
    public override void OnMoveEnd()
    {
        bezierMoveController.Stop();
        //moveController.Stop();
        base.OnMoveEnd();

    }
    public override void Move()
    {
        base.Move();
        //Remember to unsubribe event before destroy something
        bezierMoveController.OnMoveEnd += Patrol;
        bezierMoveController.SetPath(intro);
        bezierMoveController.StartMove(LoopType.None);

        //moveController.movementEndEvent += Patrol;
        //moveController.pathContainer = introPath;
        //moveController.moveToPath = false;
        //moveController.loopType = splineMove.LoopType.none;
        //moveController.StartMove();


        currentMovementState = MovementState.INTRO;
    }
    public override void Patrol()
    {       
        base.Patrol();
        bezierMoveController.OnMoveEnd -= Patrol;
        bezierMoveController.SetPath(patrol);
        bezierMoveController.StartMove(LoopType.Loop);
        

        //moveController.movementEndEvent -= Patrol;
        //moveController.pathContainer = patrolPath;
        //moveController.moveToPath = false;
        //moveController.loopType = splineMove.LoopType.loop;
        //moveController.StartMove();

        currentMovementState = MovementState.PATROL;
    }

    private void TransformIntoAngryForm(int form)
    {
        FXManager.Instance.CreateFX(2, transform.position);
        if (form == 2)
        {
            transform.localScale = Vector3.one * 1f;
            animator.Play("meteor_monster_form2_idle");
        }
        else if(form == 3)
        {
            transform.localScale = Vector3.one * 1.3f;
            animator.Play("meteor_monster_form3_idle");
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
