using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeGateInController : EnvironmentWithCustomPath
{
    public bool isShowGizmo = true;
    public BlackholeGateOutController gateOut;
    public float moveSpeed;
    public Coroutine pullingCoroutine;

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
        bezierMoveController.OnMoveEnd -= OnMoveEnd;
        bezierMoveController.OnMoveEnd += OnMoveEnd;
        bezierMoveController.SetPath(intro);
        bezierMoveController.Stop();
        bezierMoveController.StartMove(LoopType.None);


    }
    #region Handling pulling behaviour
    private void PullingShip(GameObject ship)
    {
        ship.GetComponent<Animator>().Play("ship_rotate");
        if (gateOut.pushCoroutine != null)
        {
            StopCoroutine(gateOut.pushCoroutine);
            gateOut.pushCoroutine = null;
        }
        //Reset everything for safety
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
                ship.GetComponent<Animator>().Play("ship_idle");
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
        if (gateOut.pushCoroutine != null)
        {
            StopCoroutine(gateOut.pushCoroutine);
            gateOut.pushCoroutine = null;
        }
        pullingCoroutine = StartCoroutine(OnPullingMonster(monster));
    }
    private IEnumerator OnPullingMonster(GameObject monster)
    {
        float moveSpeed = 4.0f;
        float scaleSpeed = 2.5f;
        while (true)
        {
            if (monster.transform.localScale.x <= 0)
            {
                monster.transform.localScale = Vector3.zero;
                monster.transform.position = gateOut.gameObject.transform.position;
                gateOut.PushingOutMonster(monster);
                yield break;
            }
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, transform.position, moveSpeed * Time.deltaTime);
            monster.transform.localScale -= scaleSpeed * Time.deltaTime * Vector3.one;
            yield return null;
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(GameHelper.IsInsideScreenBounds(gateOut.gameObject.transform.position))
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<SpaceShipController>().currentStatus = ShipStatus.DISABLE;
                collision.GetComponent<SpaceShipMovement>().currentStatus = ShipStatus.DISABLE;
                collision.GetComponent<SpaceShipController>().StopShoot();
                PullingShip(collision.gameObject);
            }
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<MonsterWithCustomPath>().Stop();
                PullingMonster(collision.gameObject);
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
