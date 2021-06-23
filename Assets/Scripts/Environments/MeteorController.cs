using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;

public class MeteorController : EnvironmentWithCustomPath
{
    public System.Action<int> OnHit;
    public override void Setup()
    {
        base.Setup();
    }
    public override void Spawn()
    {
        base.Spawn();
        if(OnHit==null)
        {
            OnHit+= GameManager.Instance.spaceShip.GetComponent<SpaceShipController>().HandleGetHitByEntity;
        }
        Move();
    }
    public override void Move()
    {
        base.Move();
        GetComponent<Animator>().Play("meteor_idle");
        GetComponent<Animator>().SetFloat("RotationSpeedModifier", 2.0f);

        bezierMoveController.OnMoveEnd -= OnMoveEnd;
        bezierMoveController.OnMoveEnd += OnMoveEnd;
        bezierMoveController.SetPath(intro);
        bezierMoveController.Stop();
        bezierMoveController.StartMove(LoopType.None);

        //moveController.movementEndEvent -= OnMoveEnd;
        //moveController.movementEndEvent += OnMoveEnd;
        //moveController.pathContainer = introPath;
        //moveController.loopType = splineMove.LoopType.none;
        //moveController.moveToPath = false;
        //moveController.StartMove();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            currentHP-=collision.GetComponent<BaseBullet>().Damage;
            if(currentHP <=0)
            {
                //moveController.Stop();
                FXManager.Instance.CreateFX(1, transform.position);
                OnMoveEnd();
            }
            collision.GetComponent<BaseBullet>().Reset();
        }
        if (collision.CompareTag("Player"))
        {
            //Current ship have no HP so set it to 1. If changed the system to HP number based,
            //could switch to the bigger number easily
            OnHit?.Invoke(1);
            bezierMoveController.Stop();

            //moveController.Stop();
            FXManager.Instance.CreateFX(1, transform.position);
            OnMoveEnd();
        }
        if (collision.CompareTag("Enemy"))
        {

        }
        
    }
}
