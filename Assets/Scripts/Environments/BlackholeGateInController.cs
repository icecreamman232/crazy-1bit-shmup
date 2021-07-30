using System.Collections;
using UnityEngine;
using System;

public class BlackholeGateInController : EnvironmentWithCustomPath
{
    public bool isShowGizmo = true;
    public BlackholeGateOutController gateOut;
    public float moveSpeed;
    public Coroutine pullingCoroutine;

    public System.Action StopCurrentFunction;

    #region Reference

    private SpaceShipController shipController;
    
    private Animator shipAnimator;


    //New fields here
    public Action OnFinishedPulling;



    #endregion Reference

    private void Start()
    {
        shipController = GameManager.Instance.spaceShip.GetComponent<SpaceShipController>();
        shipAnimator = GameManager.Instance.spaceShip.GetComponent<Animator>();
    }

    public void CurrentCoroutine()
    {
        if (pullingCoroutine != null)
        {
            StopCoroutine(pullingCoroutine);
            pullingCoroutine = null;
        }
    }

    public override void Setup()
    {
        base.Setup();
        StopCurrentFunction += gateOut.CurrentCoroutine;
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

    #region Handling pulling behaviour
    private void PullingForShip(GameObject ship)
    {
        gateOut.isProcessing = true;
        LeanTween.rotateAroundLocal(ship, Vector3.forward, 360f, 0.3f);
        LeanTween.scale(ship, Vector3.zero, 0.3f);
        LeanTween.move(ship, transform.position, 0.4f)
            .setOnComplete(()=> {
                ship.transform.rotation = Quaternion.identity;
                ship.transform.localScale = Vector3.zero;
                ship.transform.position = gateOut.gameObject.transform.position;
                gateOut.PushingForShip(ship);
            });

    }
    private void PullingForMonster(GameObject monster, Vector3 originScale)
    {
        gateOut.isProcessing = true;
        LeanTween.rotateAroundLocal(monster, Vector3.forward, 360f, 0.2f);
        LeanTween.scale(monster, Vector3.zero, 0.2f);
        LeanTween.move(monster, transform.position, 0.3f)
            .setOnComplete(() => {
                monster.transform.rotation = Quaternion.identity;
                monster.transform.localScale = Vector3.zero;
                monster.transform.position = gateOut.gameObject.transform.position;
                gateOut.PushingForMonster(monster, originScale);
            });
    }

    #endregion Handling pulling behaviour

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameHelper.IsInsideScreenBounds(gateOut.gameObject.transform.position))
        {
            if (GameHelper.IsInsideScreenBounds(transform.position))
            {

                if (collision.CompareTag("Player"))
                {
                    shipController.currentStatus = ShipStatus.DISABLE;
                    shipController.StopShoot();
                    gateOut.isProcessing = true;
                    PullingForShip(collision.gameObject);
                }
                if(!gateOut.isProcessingMonster)
                {
                    if (collision.CompareTag("Enemy"))
                    {
                        collision.GetComponent<BaseMonster>().isInteracting = true;
                        collision.GetComponent<BezierMoveController>().Pause();
                        gateOut.isProcessingMonster = true;
                        PullingForMonster(collision.gameObject, collision.gameObject.transform.localScale);
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
            Gizmos.color = new Color(0, 1.0f, 0, 1.0f);
            Gizmos.DrawSphere(transform.position, 0.2f);
        }
    }

#endif
}