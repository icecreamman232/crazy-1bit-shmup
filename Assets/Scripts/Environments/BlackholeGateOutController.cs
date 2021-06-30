using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BlackholeGateOutController : EnvironmentWithCustomPath
{
    public bool isShowGizmo = true;
    public BlackholeGateInController gateIn;
    public Coroutine pushCoroutine;

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
   
    public void PushingOutMonster(GameObject monster,Vector3 originScale)
    {
        StopCoroutine(gateIn.pullingCoroutine);
        gateIn.pullingCoroutine = null;
        LeanTween.scale(monster, originScale, 0.5f);
        pushCoroutine  = StartCoroutine(ScaleUpMonster(monster));
    }
    private IEnumerator ScaleUpMonster(GameObject monster)
    {
        float moveSpeed = 3.0f;
        Vector3 targetPos = GameManager.Instance.spaceShip.transform.position;
        float timer = 0;
        float xSigned = 0;
        float ySigned = 0;
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
        StopCoroutine(gateIn.pullingCoroutine);
        gateIn.pullingCoroutine = null;
        ship.GetComponent<Animator>().Play("ship_rotate");
        pushCoroutine = StartCoroutine(ScaleUp(ship));
    }
    private IEnumerator ScaleUp(GameObject ship)
    {
        var normalizeVector = transform.position.normalized;
        var directionVector = Random.insideUnitSphere - normalizeVector;
        var targetPos = Vector3.ClampMagnitude(directionVector, 1.5f);
        float moveSpeed = 4.0f;
        float scaleSpeed = 2.5f;
        while (true)
        {
            if (ship.transform.localScale.x >= 1.0f)
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
                transform.GetChild(0).position,
                moveSpeed * Time.deltaTime);
            ship.transform.localScale += scaleSpeed * Time.deltaTime * Vector3.one;
            yield return null;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
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
