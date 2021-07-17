using System.Collections;
using UnityEngine;

public class DodgeMonsterController : MonsterWithCustomPath
{
    [Header("Dodging Behaviour")]
    public CircleCollider2D circleCollider;

    public int dodgeCount;
    private int counter;
    private Transform currentShipTransform;

    private void Start()
    {
        currentShipTransform = GameManager.Instance.spaceShip.transform;
    }

    public override void Setup()
    {
        counter = dodgeCount;
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
        //Remember to unsubribe event before destroy something
        bezierMoveController.OnMoveEnd -= OnMoveEnd;
        bezierMoveController.OnMoveEnd -= ChasingAfterShip;
        bezierMoveController.path = intro;
        bezierMoveController.OnMoveEnd += ChasingAfterShip;
        bezierMoveController.Stop();
        bezierMoveController.StartMove(LoopType.None);

        currentMovementState = MovementState.INTRO;
    }

    private void ChasingAfterShip()
    {
        StartCoroutine(OnChasing());
    }

    private IEnumerator OnChasing()
    {
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentShipTransform.position, data.baseMoveSpd * Time.deltaTime);
            yield return null;
        }
    }

    private void DoDodgeIncomingBullet(Collider2D bullet_collider)
    {
        Vector3 nextPos = transform.position;

        //Check the side of bullet, if it's on the left side, monster will dodge to the right side and vice versa
        if (bullet_collider.transform.position.x > transform.position.x)
        {
            nextPos.x -= Random.Range(0.8f, 1.5f);
        }
        else
        {
            nextPos.x += Random.Range(0.8f, 1.5f);
        }

        nextPos.y += 0.8f;

        LeanTween.move(gameObject, nextPos, 0.3f)
            .setOnComplete(OnFinishDodge)
            .setEase(LeanTweenType.easeOutQuart);
    }

    private void OnFinishDodge()
    {
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (counter > 0)
            {
                if (circleCollider.IsTouching(collision))
                {
                    //Stop current path moving and resume later after dodging.
                    //This could cause weird teleport back to path!
                    DoDodgeIncomingBullet(collision);
                    counter--;
                }
            }
            else
            {
                base.OnTriggerEnter2D(collision);
            }
        }
    }
}