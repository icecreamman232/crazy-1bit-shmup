using System.Collections;
using UnityEngine;

public class BlackholeGateInController : EnvironmentWithCustomPath
{
    public bool isShowGizmo = true;
    public BlackholeGateOutController gateOut;
    public float moveSpeed;
    public Coroutine pullingCoroutine;

    public System.Action StopCurrentFunction;

    #region Reference

    private SpaceShipController shipController;
    private SpaceShipMovement shipMovement;
    private Animator shipAnimator;

    #endregion Reference

    private void Start()
    {
        shipController = GameManager.Instance.spaceShip.GetComponent<SpaceShipController>();
        shipMovement = GameManager.Instance.spaceShip.GetComponent<SpaceShipMovement>();
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

    private void PullingShip(GameObject ship)
    {
        gateOut.isProcessing = true;
        shipAnimator.Play("ship_rotate");
        StopCurrentFunction?.Invoke();
        ship.transform.rotation = Quaternion.identity;
        pullingCoroutine = StartCoroutine(OnPullingShip(ship));
    }

    private IEnumerator OnPullingShip(GameObject ship)
    {
        float moveSpeed = 4.0f;
        float scaleSpeed = 2.5f;
        while (true)
        {
            if (ship.transform.localScale.x <= 0)
            {
                ship.transform.localScale = Vector3.zero;
                ship.transform.position = gateOut.gameObject.transform.position;
                gateOut.PushingOutShip(ship);
                yield break;
            }
            ship.transform.position = Vector3.MoveTowards(ship.transform.position, transform.position, moveSpeed * Time.deltaTime);
            ship.transform.localScale -= scaleSpeed * Time.deltaTime * Vector3.one;
            yield return null;
        }
    }

    private void PullingMonster(GameObject monster)
    {
        StopCurrentFunction?.Invoke();
        pullingCoroutine = StartCoroutine(OnPullingMonster(monster));
    }

    private IEnumerator OnPullingMonster(GameObject monster)
    {
        float moveSpeed = 4.0f;
        float scaleSpeed = 2.5f;
        Vector3 originScale = monster.transform.localScale;
        while (true)
        {
            if (monster.transform.localScale.x <= 0)
            {
                monster.transform.localScale = Vector3.zero;
                monster.transform.position = gateOut.gameObject.transform.position;
                gateOut.PushingOutMonster(monster, originScale);
                yield break;
            }
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, transform.position, moveSpeed * Time.deltaTime);

            monster.transform.localScale -= scaleSpeed * Time.deltaTime * Vector3.one;
            yield return null;
        }
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
                    shipMovement.currentStatus = ShipStatus.DISABLE;
                    shipController.StopShoot();
                    gateOut.isProcessing = true;
                    PullingShip(collision.gameObject);
                }
                if (collision.CompareTag("Enemy"))
                {
                    collision.GetComponent<BaseMonster>().isInteracting = true;
                    collision.GetComponent<BezierMoveController>().Pause();
                    gateOut.isProcessingMonster = true;
                    PullingMonster(collision.gameObject);
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