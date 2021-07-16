using UnityEngine;

public class FlyingSlimeController : MonsterWithCustomPath
{
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

        //Remember to unsubribe event before destroy something
        bezierMoveController.OnMoveEnd -= OnMoveEnd;
        bezierMoveController.OnMoveEnd += OnMoveEnd;
        bezierMoveController.SetPath(intro);
        bezierMoveController.Stop();
        bezierMoveController.StartMove(LoopType.None);

        currentMovementState = MovementState.INTRO;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (GameHelper.IsInsideScreenBounds(transform.position))
        {
            //if (collision.CompareTag("Environment"))
            //{
            //    if (collision.GetComponent<BaseEnvironment>().id == 2)
            //    {
            //        //if transformed then get hit and lose HP
            //        UpdateHP(collision.GetComponent<BaseEnvironment>().impactDamage);
            //        collision.GetComponent<BaseEnvironment>().currentHP -= baseImpactDamage;
            //    }
            //}
        }
    }
}