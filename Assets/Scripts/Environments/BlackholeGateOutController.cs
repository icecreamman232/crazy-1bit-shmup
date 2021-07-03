using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BlackholeGateOutController : EnvironmentWithCustomPath
{
    public bool isShowGizmo = true;
    public BlackholeGateInController gateIn;
    public Coroutine pushCoroutine;
    public System.Action StopCurrentFunction;
    public GameObject pointOut;


    SpaceShipController shipController;
    SpaceShipMovement shipMovement;
    Animator shipAnimator;

    private Coroutine pullingShipAtGateCoroutine;
    private Coroutine pushingShipAtGateCoroutine;

    private Coroutine pullingMonsterAtGateCoroutine;
    private Coroutine pushingMonsterAtGateCoroutine;

    public bool isProcessing;
    public bool isProcessingMonster;
    private void Start()
    {
        shipController = GameManager.Instance.spaceShip.GetComponent<SpaceShipController>();
        shipMovement = GameManager.Instance.spaceShip.GetComponent<SpaceShipMovement>();
        shipAnimator = GameManager.Instance.spaceShip.GetComponent<Animator>();
        isProcessing = false;
    }
    public void CurrentCoroutine()
    {
        if(pushCoroutine!=null)
        {
            StopCoroutine(pushCoroutine);
            pushCoroutine = null;
        }
    }
    public override void Setup()
    {
        base.Setup();
        StopCurrentFunction += gateIn.CurrentCoroutine;
    }
    public override void Spawn()
    {
        base.Spawn();
        Move();
    }
    public override void Move()
    {
        base.Move();
        bezierMoveController.OnMoveEnd -= OnMoveEnd;
        bezierMoveController.OnMoveEnd += OnMoveEnd;
        bezierMoveController.SetPath(intro);
        bezierMoveController.Stop();
        bezierMoveController.StartMove(LoopType.None);
    }
    #region Push / Pull with Gate In
    public void PushingOutMonster(GameObject monster,Vector3 originScale)
    {      
        LeanTween.scale(monster, originScale, 0.8f);
        pushCoroutine  = StartCoroutine(ScaleUpMonster(monster));
    }
    private IEnumerator ScaleUpMonster(GameObject monster)
    {
        float moveSpeed = 2.25f;
        Vector3 targetPos = pointOut.transform.position - transform.position;//GameManager.Instance.spaceShip.transform.position;
        float timer = 0;
        float xSigned;
        float ySigned;
        if (targetPos.x < 0)
        {
            xSigned = -1;
        }
        else
        {
            xSigned = 1;
        }
        if (targetPos.y < 0)
        {
            ySigned = -1;
        }
        else
        {
            ySigned = 1;
        }

        while (true)
        {

            if (monster.transform.position.x <= -GameHelper.HalfSizeOfCamera().x - 1f ||
                monster.transform.position.x >= GameHelper.HalfSizeOfCamera().x + 1f ||
                monster.transform.position.y <= -GameHelper.HalfSizeOfCamera().y - 1f ||
                monster.transform.position.y >= GameHelper.HalfSizeOfCamera().y + 1f
                )
            {              
                monster.GetComponent<BaseMonster>().currentHP = 0;
                isProcessingMonster = false;
                StopCurrentFunction?.Invoke();
                yield break;
            }

            monster.transform.position = Vector3.MoveTowards(
                monster.transform.position,
                 Vector3.Lerp(monster.transform.position, targetPos, 0.1f),
                moveSpeed * Time.deltaTime);

            targetPos.x += xSigned * 1.0f;
            targetPos.y += ySigned * 1.0f;


            timer += Time.deltaTime;
            yield return null;
        }
    }
    public void PushingOutShip(GameObject ship)
    {
        //StopCurrentFunction?.Invoke();
        pushCoroutine = StartCoroutine(ScaleUp(ship));
    }
    private IEnumerator ScaleUp(GameObject ship)
    {
        float moveSpeed = 4.0f;
        float scaleSpeed = 2.5f;
        while (true)
        {
            if (ship.transform.localScale.x >= 1.0f)
            {
                ship.transform.localScale = Vector3.one;

                //Return ship to its normal state
                shipAnimator.Play("ship_idle");
                shipController.currentStatus = ShipStatus.NORMAL;
                shipMovement.currentStatus = ShipStatus.NORMAL;
                shipController.BeginShoot();
                isProcessing = false;
                StopCurrentFunction?.Invoke();
                yield break;
            }
            ship.transform.position = Vector3.MoveTowards(
                ship.transform.position,
                transform.GetChild(0).position,
                moveSpeed * Time.deltaTime);
            ship.transform.localScale += scaleSpeed * Time.deltaTime * Vector3.one;
            yield return null;
        }
    }

    #endregion

    #region Push / Pull with Gate Out itself

    private IEnumerator PullingShipAtGate(GameObject ship)
    {
        float moveSpeed = 4.0f;
        float scaleSpeed = 2.5f;
        while (true)
        {
            ship.transform.position = Vector3.MoveTowards(ship.transform.position, transform.position, moveSpeed * Time.deltaTime);
            ship.transform.localScale -= scaleSpeed * Time.deltaTime * Vector3.one;
            if (ship.transform.localScale.x <= 0)
            {
                ship.transform.localScale = Vector3.zero;
                ship.transform.position = transform.position;
                pushingShipAtGateCoroutine = StartCoroutine(PushOutShipAtGate(ship));
                yield break;
            }          
            yield return null;
        }
    }
    private IEnumerator PushOutShipAtGate(GameObject ship)
    {
        float moveSpeed = 4.0f;
        float scaleSpeed = 2.5f;
        while (true)
        {
            if (ship.transform.localScale.x >= 1.0f)
            {
                ship.transform.localScale = Vector3.one;
                //Return ship to its normal state
                shipAnimator.Play("ship_idle");
                shipController.currentStatus = ShipStatus.NORMAL;
                shipMovement.currentStatus = ShipStatus.NORMAL;              
                shipController.BeginShoot();
                yield break;
            }
            ship.transform.position = Vector3.MoveTowards(
                ship.transform.position,
                pointOut.transform.position,
                moveSpeed * Time.deltaTime);
            ship.transform.localScale += scaleSpeed * Time.deltaTime * Vector3.one;
            yield return null;
        }
    }

    private IEnumerator PullingMonsterAtGate(GameObject monster)
    {
        float moveSpeed = 4.0f;
        float scaleSpeed = 2.5f;
        Vector3 originScale = monster.transform.localScale;
        while (true)
        {
            if (monster.transform.localScale.x <= 0)
            {
                monster.transform.localScale = Vector3.zero;
                monster.transform.position = transform.position;
                pushingMonsterAtGateCoroutine = StartCoroutine(MoveOutMonster(monster));
                yield break;
            }
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, transform.position, moveSpeed * Time.deltaTime);
            monster.transform.localScale -= scaleSpeed * Time.deltaTime * Vector3.one;
            yield return null;
        }

    }
    private IEnumerator MoveOutMonster(GameObject monster)
    {
        Vector3 target = pointOut.transform.position-transform.position;
        float moveSpeed = 2.25f;
        float scaleSpd = 1.0f;
        float xSigned;
        float ySigned;
        if (target.x < 0)   { xSigned = -1;}
        else                { xSigned = 1; }
        if (target.y < 0)   { ySigned = -1;}
        else                { ySigned = 1; }
        while (true)
        {
            if (monster.transform.position.x <= -GameHelper.HalfSizeOfCamera().x - 1f ||
                monster.transform.position.x >= GameHelper.HalfSizeOfCamera().x + 1f ||
                monster.transform.position.y <= -GameHelper.HalfSizeOfCamera().y - 1f ||
                monster.transform.position.y >= GameHelper.HalfSizeOfCamera().y + 1f
                )
            {
                monster.GetComponent<BaseMonster>().currentHP = 0;
                yield break;
            }
            if (monster.transform.localScale.x >= 1f)
            {
                monster.transform.localScale = Vector3.one;
            }
            monster.transform.position = 
                Vector3.MoveTowards(monster.transform.position, Vector3.Lerp(monster.transform.position, target, 0.1f), moveSpeed * Time.deltaTime);
            if (monster.transform.localScale.x >= 1f)
            {
                monster.transform.localScale = Vector3.one;
            }
            else
            {
                monster.transform.localScale += scaleSpd * Time.deltaTime * Vector3.one;
            }  
            target.x += xSigned * 1.0f;
            target.y += ySigned * 1.0f;
            yield return null;
        }
    }
    #endregion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(GameHelper.IsInsideScreenBounds(collision.gameObject.transform.position))
        {
            if (!isProcessing)
            {
                //Layer 9 = Player
                if (collision.gameObject.layer == 9)
                {
                    if (collision.gameObject.CompareTag("Player"))
                    {
                        //ResetCoroutine();
                        shipAnimator.Play("ship_rotate");
                        shipController.currentStatus = ShipStatus.DISABLE;
                        shipMovement.currentStatus = ShipStatus.DISABLE;
                        shipController.StopShoot();
                        pullingShipAtGateCoroutine = StartCoroutine(PullingShipAtGate(collision.gameObject));
                    }
                }
            }
            //Layer 8 = Enemy
            if (collision.gameObject.layer == 8)
            {
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    if(!collision.GetComponent<BaseMonster>().isInteracting)
                    {
                        collision.GetComponent<BezierMoveController>().Pause();
                        pullingMonsterAtGateCoroutine = StartCoroutine(PullingMonsterAtGate(collision.gameObject));
                    }                  
                }
            }
          
        } 
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (isShowGizmo)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;
            var labelPos = transform.GetChild(0).position;
            labelPos.y += 1f;
            Handles.Label(labelPos, "Way Out",style);
            Gizmos.color = new Color(1.0f, 0, 0, 1.0f);
            Gizmos.DrawSphere(transform.position, 0.2f);

            Gizmos.color = Color.yellow;
            
            Gizmos.DrawCube(transform.GetChild(0).position, 0.3f * Vector3.one);
            Gizmos.DrawLine(transform.position, transform.GetChild(0).position);
        }
    }
#endif
}
