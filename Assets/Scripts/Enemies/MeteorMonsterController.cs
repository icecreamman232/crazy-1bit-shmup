using System.Collections;
using UnityEngine;

public class MeteorMonsterController : MonsterWithCustomPath
{
    public GunDO gunData_Form2;
    public GunDO gunData_Form3;

    public MeteorMonsterRocketGun LGun;
    public MeteorMonsterRocketGun RGun;

    public RotateArmorController armorController;

    private float amountMovingRecoil;

    //Amount of moving back because of recoiling
    private float timeRecoilBack;

    //Amount of moving forward to back the previous position
    private float timeToBackPreviousPosition;

    private int curForm;

    public int CurForm
    {
        get
        {
            return curForm;
        }
    }

    private Animator animator;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        
    }
    private void OnDisable()
    {
  
    }
    private void Start()
    {
        
    }
    public override void Setup()
    {
        base.Setup();
        transform.localScale = Vector3.one * 0.8f;
        animator.Play("meteor_monster_idle");
        curForm = 1;
        LGun.SetupGun();
        RGun.SetupGun();

        //Setup shield
        for (int i = 0; i < armorController.armorList.Count; i++)
        {
            armorController.armorList[i].transform.localPosition = Vector3.zero;
            Color color = armorController.armorList[i].sprite.color;
            color.a = 0f;
            armorController.armorList[i].sprite.color = color;
        }

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
        else if (form == 3)
        {
            animator.Play("meteor_monster_form3_idle");
            //Switch the gun
            LGun.gunData = gunData_Form3;
            RGun.gunData = gunData_Form3;
            //Reset HP
            baseHP = Mathf.RoundToInt(baseHP * 1.2f);
            maxHP = baseHP;
            currentHP = maxHP;
            UpdateHPInterface();
            //Increase dmg
            baseImpactDamage += baseImpactDamage;
            //Increase size
            transform.localScale = Vector3.one * 1.3f;

            amountMovingRecoil = 0.3f;
            timeRecoilBack = 0.2f;
            timeToBackPreviousPosition = 0.3f;

            ActiveShield();
        }
        if (form == 2)
        {
            //Start to shoot after transformed into 2nd form
            RGun.Shoot();
            RGun.StopMoveToShoot += bezierMoveController.Pause;
            RGun.Done1Shot += ResumeMoving;
            RGun.PlayRecoilAnimation += PlayRecoilAnimation;

            LGun.Shoot();
            LGun.StopMoveToShoot += bezierMoveController.Pause;
            LGun.Done1Shot += ResumeMoving;
            LGun.PlayRecoilAnimation += PlayRecoilAnimation;
        }
    }

    public void ActiveShield()
    {
        for (int i = 0; i < armorController.armorList.Count; i++)
        {
            armorController.armorList[i].gameObject.SetActive(true);
        }

        Vector3 right = new Vector3(1.3f, 0.8f, 0f);
        Vector3 bot = new Vector3(0f, -1.5f, 0f);
        Vector3 left = new Vector3(-1.3f, 0.8f, 0f);

        Color rightColor = armorController.armorList[0].sprite.color;
        rightColor.a = 1f;
        Color botColor = armorController.armorList[1].sprite.color;
        botColor.a = 1f;
        Color leftColor = armorController.armorList[2].sprite.color;
        leftColor.a = 1f;

        LeanTween.moveLocal(armorController.armorList[0].gameObject, right, 1f)
            .setEase(LeanTweenType.easeOutCirc);
        LeanTween.moveLocal(armorController.armorList[1].gameObject, bot, 1f)
            .setEase(LeanTweenType.easeOutCirc)
            .setOnComplete(armorController.Active);
        LeanTween.moveLocal(armorController.armorList[2].gameObject, left, 1f)
            .setEase(LeanTweenType.easeOutCirc);

        LeanTween.color(armorController.armorList[0].gameObject, rightColor, 1f)
            .setEase(LeanTweenType.easeOutCirc);
        LeanTween.color(armorController.armorList[1].gameObject, botColor, 1f)
            .setEase(LeanTweenType.easeOutCirc);
        LeanTween.color(armorController.armorList[2].gameObject, leftColor, 1f)
            .setEase(LeanTweenType.easeOutCirc);
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
                    () =>
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
        else if (curForm == 3)
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
        if (GameHelper.IsInsideScreenBounds(transform.position))
        {
            if (collision.gameObject.CompareTag("Meteor"))
            {
                collision.GetComponent<BaseEnvironment>().currentHP -= baseImpactDamage;
                if (curForm < 3)
                {
                    curForm++;
                    TransformIntoAngryForm(curForm);
                }
            }
        }
    }
}