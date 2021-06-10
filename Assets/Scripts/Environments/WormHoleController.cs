using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;

 public enum WormHoleType
{
    None        = 0,
    GateIn      = 1,
    GateOut     = 2,
}

public class WormHoleController : EnvironmentWithCustomPath
{
    public bool isShowGizmo = true;
    public float moveSpeed;
    public WormHoleType wormholeType;
    public Transform gateOutTransform;


    private Coroutine launchOutCoroutine;
    private Coroutine doVacumnCoroutine;

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
    private void DoVancumnMonster(GameObject monster)
    {
        StartCoroutine(OnVancumningMonster(monster));
    }
    private IEnumerator OnVancumningMonster(GameObject monster)
    {
        float moveSpeed = 4.0f;
        float scaleSpeed = 2.5f;
        while (true)
        {
            if (monster.transform.localScale.x <= 0)
            {
                monster.transform.localScale = Vector3.zero;
                monster.transform.position = gateOutTransform.position;
                LaunchOutMonster(monster);
                yield break;
            }
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, transform.position, moveSpeed * Time.deltaTime);
            monster.transform.localScale -= scaleSpeed * Time.deltaTime * Vector3.one;
            yield return null;
        }
    }
    private void LaunchOutMonster(GameObject monster)
    {
        LeanTween.scale(monster, Vector3.one, 0.5f);
        StartCoroutine(ScaleUpMonster(monster));
    }
    private IEnumerator ScaleUpMonster(GameObject monster)
    {
        float moveSpeed = 3.0f;
        Vector3 targetPos = GameManager.Instance.spaceShip.transform.position;
        float timer = 0;
        while (true)
        {
            if (timer >= 2.0f)
            {
                monster.GetComponent<BaseMonster>().currentHP = 0;
                yield break;
            }
           
            monster.transform.position = Vector3.MoveTowards(
                monster.transform.position,
                 Vector3.Lerp(monster.transform.position,targetPos,0.1f),
                moveSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }
    private void DoVancumnShip(GameObject ship)
    {
        ship.GetComponent<Animator>().Play("ship_rotate");
        if(launchOutCoroutine!=null)
        {
            StopCoroutine(launchOutCoroutine);
        }       
        //Reset everything for safety
        ship.transform.rotation = Quaternion.identity;
        doVacumnCoroutine = StartCoroutine(OnVancumning(ship));
    }
    private IEnumerator OnVancumning(GameObject ship)
    {
        float moveSpeed = 4.0f;
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
            ship.transform.position = Vector3.MoveTowards(ship.transform.position, transform.position, moveSpeed * Time.deltaTime);
            ship.transform.localScale -= scaleSpeed * Time.deltaTime * Vector3.one;
            yield return null;
        }
    }
    private void LaunchOut(GameObject ship)
    {
        StopCoroutine(doVacumnCoroutine);
        ship.GetComponent<Animator>().Play("ship_rotate");
        launchOutCoroutine = StartCoroutine(ScaleUp(ship));
    }
    private IEnumerator ScaleUp(GameObject ship)
    {
        var normalizeVector = gateOutTransform.position.normalized;
        var directionVector = Random.insideUnitSphere - normalizeVector;
        var targetPos = Vector3.ClampMagnitude(directionVector, 1.5f);
        float moveSpeed = 4.0f;
        float scaleSpeed = 2.5f;
        while (true)
        {
            if(ship.transform.localScale.x >= 1.0f)
            {
                ship.transform.localScale = Vector3.one;

                //Return ship to its normal state
                ship.GetComponent<Animator>().Play("ship_idle");
                ship.GetComponent<SpaceShipController>().currentStatus = ShipStatus.NORMAL;
                ship.GetComponent<SpaceShipMovement>().currentStatus = ShipStatus.NORMAL;
                ship.GetComponent<SpaceShipController>().BeginShoot();
                yield break;
            }
            ship.transform.position = Vector3.MoveTowards(
                ship.transform.position, 
                gateOutTransform.GetChild(0).position, 
                moveSpeed * Time.deltaTime);
            ship.transform.localScale += scaleSpeed * Time.deltaTime * Vector3.one;
            yield return null;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (wormholeType == WormHoleType.GateIn)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<SpaceShipController>().currentStatus = ShipStatus.DISABLE;
                collision.GetComponent<SpaceShipMovement>().currentStatus = ShipStatus.DISABLE;
                collision.GetComponent<SpaceShipController>().StopShoot();
                DoVancumnShip(collision.gameObject);
            }
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<MonsterWithCustomPath>().Stop();

                DoVancumnMonster(collision.gameObject);
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
            if(wormholeType== WormHoleType.GateOut)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(transform.GetChild(0).position, 0.3f * Vector3.one);
                Gizmos.DrawLine(transform.position, transform.GetChild(0).position);
            }

        }
    }
#endif
}
