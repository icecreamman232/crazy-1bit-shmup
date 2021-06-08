using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;

 public enum WormHoleType
{
    None       = 0,
    GateIn      = 1,
    GateOut   = 2,
}

public class WormHoleController : EnvironmentWithCustomPath
{
    public bool isShowGizmo = true;
    public float moveSpeed;
    public WormHoleType wormholeType;
    public Transform gateOutTransform;

    public override void Setup()
    {
        base.Setup();
    }
    public override void Spawn()
    {
        base.Spawn();
        Move();
    }

    public override void Move()
    {
        base.Move();
        moveController.movementEndEvent -= OnMoveEnd;
        moveController.movementEndEvent += OnMoveEnd;
        moveController.pathContainer = introPath;
        moveController.loopType = splineMove.LoopType.none;
        moveController.moveToPath = false;
        moveController.StartMove();
    }

    private void DoVancumn(GameObject ship)
    {
        ship.GetComponent<Animator>().Play("ship_rotate");
        StartCoroutine(OnVancumning(ship));
    }
    private IEnumerator OnVancumning(GameObject ship)
    {
        float scaleSpeed = 2.5f;
        while(true)
        {
            if(ship.transform.localScale.x <= 0)
            {
                ship.GetComponent<Animator>().Play("ship_idle");
                ship.transform.localScale = Vector3.zero;
                ship.transform.position = gateOutTransform.position;
                LaunchOut(ship);
                yield break;
            }
            ship.transform.position = transform.position;
            ship.transform.localScale -= Vector3.one * scaleSpeed * Time.deltaTime;
            yield return null;
        }
    }
    private void LaunchOut(GameObject ship)
    {
        StartCoroutine(ScaleUp(ship));
    }
    private IEnumerator ScaleUp(GameObject ship)
    {
        var normalizeVector = gateOutTransform.position.normalized;
        var directionVector = Random.insideUnitSphere - normalizeVector;
        var targetPos = Vector3.ClampMagnitude(directionVector, 1.5f);
        float moveSpeed = 8.0f;
        float scaleSpeed = 2.5f;
        while (true)
        {
            if(ship.transform.localScale.x >= 1.0f)
            {
                ship.transform.localScale = Vector3.one;

                //Return ship to its normal state
                ship.GetComponent<SpaceShipController>().currentStatus = ShipStatus.NORMAL;
                ship.GetComponent<SpaceShipMovement>().currentStatus = ShipStatus.NORMAL;
                ship.GetComponent<SpaceShipController>().BeginShoot();
                yield break;
            }
            ship.transform.position = Vector3.MoveTowards(ship.transform.position, targetPos, moveSpeed * Time.deltaTime);
            ship.transform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;
            yield return null;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(wormholeType == WormHoleType.GateIn)
            {
                collision.GetComponent<SpaceShipController>().currentStatus = ShipStatus.DISABLE;
                collision.GetComponent<SpaceShipMovement>().currentStatus = ShipStatus.DISABLE;
                collision.GetComponent<SpaceShipController>().StopShoot();
                DoVancumn(collision.gameObject);
            }               
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (isShowGizmo)
        {
            if (wormholeType == WormHoleType.GateIn)
            {
                Gizmos.color = new Color(0, 1.0f, 0, 1.0f);
                Gizmos.DrawSphere(transform.position, 0.2f);
            }
            if (wormholeType == WormHoleType.GateOut)
            {
                Gizmos.color = new Color(1.0f, 0, 0, 1.0f);
                Gizmos.DrawSphere(transform.position, 0.2f);
            }
        }
    }
#endif
}
