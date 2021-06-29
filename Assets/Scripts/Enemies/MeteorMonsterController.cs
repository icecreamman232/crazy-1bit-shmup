using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeteorMonsterController : MonsterWithCustomPath
{
    public GunDO gunData_Form2;
    public GunDO gunData_Form3;

    public EnemyGunController leftGun;
    public EnemyGunController rightGun;

    public RotateArmorController armorController;

    private float amountMovingRecoil;

    //Amount of moving back because of recoiling
    private float timeRecoilBack;
    //Amount of moving forward to back the previous position
    private float timeToBackPreviousPosition;
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
        rightGun.SetupGun();
        leftGun.SetupGun();
    }
    public override void Spawn()
    {
        base.Spawn();
        Move();
        
    }
    public override void OnMoveEnd()
    {
        bezierMoveController.Stop();

        base.OnMoveEnd();

    }
    public override void Move()
    {
        base.Move();
        //Remember to unsubribe event before destroy something
        bezierMoveController.OnMoveEnd += Patrol;
        bezierMoveController.SetPath(intro);
        bezierMoveController.Stop();
        bezierMoveController.StartMove(LoopType.None);

        currentMovementState = MovementState.INTRO;
    }
    public override void Patrol()
    {       
        base.Patrol();
        bezierMoveController.OnMoveEnd -= Patrol;
        bezierMoveController.moveToPath = true;
        bezierMoveController.SetPath(patrol);
        bezierMoveController.StartMove(LoopType.Loop);


        currentMovementState = MovementState.PATROL;
    }
    private void Update()
    {
        
    }
    private void TransformIntoAngryForm(int form)
    {
        FXManager.Instance.CreateFX(fxID.EXPLOSION_FOR_TRANSFORM, transform.position);
        if (form == 2)
        {

            baseHP = Mathf.RoundToInt(baseHP * 1.2f);
            maxHP = baseHP;
            currentHP = maxHP;
            UpdateHPInterface();
            baseImpactDamage += baseImpactDamage;
            transform.localScale = Vector3.one * 1f;
            animator.Play("meteor_monster_form2_idle");
            amountMovingRecoil = 0.4f;
            timeRecoilBack = 0.15f;
            timeToBackPreviousPosition = 0.25f;

        }
        else if(form == 3)
        {
            //Switch the gun
            leftGun.gunData = gunData_Form3;
            rightGun.gunData = gunData_Form3;
            //Reset HP
            baseHP = Mathf.RoundToInt(baseHP * 1.2f);
            maxHP = baseHP;
            currentHP = maxHP;
            UpdateHPInterface();
            //Increase dmg
            baseImpactDamage += baseImpactDamage;
            //Increase size
            transform.localScale = Vector3.one * 1.3f;

            animator.Play("meteor_monster_form3_idle");
            amountMovingRecoil = 0.3f;
            timeRecoilBack = 0.2f;
            timeToBackPreviousPosition = 0.3f;

            armorController.Active();

        }
        if(form==2)
        {
            //Start to shoot after transformed into 2nd form
            rightGun.Shoot();
            leftGun.Shoot();
            rightGun.StopMoveToShoot    += bezierMoveController.Pause;
            rightGun.Done1Shot          += ResumeMoving;
            rightGun.PlayRecoilAnimation += PlayRecoilAnimation;
            leftGun.StopMoveToShoot     += bezierMoveController.Pause;
            leftGun.Done1Shot           += ResumeMoving;
            leftGun.PlayRecoilAnimation += PlayRecoilAnimation;
        }
    }
    private void PlayRecoilAnimation()
    {
        LeanTween.moveLocalY(this.gameObject, transform.position.y + amountMovingRecoil, timeRecoilBack).
            setEase(LeanTweenType.easeOutExpo)
            .setOnComplete(
            () =>
            {
                LeanTween.moveLocalY(this.gameObject, transform.position.y - amountMovingRecoil, timeToBackPreviousPosition)
                .setOnComplete(
                    ()=>
                    {
                        ResetAnimation();
                    });
            });
    }
   
    private void ResetAnimation()
    {
        if (curForm == 2)
        {
            animator.Play("meteor_monster_form2_idle");

        }
        else if(curForm == 3)
        {
            animator.Play("meteor_monster_form3_idle");
        }
    }
    private void ResumeMoving()
    {
        StartCoroutine(WaitForShotWentOut());
    }
    private IEnumerator WaitForShotWentOut()
    {
        float time = timeRecoilBack + timeToBackPreviousPosition + 0.25f;
        yield return new WaitForSeconds(time/*0.35f*/);
        bezierMoveController.Resume();
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Environment"))
        {
            if (collision.GetComponent<BaseEnvironment>().id == 2)
            {              
                //if transformed then get hit and lose HP
                UpdateHP(collision.GetComponent<BaseEnvironment>().impactDamage);
                collision.GetComponent<BaseEnvironment>().currentHP -= baseImpactDamage;
                if (curForm < 3)
                {
                    curForm++;
                    //if meteor hit this monster, it would turn into angry one
                    TransformIntoAngryForm(curForm);
                }
            }          
        }
    }
}
